using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

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
    <script>{Config.prism_js}</script>
</head>
<body>
    {HTML_fragment}
</body>
</html>";
      return html;
   }
}