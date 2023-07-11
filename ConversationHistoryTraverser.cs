namespace WinGPT;

internal class ConversationHistoryTraverser {
   public static TreeNode TraverseTree(DirectoryInfo targetDirectory) {
      var root = new TreeNode(targetDirectory.FullName);

      // Process all files directly in the directory
      var fileEntries = Directory.GetFiles(targetDirectory.FullName);
      foreach (var fileName in fileEntries) {
         var file = new FileInfo(fileName);
         if (file.Extension == Config.marf278down_extenstion) {
            var node = new TreeNode(file.Name) {
               Tag = file // Associate FileInfo with TreeNode
            };
            root.Nodes.Add(node);
         }
      }

      // Recurse into subdirectories
      targetDirectory.Refresh();
      DirectoryInfo[] subdirectories = targetDirectory.GetDirectories();
      //var subdirectoryEntries = Directory.GetDirectories(targetDirectory);
      foreach (var subdirectory in subdirectories) {
         var subNode = TraverseTree(subdirectory);
         if (subNode.Nodes.Count > 0) {
            root.Nodes.Add(subNode);
         }
      }

      return root;
   }

   private static void UpdateTreeView(DirectoryInfo targetDirectory, TreeView treeView) {
      if (treeView.InvokeRequired)
         treeView.Invoke(() => UpdateTreeView_unsafe(targetDirectory, treeView));
      else
         UpdateTreeView_unsafe(targetDirectory, treeView);
   }

   private static void UpdateTreeView_unsafe(DirectoryInfo targetDirectory, TreeView treeView) {
      // Store full path of selected node
      string? selectedPath = null;
      if (treeView.SelectedNode?.Tag is FileInfo selectedFile) {
         selectedPath = selectedFile.FullName;
      }

      treeView.Nodes.Clear(); // Clear the existing tree

      var root = TraverseTree(targetDirectory); // Rebuild the tree
      if (root.Nodes.Count > 0) {
         treeView.Nodes.Add(root); // Add the new tree
      }

      if (selectedPath == null)
         return;

      // If a node was previously selected, try to select it again
      var node = FindNodeByPath(treeView.Nodes, selectedPath);
      if (node is not null)
         treeView.SelectedNode = node;
   }

   public static TreeNode? FindNodeByPath(TreeNodeCollection nodes, string? path) {
      foreach (TreeNode node in nodes) {
         if (node.Tag is FileInfo file && file.FullName == path)
            return node;

         var found = FindNodeByPath(node.Nodes, path);
         if (found is not null)
            return found;
      }

      return null;
   }

   public static void StartWatching(DirectoryInfo targetDirectory, TreeView treeView) {
      var watcher = new FileSystemWatcher {
         Path = targetDirectory.FullName,
         NotifyFilter = NotifyFilters.LastAccess |
                        NotifyFilters.LastWrite  |
                        NotifyFilters.FileName   |
                        NotifyFilters.DirectoryName,
         Filter = Config.marf278down_filter
      };

      // Add event handlers.
      watcher.Changed += (_, _) => UpdateTreeView(targetDirectory, treeView);
      watcher.Created += (_, _) => UpdateTreeView(targetDirectory, treeView);
      watcher.Deleted += (_, _) => UpdateTreeView(targetDirectory, treeView);
      watcher.Renamed += (_, _) => UpdateTreeView(targetDirectory, treeView);

      UpdateTreeView(targetDirectory, treeView);

      // Begin watching.
      watcher.EnableRaisingEvents = true;
   }
}