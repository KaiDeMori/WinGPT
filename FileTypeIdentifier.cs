using Newtonsoft.Json;

namespace WinGPT;

public enum FileType {
   Image,
   Code,
   Document,
   Other
}

public static class FileTypeIdentifier {
   private static readonly Dictionary<FileType, List<string>> _fileTypes;

   private const string File_Types_Json_FileName = "File_Types.json";

   static FileTypeIdentifier() {
      try {
         _fileTypes = LoadFileTypes(File_Types_Json_FileName);
      }
      catch (Exception) {
         MessageBox.Show($"Failed to load file types from file {File_Types_Json_FileName} .", "Error", MessageBoxButtons.OK,
            MessageBoxIcon.Error);
      }
   }

   private static Dictionary<FileType, List<string>> LoadFileTypes(string jsonFilePath) {
      var json                  = File.ReadAllText(jsonFilePath);
      var file_types_dictionary = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(json);

      if (file_types_dictionary == null)
         throw new Exception("Failed to load file types from JSON file.");

      return file_types_dictionary.ToDictionary(
         kvp => Enum.Parse<FileType>(kvp.Key),
         kvp => kvp.Value.Select(ext => "." + ext.Key.ToLower()).ToList()
      );
   }

   public static FileType GetFileType(FileInfo file) => GetFileType(file.FullName);

   public static FileType GetFileType(string file_fullname) {
      var extension = Path.GetExtension(file_fullname).ToLower();

      foreach (var fileType in _fileTypes.Where(fileType => fileType.Value.Contains(extension)))
         return fileType.Key;

      return FileType.Other;
   }
}