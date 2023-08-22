//using System.Linq;
//using Markdig;
//using Markdig.Helpers;
//using Markdig.Renderers;
//using Markdig.Syntax;
//using Markdig.Syntax.Inlines;

//namespace WinGPT;

//public class AngleBracketEscapeExtension : IMarkdownExtension {
//   public void Setup(MarkdownPipelineBuilder pipeline) {
//      // Hook into the pipeline
//      pipeline.DocumentProcessed += DocumentProcessed;
//   }

//   public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer) {
//      // Hook into the pipeline
//      //pipeline.DocumentProcessed += DocumentProcessed;
//   }

//   public void RendererSetup(MarkdownPipeline pipeline, IMarkdownRenderer renderer) {
//      // No renderer setup is required
//   }

//   private void DocumentProcessed(MarkdownDocument document) {
//      foreach (var descendant in document.Descendants()) {
//         if (descendant is ParagraphBlock paragraph) {
//            EscapeAngleBrackets(paragraph);
//         }
//         else if (descendant is CodeBlock code) {
//            EscapeAngleBrackets(code);
//         }
//      }
//   }

//   private void EscapeAngleBrackets(LeafBlock block) {
//      if (block.Lines.Lines != null) {
//         var newLines = new StringLine[block.Lines.Count];

//         for (var i = 0; i < block.Lines.Count; i++) {
//            var line  = block.Lines.Lines[i];
//            var slice = line.Slice;

//            var text = slice.Text.Substring(slice.Start, slice.Length);
//            text = text.Replace("<", "&lt;").Replace(">", "&gt;");

//            newLines[i] = new StringLine(text, slice.Start, slice.Start + text.Length - 1);
//         }

//         block.Lines = new StringLineGroup(newLines);
//      }
//   }
//}