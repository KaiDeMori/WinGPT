using Markdig;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.Syntax;

public class CodeBlockTextReplaceExtension : IMarkdownExtension {
   public void Setup(MarkdownPipelineBuilder pipeline) {
      // Nothing to do here.
   }

   public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer) {
      if (renderer is HtmlRenderer htmlRenderer) {
         // Replace the default code block renderer
         var originalCodeBlockRenderer = htmlRenderer.ObjectRenderers.FindExact<CodeBlockRenderer>();
         if (originalCodeBlockRenderer != null) {
            htmlRenderer.ObjectRenderers.Remove(originalCodeBlockRenderer);
         }

         htmlRenderer.ObjectRenderers.Add(new CustomCodeBlockRenderer());
      }
   }
}

public class CustomCodeBlockRenderer : HtmlObjectRenderer<CodeBlock> {
   protected override void Write(HtmlRenderer renderer, CodeBlock obj) {
      // Assuming the text to be replaced is "TODO" with "REPLACED"
      var replacedContent = obj.Lines.ToString().Replace("<", "&lt;");
      renderer.Write("<pre><code>").Write(replacedContent).Write("</code></pre>");
   }
}

public static class MarkdownBuilderExtensions {
   public static MarkdownPipelineBuilder UseCodeBlockTextReplace(this MarkdownPipelineBuilder pipeline) {
      pipeline.Extensions.Add(new CodeBlockTextReplaceExtension());
      return pipeline;
   }
}