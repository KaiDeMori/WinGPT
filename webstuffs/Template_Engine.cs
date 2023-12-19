using System.Text;
using System.Text.RegularExpressions;
using WinGPT;

public static class Template_Engine {
   private static Regex  cssRegex     = null!;
   private static Regex  jsRegex      = null!;
   private static string htmlTemplate = null!;
   private static string webroot      = "webstuffs/";

   private static HTML_CONSTANTS constants = new HTML_CONSTANTS();

   public static void Init() {
      htmlTemplate = File.ReadAllText(webroot + "index_template.html");
      cssRegex     = new Regex("<link rel=\"stylesheet\" href=\"(.*?)\">", RegexOptions.Compiled);
      jsRegex      = new Regex("<script src=\"(.*?)\"></script>",          RegexOptions.Compiled);

      // Replace CSS
      htmlTemplate = cssRegex.Replace(htmlTemplate, match => {
         var filePath = match.Groups[1].Value;

         // Ensure we only replace local files
         if (Uri.IsWellFormedUriString(filePath, UriKind.Relative)) {
            var cssContent = File.ReadAllText(webroot + filePath);
            return $"<style>\n{cssContent}\n</style>";
         }

         return match.Value;
      });

      // Replace JS
      htmlTemplate = jsRegex.Replace(htmlTemplate, match => {
         var filePath = match.Groups[1].Value;

         // Ensure we only replace local files
         if (Uri.IsWellFormedUriString(filePath, UriKind.Relative)) {
            var jsContent = File.ReadAllText(webroot + filePath);
            return $"<script>\n{jsContent}\n</script>";
         }

         return match.Value;
      });
   }

   public static string CreateFullHtml_FromFile(string HTML_fragment) {
      var s = htmlTemplate.Replace("%%HTML_fragment%%", HTML_fragment);
      s = s.Replace("//%%CONSTANTS%%", constants.To_JS_Constants());
      return s;
   }
}

class HTML_CONSTANTS {
   public string auto_scroll => Config.Active.UIable.Auto_Scroll.ToString().ToLower();

   public string To_JS_Constants() {
      StringBuilder sb = new();
      //create JS constants from the properties of this class
      foreach (var prop in this.GetType().GetProperties()) {
         sb.AppendLine($"const {prop.Name} = {prop.GetValue(this)};");
      }

      return sb.ToString();
   }

}