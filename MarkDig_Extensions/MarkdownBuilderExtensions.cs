using Markdig;

namespace WinGPT.MarkDig_Extensions;

public static class MarkdownBuilderExtensions {
   public static MarkdownPipelineBuilder UseCodeBlockTextReplace(this MarkdownPipelineBuilder pipeline) {
      pipeline.Extensions.AddIfNotAlready(new CodeBlockTextReplaceExtension());
      return pipeline;
   }

   public static MarkdownPipelineBuilder UseMathQuickFix(this MarkdownPipelineBuilder pipeline) {
      pipeline.Extensions.AddIfNotAlready(new MathQuickFixExtension());
      return pipeline;
   }
}