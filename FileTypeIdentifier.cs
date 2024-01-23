using Newtonsoft.Json;

namespace WinGPT;

public enum FileType {
   Image,
   Code,
   Documents,
   Other
}

public static class FileTypeIdentifier {
   private static readonly Dictionary<FileType, List<string>> _fileTypes;
   private const string DefaultFileTypesJsonFileName = "FileTypes.json";

   static FileTypeIdentifier () {
      try {
         _fileTypes = LoadFileTypes(DefaultFileTypesJsonFileName);
      } catch (Exception) {
         MessageBox.Show("Failed to load file types from JSON file. A new default file will be generated.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
         GenerateDefaultFileTypesJson();
         _fileTypes = LoadFileTypes(DefaultFileTypesJsonFileName);
      }
   }

   private static Dictionary<FileType, List<string>> LoadFileTypes(string jsonFilePath) {
      var json      = File.ReadAllText(jsonFilePath);
      var fileTypes = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(json);

      if (fileTypes == null)
         throw new Exception("Failed to load file types from JSON file.");

      return fileTypes.ToDictionary(
         kvp => Enum.Parse<FileType>(kvp.Key),
         kvp => kvp.Value.Select(ext => ext.ToLower()).ToList());
   }

   public static FileType GetFileType(string filePath) {
      var extension = Path.GetExtension(filePath).ToLower();

      foreach (var fileType in _fileTypes.Where(fileType
                  => fileType.Value.Contains(extension))) {
         return fileType.Key;
      }

      return FileType.Other;
   }

   private static void GenerateDefaultFileTypesJson() {
      var defaultFileTypes = new Dictionary<string, List<string>> {
         { "Image", new List<string> { ".bmp", ".gif", ".jpeg", ".jpg", ".png" } },
         { "Code", new List<string> { ".cpp", ".cs", ".css", ".go", ".html", ".java", ".js", ".json", ".md", ".php", ".py", ".rb", ".ts", ".vb", ".xml" } },
         { "Text", new List<string> { ".doc", ".docx", ".pdf", ".ppt", ".pptx", ".txt", ".xls", ".xlsx" } }
      };

      var json = JsonConvert.SerializeObject(defaultFileTypes, Formatting.Indented);
      File.WriteAllText(DefaultFileTypesJsonFileName, json);
   }
}