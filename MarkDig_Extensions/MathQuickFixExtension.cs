using Markdig;
using Markdig.Renderers;

namespace WinGPT.MarkDig_Extensions;

public class MathQuickFixExtension : IMarkdownExtension {
   public void Setup(MarkdownPipelineBuilder pipeline) {
      //nothing to do here
   }

   public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer) {
      throw new NotImplementedException();
   }
}