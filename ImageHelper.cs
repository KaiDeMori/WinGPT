using System.Drawing.Imaging;

namespace WinGPT;

public static class ImageHelper {
   /// <summary>
   /// Reads an image from a file and converts it to a Base64 data URL.
   /// </summary>
   /// <param name="imagePath">The path to the image file.</param>
   /// <returns>A Base64 data URL representing the image.</returns>
   public static string GetBase64DataUrl_for_Image(string imagePath) {
      using Image image = Image.FromFile(imagePath);
      return GetBase64DataUrl_for_Image(image);
   }

   /// <summary>
   /// Converts an Image to a Base64 data URL.
   /// </summary>
   /// <param name="image">The Image to convert.</param>
   /// <returns>A Base64 data URL representing the image.</returns>
   public static string GetBase64DataUrl_for_Image(Image image) {
      using MemoryStream m = new MemoryStream();
      // Save the image to the stream in PNG format
      image.Save(m, ImageFormat.Png);

      // Convert the image to byte array
      byte[] imageBytes = m.ToArray();

      // Convert byte array to Base64 string
      string base64String = Convert.ToBase64String(imageBytes);

      // Construct the data URL
      return $"data:image/png;base64,{base64String}";
   }

   /// <summary>
   /// Converts a PDF to a Base64 data URL.
   /// </summary>
   /// <param name="docPath">The path to the PDF document file.</param>
   /// <returns>A Base64 data URL representing the PDF document.</returns>
   public static string GetBase64DataUrl_for_Document(string docPath) {
      byte[] docBytes = File.ReadAllBytes(docPath);
      string base64String = Convert.ToBase64String(docBytes);
      return $"data:application/pdf;base64,{base64String}";
   }
}
