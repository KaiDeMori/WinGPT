namespace WinGPT;

public static class TreeViewHelper {
   public static TreeNode? FindNode(string path, DirectoryInfo baseDirectory, TreeView treeView) {
      // Get the relative path from the directoryName to the rootPath
      var relativePath = Path.GetRelativePath(baseDirectory.FullName, path);
      var pathParts    = relativePath.Split(Path.DirectorySeparatorChar);

      // Set the root node as current node to start with
      TreeNode? currentNode = treeView.Nodes[0];

      foreach (var part in pathParts) {
         var treeNodes = currentNode.Nodes.Find(part, false);
         if (treeNodes.Length > 1)
            throw new Exception("how can this happen on a windows system?");

         if (treeNodes.Length == 0)
            return null;

         currentNode = treeNodes[0];
      }

      return currentNode;
   }

   public static TreeNode? FindNode(FileSystemInfo? fileSystemInfo, DirectoryInfo baseDirectory, TreeView treeView) {
      if (fileSystemInfo == null)
         return null;
      var path = fileSystemInfo.FullName;
      return FindNode(path, baseDirectory, treeView);
   }
}