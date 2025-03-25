using Newtonsoft.Json;

namespace WinGPT;

/// <summary>
/// We used to distinguish between images and documents, but times change!
/// Now we unify everything in one place. 🤗
/// </summary>
public static class FileHelper {
   /// <summary>
   /// A dictionary that maps file extensions to their associated MIME type.
   /// </summary>
   private static readonly Dictionary<string, string> extension_mime_map = new(StringComparer.OrdinalIgnoreCase);

   private const string File_Types_Json_FileName = "File_Types.json";

   static FileHelper() {
      try {
         var all_types = Defaults.DefaultFilesHandler.load_json_file<Dictionary<string, Dictionary<string, string>>>(File_Types_Json_FileName);

         if (all_types == null)
            throw new Exception("Failed to load file types from JSON file.");

         // Flatten all categories of extensions into one dictionary
         foreach (var category in all_types) {
            foreach (var ext_and_mime in category.Value) {
               var dot_extension = "." + ext_and_mime.Key.ToLower();
               if (!extension_mime_map.ContainsKey(dot_extension)) {
                  extension_mime_map[dot_extension] = ext_and_mime.Value;
               }
            }
         }
      }
      catch (Exception e) {
         MessageBox.Show(
            $"Failed to load file types from {File_Types_Json_FileName}: {e.Message}",
            "Error",
            MessageBoxButtons.OK,
            MessageBoxIcon.Error
         );
      }
   }

   /// <summary>
   /// Converts known file types to a Base64 data URL.
   /// Uses <see cref="extension_mime_map"/> to determine the associated MIME type.
   /// In case of error, returns an empty string.
   /// </summary>
   /// <param name="file_path">The path to the file.</param>
   /// <returns>A Base64 data URL representing the file, or an empty string if there's an error.</returns>
   public static string get_base64_data_url(string file_path) {
      if (string.IsNullOrWhiteSpace(file_path) || !File.Exists(file_path))
         return string.Empty;

      try {
         // Identify the extension so we can look up the correct MIME type
         var extension = Path.GetExtension(file_path).ToLowerInvariant();
         var mime_type = extension_mime_map.GetValueOrDefault(extension, "application/octet-stream");

         // Convert the file into a Base64 string
         var file_bytes    = File.ReadAllBytes(file_path);
         var base64_string = Convert.ToBase64String(file_bytes);
         return $"data:{mime_type};base64,{base64_string}";
      }
      catch {
         return string.Empty;
      }
   }

   public static string get_base64_data_url(FileInfo file) {
      return get_base64_data_url(file.FullName);
   }
}