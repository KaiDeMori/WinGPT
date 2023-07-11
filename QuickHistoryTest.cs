namespace WinGPT;

internal class QuickHistoryTest {
   public static void run() {
      var      history_file      = "../../../_RELEASE/Conversation History/History_Example.md";
      FileInfo history_file_info = new(history_file);
      //check if the file exists
      if (!File.Exists(history_file)) {
         Console.WriteLine("File does not exist");
         return;
      }

      //an array
      var history_delimiters = new[] {
         SpecialTokens.ConversationHistory,
         SpecialTokens.System,
         SpecialTokens.User,
         SpecialTokens.Assistant,
      };

      //get content as string
      string content  = File.ReadAllText(history_file);
      //var    somelist = HistoryFileParser.ParseFileContent2(content, history_delimiters);
      //var somelist = HistoryFileParser.ParseFileContent(content);
      //Console.WriteLine(somelist);

      Environment.Exit(0);
   }
}