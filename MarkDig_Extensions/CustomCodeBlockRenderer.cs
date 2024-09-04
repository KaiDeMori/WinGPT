using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.Syntax;

namespace WinGPT.MarkDig_Extensions;

public class CustomCodeBlockRenderer : HtmlObjectRenderer<CodeBlock> {
   protected override void Write(HtmlRenderer renderer, CodeBlock obj) {
      // Assuming the text to be replaced is "TODO" with "REPLACED"
      var replacedContent = obj.Lines.ToString().Replace("\n\n", "\n");
      renderer.Write("<pre><code  class=\"language-csharp\">").Write(replacedContent).Write("</code></pre>");
   }
}