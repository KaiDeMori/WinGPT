using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System.Text.RegularExpressions;

namespace WinGPT;

public static class AGI {
   private static bool IsSchemaOK(string jsonString, string schema_json) {
      JSchema schema     = JSchema.Parse(schema_json);
      JObject jsonObject = JObject.Parse(jsonString);

      bool isValid = jsonObject.IsValid(schema);
      return isValid;
   }

   public static string CreateFullHtml(string HTML_fragment) {
      // Embed the CSS and JS directly in the HTML. 
      string html = $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset=""utf-8"">
    <style>{Config.my_css}</style>
    <style>{Config.prism_css}</style>
    <script>
        {Config.prism_js}</script>
</head>
<body class=""line-numbers match-braces"">
    {HTML_fragment}
</body>
</html>";
      return html;
   }


   private static Dictionary<string, string> fileCache = new();

   private static readonly Regex  StylesheetRegex = new Regex("<link rel=\"stylesheet\" href=\"(?<file>.*?)\">", RegexOptions.Compiled);
   private static readonly Regex  ScriptRegex     = new Regex("<script src=\"(?<file>.*?)\"></script>",          RegexOptions.Compiled);
   private static readonly string webroot         = "webstuffs/";

   private static string html_template => File.ReadAllText(webroot + "index.html");

   public static void Init() {
      foreach (Match match in StylesheetRegex.Matches(html_template).Concat(ScriptRegex.Matches(html_template))) {
         string file = match.Groups["file"].Value;
         fileCache[file] = File.ReadAllText(webroot + file);
      }
   }

   public static string CreateFullHtml_FromFile(string HTML_fragment) {
      var    webroot       = "webstuffs/";
      string html_template = File.ReadAllText(webroot + "index.html");

      Func<string, string> GetFileContent = file => fileCache[file];

      Dictionary<Regex, Func<string, string>> replacements = new() {
         {new Regex("%%HTML_fragment%%", RegexOptions.Compiled), _ => HTML_fragment},
         {StylesheetRegex, file => $"<style>\n{GetFileContent(file)}\n</style>"},
         {ScriptRegex, file => $"<script>\n{GetFileContent(file)}\n</script>"},
      };

      foreach (var pair in replacements) {
         foreach (Match match in pair.Key.Matches(html_template).Cast<Match>()) {
            string file        = match.Groups["file"].Value;
            string replacement = pair.Value(file);
            html_template = html_template.Replace(match.Value, replacement);
         }
      }

      return html_template;
   }
}