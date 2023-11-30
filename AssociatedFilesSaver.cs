namespace WinGPT;

public static class AssociatedFilesSaver {
   public static void SaveFile(string filename, string content, FileInfo[]? associated_files, out string dummy_assistant_content) {
      // Attempt to find the file in the associated files
      var fileToSave = associated_files?.FirstOrDefault(f => f.Name == filename);

      // If the file does not exist, create a new FileInfo in the basdir downloads path
      if (fileToSave == null) {
         fileToSave              = new FileInfo(Path.Join(Config.AdHoc_Downloads_Path.FullName, filename));
         dummy_assistant_content = $"The file {fileToSave.Name} was saved to the downloads folder.";
      }
      else {
         dummy_assistant_content = $"The file {fileToSave.Name} was overwritten in its original location.";
      }

      // Try to save the file and return the result message
      try {
         File.WriteAllText(fileToSave.FullName, content);
      }
      catch (Exception e) {
         dummy_assistant_content = $"Failed to save the file {fileToSave.Name} to the downloads folder. Error: {e.Message}";
      }
   }
}