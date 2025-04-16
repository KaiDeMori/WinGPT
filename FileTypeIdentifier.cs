namespace WinGPT;

public enum FileType {
   Image,
   Code,
   Document,
   Other
}

public static class FileTypeIdentifier {
   private static readonly Dictionary<FileType, List<string>> File_Types;

   private const string File_Types_Json_FileName = "File_Types.json";

   static FileTypeIdentifier() {
      try {
         File_Types = LoadFileTypes(File_Types_Json_FileName);
      }
      catch (Exception e) {
         File_Types = new();
         MessageBox.Show(
            $"Failed to load file types from {File_Types_Json_FileName}: {e.Message}",
            "Error",
            MessageBoxButtons.OK,
            MessageBoxIcon.Error
         );
      }
   }

   private static Dictionary<FileType, List<string>> LoadFileTypes(string jsonFilePath) {
      var file_types_dictionary = Defaults.DefaultFilesHandler
         .load_json_file<Dictionary<string, List<string>>>(jsonFilePath);

      if (file_types_dictionary == null)
         throw new Exception("Failed to load file types from JSON file.");

      return file_types_dictionary.ToDictionary(
         kvp => Enum.Parse<FileType>(kvp.Key),
         kvp => kvp.Value.Select(ext => "." + ext.ToLower()).ToList()
      );
   }

   public static FileType GetFileType(FileInfo file) => GetFileType(file.FullName);

   public static FileType GetFileType(string file_fullname) {
      var extension = Path.GetExtension(file_fullname).ToLower();

      foreach (var fileType in File_Types.Where(fileType => fileType.Value.Contains(extension)))
         return fileType.Key;

      return FileType.Other;
   }
}