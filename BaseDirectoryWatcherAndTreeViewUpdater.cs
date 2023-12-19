namespace WinGPT;

public class BaseDirectoryWatcherAndTreeViewUpdater : IDisposable {
   private readonly TreeView          _treeView;
   private readonly DirectoryInfo     baseDirectory;
   private readonly FileSystemWatcher fileSystemWatcher;
   public readonly  TreeViewPersistor treeViewPersistor;

   private FileSystemInfo? node_to_select;

   public BaseDirectoryWatcherAndTreeViewUpdater(DirectoryInfo base_directory, TreeView treeView) {
      this._treeView     = treeView;
      this.baseDirectory = base_directory;

      treeView.Nodes.Clear();
      treeView.ShowNodeToolTips = true;
      InitializeTreeView(base_directory);
      this.fileSystemWatcher = InitFileSystemWatcher(base_directory);

      this.treeViewPersistor = new TreeViewPersistor(treeView);
      treeViewPersistor.Load();
      
      fileSystemWatcher.EnableRaisingEvents = true;
   }

   private FileSystemWatcher InitFileSystemWatcher(DirectoryInfo base_directory) {
      var watcher = new FileSystemWatcher {
         Path = base_directory.FullName,
         //Filter                = Config.marf278down_filter,
         NotifyFilter          = NotifyFilters.FileName | NotifyFilters.DirectoryName,
         IncludeSubdirectories = true
      };

      watcher.Created += OnCreated;
      watcher.Changed += OnChanged;
      watcher.Deleted += OnDeleted;
      watcher.Renamed += OnRenamed;
      watcher.Error   += OnError;

      return watcher;
   }

   private void OnCreated(object sender, FileSystemEventArgs e) {
      FileSystemInfo systemfileinfo = CreateFileSystemInfo(e.FullPath);

      var intermediateDirs = Tools.GetRelativeDirectories(baseDirectory, systemfileinfo);
      InvokeIfNeeded(() => {
         //create all necessary subdirectories if they don't exist yet
         var rootNode    = _treeView.Nodes[0];
         var currentNode = rootNode;
         foreach (var dir in intermediateDirs) {
            var node = currentNode.Nodes.Find(dir.Name, false).FirstOrDefault();
            if (node == null) {
               node = new FileTreeNode(dir);
               currentNode.Nodes.Add(node);
            }

            currentNode = node;
         }

         //if the filesysteminfo is a fileinfo, we need to add it to the treeview
         //but only if its a markf278down file
         if (systemfileinfo is FileInfo file && file.Name.EndsWith(Config.marf278down_extenstion)) {
            currentNode.Nodes.Add(new FileTreeNode(file));
         }

         check_if_new_file_should_be_selected(systemfileinfo);
      });
   }

   private void OnRenamed(object sender, RenamedEventArgs e) {
      //var old_file = CreateFileSystemInfo(e.OldFullPath);
      var new_file = CreateFileSystemInfo(e.FullPath);

      InvokeIfNeeded(() => {
         var node = TreeViewHelper.FindNode(e.OldFullPath, baseDirectory, _treeView);
         if (node == null)
            return;

         node.Text = e.Name;
         node.Tag  = new_file;
         treeViewPersistor.SelectNode(node);
      });
   }

   private void OnChanged(object sender, FileSystemEventArgs e) {
      //not sure when this is called exactly
   }

   private void OnDeleted(object sender, FileSystemEventArgs e) {
      InvokeIfNeeded(() => {
         var node = TreeViewHelper.FindNode(e.FullPath, baseDirectory, _treeView);

         node?.Remove();
      });
   }

   private void OnError(object sender, ErrorEventArgs e) {
      //TADA: log error
   }

   private void InitializeTreeView(DirectoryInfo base_directory, TreeNode? parentNode = null) {
      var directoryNode = new FileTreeNode(base_directory);

      if (parentNode == null) {
         _treeView.Nodes.Add(directoryNode);
      }
      else {
         parentNode.Nodes.Add(directoryNode);
      }

      foreach (var directory in base_directory.GetDirectories()) {
         InitializeTreeView(directory, directoryNode);
      }

      foreach (var file in base_directory.GetFiles(Config.marf278down_filter)) {
         directoryNode.Nodes.Add(new FileTreeNode(file));
      }
   }

   private TreeNode? GetParentNode(FileSystemInfo systemfileinfo) {
      var parent_node = systemfileinfo switch {
         FileInfo file           => TreeViewHelper.FindNode(file.Directory,   baseDirectory, _treeView),
         DirectoryInfo directory => TreeViewHelper.FindNode(directory.Parent, baseDirectory, _treeView),
         _                       => null
      };

      return parent_node;
   }

   private void check_if_new_file_should_be_selected(FileSystemInfo file) {
      if (node_to_select != null && node_to_select.FullName == file.FullName) {
         var success = treeViewPersistor.SelectNode(file);
         if (success)
            node_to_select = null;
      }
   }

   private FileSystemInfo CreateFileSystemInfo(string path) {
      var fileAttributes = File.GetAttributes(path);
      if (fileAttributes.HasFlag(FileAttributes.Directory))
         return new DirectoryInfo(path);
      return new FileInfo(path);
   }

   public void Dispose() {
      fileSystemWatcher.EnableRaisingEvents =false;
      fileSystemWatcher.Dispose();
   }

   private void InvokeIfNeeded(Action action) {
      if (_treeView.InvokeRequired) {
         _treeView.Invoke((MethodInvoker) delegate { action(); });
      }
      else {
         action();
      }
   }

   public void RefreshTreeView() {
      InvokeIfNeeded(() => {
         _treeView.BeginUpdate();
         treeViewPersistor.Save();
         _treeView.Nodes.Clear();
         InitializeTreeView(baseDirectory);
         _treeView.Refresh();
         treeViewPersistor.Load();
         _treeView.EndUpdate();
      });
   }

   public void SelectNode(FileSystemInfo conversationHistoryFile) {
      if (!treeViewPersistor.SelectNode(conversationHistoryFile))
         node_to_select = conversationHistoryFile;
   }
}

/// <summary>
/// seriously? The "Name" is different from setting the "Text" in the constructor. And the docs just talk abou "key" *facepalm*
/// </summary>
public class FileTreeNode : TreeNode {
   public FileTreeNode(FileSystemInfo fileSystemInfo) {
      if (fileSystemInfo is FileInfo file) {
         if (Conversation.TryParseConversationHistoryFile(file, out var conversation))
            ToolTipText = conversation.Info.Summary ?? "";
      }

      Text = fileSystemInfo.Name;
      Name = fileSystemInfo.Name;
      Tag  = fileSystemInfo;
   }
}