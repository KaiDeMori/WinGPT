using MimeTypes;

namespace WinGPT;

/// <summary>
/// We used to distinguish between images and documents, but times change!
/// Now we unify everything in one place. 🤗
/// </summary>
public static class FileHelper {
   /// <summary>
   /// Converts known file types to a Base64 data URL.
   /// Uses <see cref="MimeTypeMap"/> to determine the associated MIME type.
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
         var mime_type = MimeTypeMap.GetMimeType(extension);

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