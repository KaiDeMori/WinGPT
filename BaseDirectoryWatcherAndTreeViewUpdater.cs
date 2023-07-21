using System.Diagnostics.CodeAnalysis;

namespace WinGPT;

public class BaseDirectoryWatcherAndTreeViewUpdater : IDisposable {
   private readonly TreeView          treeView;
   private readonly DirectoryInfo     baseDirectory;
   private readonly FileSystemWatcher fileSystemWatcher;
   private readonly HashSet<string>   expandedNodes;

   private FileSystemInfo? selectedNode;
   private FileSystemInfo? node_to_select;

   public void SelectNode(FileInfo file) {
      //we first have to check if the node is already in the treeview and select it if so
      if (TryFindNodeRecursive(treeView.Nodes, file.FullName, out var node)) {
         treeView.SelectedNode = node;
      }
      else {
         node_to_select = file;
      }
   }

   public void SelectNothing() {
      selectedNode          = null;
      treeView.SelectedNode = null;
   }

   public BaseDirectoryWatcherAndTreeViewUpdater(DirectoryInfo base_directory, TreeView treeView) {
      this.treeView      = treeView;
      this.baseDirectory = base_directory;
      this.expandedNodes = new HashSet<string>();

      treeView.Nodes.Clear();
      InitializeTreeView(base_directory);
      this.fileSystemWatcher = InitFileSystemWatcher(base_directory);

      fileSystemWatcher.EnableRaisingEvents = true;
   }

   private FileSystemWatcher InitFileSystemWatcher(DirectoryInfo base_directory) {
      var watcher = new FileSystemWatcher {
         Path                  = base_directory.FullName,
         Filter                = Config.marf278down_filter,
         NotifyFilter          = NotifyFilters.FileName | NotifyFilters.DirectoryName,
         IncludeSubdirectories = true
      };

      watcher.Created += OnFileSystemEvent;
      watcher.Changed += OnFileSystemEvent;
      watcher.Deleted += OnFileSystemEvent;
      watcher.Renamed += OnRenamed;
      watcher.Error   += OnError;


      return watcher;
   }

   private void OnError(object sender, ErrorEventArgs e) {
      //TADA: log error
   }

   private void InitializeTreeView(DirectoryInfo base_directory, TreeNode? parentNode = null) {
      var directoryNode = new TreeNode(base_directory.Name) {Tag = base_directory};

      if (parentNode == null) {
         treeView.Nodes.Add(directoryNode);
      }
      else {
         parentNode.Nodes.Add(directoryNode);
      }

      foreach (var directory in base_directory.GetDirectories()) {
         InitializeTreeView(directory, directoryNode);
      }

      foreach (var file in base_directory.GetFiles(Config.marf278down_filter)) {
         directoryNode.Nodes.Add(new TreeNode(file.Name) {Tag = file});
      }
   }


   private void OnFileSystemEvent(object sender, FileSystemEventArgs e) {
      InvokeIfNeeded(() => {
         SaveTreeViewState();
         treeView.BeginUpdate();

         if (e.ChangeType == WatcherChangeTypes.Deleted) {
            if (TryFindNodeRecursive(treeView.Nodes, e.FullPath, out var node)) {
               expandedNodes.Remove(node.FullPath);
               node.Remove();
            }
            else {
               RefreshTreeView();
            }
         }
         else {
            if (TryFindNodeRecursive(treeView.Nodes, e.FullPath, out var node)) {
               var file = CreateFileSystemInfo(e.FullPath);
               node.Tag = file;
               if (e.ChangeType == WatcherChangeTypes.Created) {
                  check_if_new_file_should_be_selected(file, node);
               }
            }
            else {
               RefreshTreeView();
            }
         }

         treeView.EndUpdate();
         RestoreTreeViewState();
      });
   }

   private void check_if_new_file_should_be_selected(FileSystemInfo file, TreeNode node) {
      if (node_to_select != null && node_to_select.FullName == file.FullName) {
         treeView.SelectedNode = node;
         node_to_select        = null;
      }
   }

   private void OnRenamed(object sender, RenamedEventArgs e) {
      InvokeIfNeeded(() => {
         SaveTreeViewState();
         treeView.BeginUpdate();

         if (TryFindNodeRecursive(treeView.Nodes, e.OldFullPath, out var node)) {
            node.Text = e.Name;
            node.Tag  = CreateFileSystemInfo(e.FullPath);

            var parentPath = Path.GetDirectoryName(e.FullPath);
            if (parentPath != null && TryFindNodeRecursive(treeView.Nodes, parentPath, out var parentNode)) {
               parentNode.Nodes.Add(node);
            }
         }
         else {
            RefreshTreeView();
         }

         treeView.EndUpdate();
         RestoreTreeViewState();
      });
   }

   private FileSystemInfo CreateFileSystemInfo(string path) =>
      Directory.Exists(path) ? new DirectoryInfo(path) : new FileInfo(path);

   private bool TryFindNodeRecursive(
      TreeNodeCollection                nodes,
      string                            path,
      [NotNullWhen(true)] out TreeNode? result
   ) {
      foreach (TreeNode node in nodes) {
         if (node.FullPath == path) {
            result = node;
            return true;
         }

         if (TryFindNodeRecursive(node.Nodes, path, out result)) {
            return true;
         }
      }

      result = null;
      return false;
   }


   private void SaveTreeViewState() {
      expandedNodes.Clear();
      SaveExpandedNodes(treeView.Nodes);
      selectedNode = treeView.SelectedNode?.Tag as FileSystemInfo;
   }

   private void SaveExpandedNodes(TreeNodeCollection nodes) {
      foreach (TreeNode node in nodes) {
         if (node.IsExpanded) {
            expandedNodes.Add(node.FullPath);
         }

         SaveExpandedNodes(node.Nodes);
      }
   }

   private void RestoreTreeViewState() {
      RestoreExpandedNodes(treeView.Nodes);
   }

   private void RestoreExpandedNodes(TreeNodeCollection nodes) {
      foreach (TreeNode node in nodes) {
         if (node.Tag is FileSystemInfo info) {
            if (expandedNodes.Contains(info.FullName)) {
               node.Expand();
            }
            else {
               node.Collapse();
            }
         }

         if ((node.Tag as FileSystemInfo)?.FullName == selectedNode?.FullName) {
            treeView.SelectedNode = node;
         }

         RestoreExpandedNodes(node.Nodes);
      }
   }

   public void Dispose() {
      fileSystemWatcher.Dispose();
   }

   private void InvokeIfNeeded(Action action) {
      if (treeView.InvokeRequired) {
         treeView.Invoke((MethodInvoker) delegate { action(); });
      }
      else {
         action();
      }
   }

   private void RefreshTreeView() {
      treeView.Nodes.Clear();
      InitializeTreeView(baseDirectory);
   }
}