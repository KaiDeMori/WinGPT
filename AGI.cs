namespace WinGPT; 

public static class AGI
{
   public static void taxonomize()
   {

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