namespace WinGPT;

/// <summary>
/// Not entirely sure why we still distinguish between images and documents.
/// </summary>
public static class ImageHelper {
   /// <summary>
   /// A dictionary that maps file extensions to their associated MIME type.
   /// </summary>
   private static Dictionary<string, string> file_extension_mime_map { get; } = new() {
      [".png"]  = "image/png",
      [".jpg"]  = "image/jpeg",
      [".jpeg"] = "image/jpeg",
      [".webp"] = "image/webp",
      [".gif"]  = "image/gif",
      [".pdf"]  = "application/pdf"
   };

   /// <summary>
   /// Reads an image from a file and converts it to a Base64 data URL.
   /// Uses <see cref="file_extension_mime_map"/> to determine the associated MIME type.
   /// In case of error, returns an empty string.
   /// </summary>
   /// <param name="image_path">The path to the image file.</param>
   /// <returns>A Base64 data URL representing the image.</returns>
   public static string GetBase64DataUrl_for_Image(string image_path) {
      return get_base64_data_url_for_file(image_path);
   }

   /// <summary>
   /// Converts a document to a Base64 data URL.
   /// Uses <see cref="file_extension_mime_map"/> to determine the associated MIME type.
   /// In case of error, returns an empty string.
   /// Currently only PDF is supported.
   /// </summary>
   /// <param name="document_path">The path to the document.</param>
   /// <returns>A Base64 data URL representing the document.</returns>
   public static string GetBase64DataUrl_for_Document(string document_path) {
      return get_base64_data_url_for_file(document_path);
   }

   /// <summary>
   /// Returns the Base64 data URL for a specified file.
   /// Uses <see cref="file_extension_mime_map"/> to determine the associated MIME type.
   /// In case of error, returns an empty string.
   /// </summary>
   /// <param name="file_path">The path to the file.</param>
   /// <returns>A Base64 data URL, or an empty string if there's an error.</returns>
   private static string get_base64_data_url_for_file(string file_path) {
      if (string.IsNullOrWhiteSpace(file_path) || !File.Exists(file_path))
         return string.Empty;

      try {
         string extension = Path.GetExtension(file_path).ToLowerInvariant();
         string mime_type = file_extension_mime_map.GetValueOrDefault(extension, "application/octet-stream");

         byte[] file_bytes    = File.ReadAllBytes(file_path);
         string base64_string = Convert.ToBase64String(file_bytes);
         return $"data:{mime_type};base64,{base64_string}";
      }
      catch {
         return string.Empty;
      }
   }
}