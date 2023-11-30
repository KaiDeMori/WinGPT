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
}