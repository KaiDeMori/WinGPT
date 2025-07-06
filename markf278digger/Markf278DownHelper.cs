using System.Diagnostics;
using System.Text;
using Markdig;
using Newtonsoft.Json;

namespace WinGPT.markf278digger;

public static class Markf278DownHelper {
   /// <summary>
   /// A mapping of file extensions to programming languages for syntax highlighting in Markdown code blocks.
   /// </summary>
   private static readonly Dictionary<string, List<string>> extension_to_language_map;

   private const string extension_to_language_map_file_name = "extension_to_language_map.json";

   private static readonly FileInfo extension_to_language_map_file = new(
      Path.Join(Application_Paths.Config_Directory.FullName, "markf278digger", extension_to_language_map_file_name));

    /// <summary>
    /// Creates a Markdown code block representation of the specified file.    
    /// The code file is formatted as a markdown code block using escaped fences (<c>~~~</c>).
    /// The <b>filename</b> (no folder information) is included in the code block header with special formatting.
    /// If the file has a known extension, the correct <b>lang-xxx</b> hint is appended to the opening fence.
    ///  
    /// An alternative syntax for escaped fences would be to use 4 backticks (<c>````</c>).
    /// </summary>
    /// <param name="file">The file to be converted into a Markdown code block.</param>
    /// <returns>A string containing the Markdown code block representation of the file, 
    /// including its filename, language and content. If the file cannot be read, an empty string is returned.</returns>
    /// <remarks>
    /// For example a file named <c>example.py</c> would be formatted as:
    /// <code>
    /// ### example.py {.external-filename}
    /// ~~~python
    /// code here…
    /// ~~~
    /// </code>
    /// </remarks>
    public static string create_markdown_code_block(FileInfo file) {
      var file_content = String.Empty;
      try {
         file_content = File.ReadAllText(file.FullName);
      }
      catch (Exception) {
         return file_content;
      }

      //get file extension without the leading dot
      string file_extension = Path.GetExtension(file.FullName).TrimStart('.');
      //find the corresponding language string
      string language =
         extension_to_language_map.TryGetValue(
            file_extension,
            out var value
         )
            ? value.FirstOrDefault()
              ?? String.Empty
            : String.Empty;

      var markdown = new StringBuilder();
      markdown.Append(Tools.nl);
      markdown.Append("### ");
      markdown.Append(file.Name);
      markdown.Append("{.external-filename}");
      markdown.Append(Tools.nl);
      markdown.Append("~~~");
      markdown.Append(language);
      markdown.Append(Tools.nl);
      markdown.Append(file_content);
      markdown.Append(Tools.nl);
      markdown.Append("~~~");
      //markdown.Append(Tools.nl);
      //default_suffix(markdown);
      return markdown.ToString();
   }

   //now we want to add code files in a code block, text files with the simple filename wrapper and all other files not at all
   //use the shiny new FileTypeIdentifier
   public static string create_markdown_text_block(FileInfo file) {
      var file_content = String.Empty;
      try {
         file_content = File.ReadAllText(file.FullName);
      }
      catch (Exception) {
         return file_content;
      }

      var markdown = new StringBuilder();
      markdown.Append("### ");
      markdown.Append(file.Name);
      markdown.Append("{.external-filename}");
      markdown.Append(Tools.nl);
      markdown.Append(file_content);
      markdown.Append(Tools.nl);
      default_suffix(markdown);
      return markdown.ToString();
   }

   public static void default_suffix(StringBuilder sb) {
      sb.Append(Tools.nl);
      sb.Append("-----");
      sb.Append(Tools.nl);
   }

   private static readonly MarkdownPipeline pipeline;

   static Markf278DownHelper() {
      if (!extension_to_language_map_file.Exists)
         throw new FileNotFoundException("Extension–to–language map file not found", extension_to_language_map_file.FullName);

      var json = File.ReadAllText(extension_to_language_map_file.FullName);
      var map  = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(json);
      extension_to_language_map = map ?? throw new Exception("Failed to load extension–to–language map from file\r\n" + extension_to_language_map_file_name);

      pipeline = new MarkdownPipelineBuilder()
         .UseAdvancedExtensions()
         .UseEmojiAndSmiley()
         .UseEmphasisExtras()
         .UseSmartyPants()
         .DisableHtml()
         .UseSoftlineBreakAsHardlineBreak()
         //.UsePrism()
         //.Use<AngleBracketEscapeExtension>()
         //.UseMathematics()
         //.UseCodeBlockTextReplace()
         .Build();
      //.UseSyntaxHighlighting()
      //.UseTaskLists()
      //.UseTypographer()
      //.Configure("typographer") 
   }

   public static void Show_markf278down(WinGPT_Form form) {
      if (Conversation.Active == null)
         throw new Exception("No active conversation!");
      var conversation = Conversation.Active;
      var markf278down = conversation.Create_markf278down();
      form.response_textBox.Text = markf278down;

      //double all line endings in the markdown
      //var markf278down_doubled = markf278down.Replace("\r\n", "\r\n\r\n");

      //now we want to use markdig to transform the messages to html
      var html_fragment = Markdown.ToHtml(markf278down, pipeline);
      var htmlFromFile  = Template_Engine.CreateFullHtml_FromFile(html_fragment);

      //DRAGONS be gone!
      if (Debugger.IsAttached)
         File.WriteAllText("PAGE.HTML", htmlFromFile);

      form.webView21.NavigateToString(htmlFromFile);
   }
}