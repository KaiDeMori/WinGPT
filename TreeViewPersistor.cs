using Newtonsoft.Json;

namespace WinGPT;

public class TreeViewPersistor {
   private readonly TreeView      _treeView;
   private readonly DirectoryInfo baseDirectory;

   public TreeViewPersistor(TreeView treeView) {
      _treeView = treeView;
      baseDirectory = _treeView.Nodes[0].Tag switch {
         DirectoryInfo dir => dir,
         _                 => throw new Exception("Root node must be a DirectoryInfo")
      };
   }

   public void Save() {
      var state = new TreeViewState {
         NodesState           = new List<NodeAndState>(),
         LastSelectedNodePath = (_treeView.SelectedNode?.Tag as FileInfo)?.FullName
      };

      SaveNodeState(_treeView.Nodes, state.NodesState);

      var json = JsonConvert.SerializeObject(state);

      File.WriteAllText(Application_Paths.Treestate_File.FullName, json);
   }

   private void SaveNodeState(TreeNodeCollection nodes, List<NodeAndState> state) {
      foreach (TreeNode node in nodes) {
         if (node.Tag is DirectoryInfo dir) {
            state.Add(new NodeAndState {Fullname = dir.FullName, IsExpanded = node.IsExpanded});

            if (node.Nodes.Count > 0)
               SaveNodeState(node.Nodes, state);
         }
      }
   }

   public void Load() {
      if (!Application_Paths.Treestate_File.Exists) return;

      TreeViewState? state;
      try {
         var json = File.ReadAllText(Application_Paths.Treestate_File.FullName);
         state = JsonConvert.DeserializeObject<TreeViewState>(json);
      }
      catch {
         // just delete for now
         Application_Paths.Treestate_File.Delete();
         return;
      }

      if (state?.NodesState == null)
         return;

      // Create a Dictionary for efficient lookup
      Dictionary<string, NodeAndState> stateDict =
         state.NodesState
            .DistinctBy(s => s.Fullname)
            .ToDictionary(s => s.Fullname);

      _treeView.BeginUpdate();

      RestoreNodeState(_treeView.Nodes, stateDict);

      _treeView.EndUpdate();

      if (state.LastSelectedNodePath != null) {
         SelectNode(new FileInfo(state.LastSelectedNodePath));
      }
   }

   private void RestoreNodeState(TreeNodeCollection nodes, Dictionary<string, NodeAndState> stateDict) {
      foreach (TreeNode node in nodes) {
         if (node.Tag is DirectoryInfo dir) {
            var fullname = dir.FullName;

            if (stateDict.TryGetValue(fullname, out var nodeState)) {
               DoNode(node, nodeState.IsExpanded);
            }

            if (node.Nodes.Count > 0)
               RestoreNodeState(node.Nodes, stateDict);
         }
      }
   }

   private static void DoNode(TreeNode node, bool expand) {
      if (expand)
         node.Expand();
      else
         node.Collapse();
   }

   public void SelectNode(TreeNode node) {
      _treeView.SelectedNode = node;
      node.EnsureVisible();
   }

   public bool SelectNode(FileSystemInfo fileSystemInfo) {
      var node = TreeViewHelper.FindNode(fileSystemInfo, baseDirectory, _treeView);
      if (node == null)
         return false;
      node.EnsureVisible();
      _treeView.SelectedNode = node;
      return true;
   }

   //private string? GetNameFromNode(TreeNode node) {
   //   return node.Tag switch {
   //      FileInfo file     => file.Name,
   //      DirectoryInfo dir => dir.Name,
   //      _                 => null
   //   };
   //}
}

public class TreeViewState {
   public required List<NodeAndState> NodesState           { get; init; }
   public          string?            LastSelectedNodePath { get; set; }
}

public class NodeAndState {
   public required string Fullname   { get; init; }
   public required bool   IsExpanded { get; init; }
}