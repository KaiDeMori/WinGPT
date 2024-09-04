using Markdig;
using Markdig.Renderers;
using Markdig.Renderers.Html;

namespace WinGPT.MarkDig_Extensions;

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