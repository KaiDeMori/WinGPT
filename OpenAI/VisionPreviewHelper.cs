using WinGPT.OpenAI.Chat;

namespace WinGPT.OpenAI;

public static class VisionPreviewHelper {
   /// <summary>
   /// Adds a vision preview user message to the list of all messages.
   /// </summary>
   /// <param name="userPrompt">The prompt string.</param>
   /// <param name="associatedFiles">An array of associated image files that will be included in the new message.</param>
   public static VisionMessage add_vision_preview_user_message(string userPrompt, FileInfo[]? associatedFiles) {
      // Create a list to hold the content elements
      var contentList = new List<VisionMessageContent>();

      // Add the original text content
      if (!string.IsNullOrEmpty(userPrompt)) {
         contentList.Add(new TextContent {Text = userPrompt});
      }

      // Add all image files as Base64 data URLs to the content
      if (associatedFiles != null) {
         foreach (var file in associatedFiles.Where(file => FileTypeIdentifier.GetFileType(file.FullName) == FileType.Image)) {
            string base64DataUrl = ImageHelper.GetBase64DataUrl(file.FullName);
            // Create an ImageContent object for the image
            var imageContent = new ImageContent {
               ImageUrl = new ImageUrl {Url = base64DataUrl}
            };
            // Add the image content to the content list
            contentList.Add(imageContent);
         }
      }

      // Create a new VisionMessage with the content list
      var newVisionMessage = new VisionMessage(Role.user, contentList);

      return newVisionMessage;
   }
}