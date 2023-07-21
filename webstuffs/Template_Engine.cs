using System;
using System.IO;
using System.Text.RegularExpressions;

public static class Template_Engine {
   private static Regex  cssRegex     = null!;
   private static Regex  jsRegex      = null!;
   private static string htmlTemplate = null!;
   private static string webroot      = "webstuffs/";

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
      return htmlTemplate.Replace("%%HTML_fragment%%", HTML_fragment);
   }
}