using System.Text;

namespace WinGPT;

public static class Markf278DownHelper {
   public static string create_markdown_code_block(FileInfo file) {
      var file_content = String.Empty;
      try {
         file_content = File.ReadAllText(file.FullName);
      }
      catch (Exception) {
         return file_content;
      }

      var markdown = new StringBuilder();
      markdown.Append("### ");
      markdown.Append(file.Name);
      markdown.Append("{.external-filename}");
      markdown.Append(Tools.nl);
      markdown.Append("```");
      markdown.Append(Tools.nl);
      markdown.Append(file_content);
      markdown.Append(Tools.nl);
      markdown.Append("```");
      markdown.Append(Tools.nl);
      default_suffix(markdown);
      return markdown.ToString();
   }

   //now we want to add code files in a code block, text files with the simple filename wrapper and all other files not at all
   //use the shiny new FileTypeIdentifier
   public static string create_markdown_text_block(FileInfo file) {
      var file_content = String.Empty;
      try {
         file_content = File.ReadAllText(file.FullName);
      }
      catch (Exception) {
         return file_content;
      }

      var markdown = new StringBuilder();
      markdown.Append("### ");
      markdown.Append(file.Name);
      markdown.Append("{.external-filename}");
      markdown.Append(Tools.nl);
      markdown.Append(file_content);
      markdown.Append(Tools.nl);
      default_suffix(markdown);
      return markdown.ToString();
   }

   public static void default_suffix(StringBuilder sb) {
      sb.Append(Tools.nl);
      sb.Append("-----");
      sb.Append(Tools.nl);
   }

   public static void create_markdown_for_file(StringBuilder sb, FileInfo file) {
      var fileType = FileTypeIdentifier.GetFileType(file.FullName);
      switch (fileType) {
         case FileType.Code:
            var markdown_codeblock = create_markdown_code_block(file);
            sb.AppendLine(markdown_codeblock);
            break;
         case FileType.Text:
            var markdown_textblock = create_markdown_text_block(file);
            sb.AppendLine(markdown_textblock);
            break;
         case FileType.Image:
            //not available. We need to use the Vision API for that!
            break;
         case FileType.Other:
            //not available. Maybe we can add a link to the file?
            //or do some Base64 encoding?
            break;
         default:
            throw new ArgumentOutOfRangeException();
      }
   }
}