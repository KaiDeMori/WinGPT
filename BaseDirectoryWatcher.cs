namespace WinGPT;

public class BaseDirectoryWatcher {
   private readonly FileSystemWatcher fileSystemWatcher;
   private readonly TreeView          treeView;

   public BaseDirectoryWatcher(DirectoryInfo path, TreeView treeView) {
      this.treeView = treeView;

      // Create the FileSystemWatcher and set its properties
      fileSystemWatcher = new FileSystemWatcher {
         Path                  = path.FullName,
         NotifyFilter          = NotifyFilters.FileName | NotifyFilters.DirectoryName,
         Filter                = Config.marf278down_extenstion, // Watch all markdown files
         IncludeSubdirectories = true    // Watch for changes in subdirectories
      };

      // Add event handlers
      fileSystemWatcher.Created += FileSystemWatcher_Created;
      fileSystemWatcher.Deleted += FileSystemWatcher_Deleted;
      fileSystemWatcher.Renamed += FileSystemWatcher_Renamed;

      // Begin watching
      fileSystemWatcher.EnableRaisingEvents = true;
   }

   private void FileSystemWatcher_Created(object sender, FileSystemEventArgs e) {
      //check if the file is a file or a directory
      if (File.GetAttributes(e.FullPath).HasFlag(FileAttributes.Directory)) {
         //give up for now an just refresh the whole tree
         InitializeTree();
      }
      else {
         var file = new FileInfo(e.FullPath);
         var node = new TreeNode(file.Name) {
            Tag = file
         };
         //find the correct node to add to
         var parent = FindNodeByTag(treeView.Nodes, file);
         treeView.Invoke(() => { parent?.Nodes.Add(node); });
      }
   }

   private void FileSystemWatcher_Deleted(object sender, FileSystemEventArgs e) {
      //check if the file is a file or a directory
      //of a deleted file?! how's that gonna work??
      //e.Name;
      //FileSystemInfo info = File.GetAttributes(e.FullPath).HasFlag(FileAttributes.Directory)
      //   ? new DirectoryInfo(e.FullPath)
      //   : new FileInfo(e.FullPath);
      //treeView.Invoke(() => {
      //   var node = FindNodeByTag(treeView.Nodes, info);
      //   node?.Remove();
      //});
   }

   private void FileSystemWatcher_Renamed(object sender, RenamedEventArgs e) {
      //check if the file is a file or a directory
      FileSystemInfo info = File.GetAttributes(e.FullPath).HasFlag(FileAttributes.Directory)
         ? new DirectoryInfo(e.FullPath)
         : new FileInfo(e.FullPath);

      //if its directory, give up for now and just refresh the whole tree
      if (info is DirectoryInfo) {
         InitializeTree();
      }
      else if (info is FileInfo file) {
         //find the correct node to rename
         var node = FindNodeByTag(treeView.Nodes, file);
         if (node == null)
            return;
         treeView.Invoke(() => {
            node.Text = file.Name;
            node.Tag  = file;
         });
      }
   }

   public void InitializeTree() {
      // Clear any existing nodes
      treeView.Nodes.Clear();

      // Create root node
      var rootpath = new DirectoryInfo(fileSystemWatcher.Path);
      TreeNode rootNode = new(rootpath.Name) {
         Tag = rootpath
      };

      PopulateNode(rootNode, rootpath);
      treeView.Nodes.Add(rootNode);
   }

   private static void PopulateNode(TreeNode parentNode, DirectoryInfo path) {
      var directories = path.GetDirectories();
      var files       = path.GetFiles();

      foreach (var directory in directories) {
         TreeNode directoryNode = new(directory.Name) {
            Tag = directory
         };

         PopulateNode(directoryNode, directory); // recursive call for other sub-directories
         parentNode.Nodes.Add(directoryNode);
      }

      foreach (var file in files) {
         TreeNode fileNode = new(file.Name) {
            Tag = file
         };

         parentNode.Nodes.Add(fileNode);
      }
   }

   private static TreeNode? FindNodeByTag(TreeNodeCollection nodes, FileSystemInfo tag) {
      foreach (TreeNode node in nodes) {
         if (node.Tag is FileSystemInfo info && info.FullName == tag.FullName) {
            return node;
         }

         // Recursive call for the node's children
         TreeNode? foundNode = FindNodeByTag(node.Nodes, tag);
         if (foundNode != null)
            return foundNode;
      }

      return null;
   }
}