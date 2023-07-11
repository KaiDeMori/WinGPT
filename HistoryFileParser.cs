using System.Diagnostics.CodeAnalysis;
using System.IO.MemoryMappedFiles;
using System.Text;
using Message = WinGPT.OpenAI.Chat.Message;

namespace WinGPT;

internal static class HistoryFileParser {
   public static bool TryParseConversationHistoryFile(FileInfo historyFile, [NotNullWhen(true)] out Conversation? conversation) {
      conversation = null;

      if (!historyFile.Exists)
         return false;

      //read into file_content with try catch
      string file_content;
      try {
         file_content = File.ReadAllText(historyFile.FullName);
      }
      catch (Exception) {
         return false;
      }

      var memory      = file_content.AsMemory();
      var currentSpan = memory.Span;

      // Process the ConversationHistory section and get the file name
      if (!TryParseHistoryHeader(ref currentSpan, out var info)) {
         return false; // Couldn't parse ConversationHistory
      }

      var messages = new List<Message>();

      // Continue with the rest of the parsing as before
      while (!currentSpan.IsEmpty) {
         int     firstDelimiterIndex = currentSpan.Length;
         string? delimiterFound      = null;

         foreach (var delimiter in SpecialTokens.To_API_Role.Keys) {
            int index = currentSpan.IndexOf(delimiter.AsSpan());
            if (index >= 0 && index < firstDelimiterIndex) {
               firstDelimiterIndex = index;
               delimiterFound      = delimiter;
            }
         }

         // No delimiter found, break out of the loop
         if (delimiterFound == null) {
            break;
         }

         // Could throw an exception if the slice end index is out of range
         var afterDelimiter     = currentSpan[(firstDelimiterIndex + delimiterFound.Length)..];
         var nextDelimiterIndex = afterDelimiter.Length;

         foreach (var delimiter in SpecialTokens.To_API_Role.Keys) {
            int index = afterDelimiter.IndexOf(delimiter.AsSpan());
            if (index >= 0 && index < nextDelimiterIndex) {
               nextDelimiterIndex = index;
            }
         }

         // Could throw an exception if the slice end index is out of range
         var content = afterDelimiter[..nextDelimiterIndex].Trim();

         // Could throw an exception if the Role key doesn't exist in the dictionary
         messages.Add(new Message {
            role    = SpecialTokens.To_API_Role[delimiterFound],
            content = content.ToString()
         });

         // Could throw an exception if the slice start index is out of range
         currentSpan = afterDelimiter[nextDelimiterIndex..];
      }

      conversation = new Conversation(historyFile) {
         Info     = info,
         Messages = messages
      };

      return true;
   }

   public static bool TryParseHistoryHeader( ref ReadOnlySpan<char> currentSpan, out Conversation_Info conversation_info) {
      var conversationHistoryDelimiter = SpecialTokens.ConversationHistory.AsSpan();

      conversation_info = new Conversation_Info();

      // IndexOf will not throw an exception if the delimiter isn't found, it returns -1
      int index = currentSpan.IndexOf(conversationHistoryDelimiter);
      if (index < 0)
         return false; // ConversationHistory token not found

      // Could throw an exception if the slice end index is out of range
      var afterDelimiter = currentSpan[(index + conversationHistoryDelimiter.Length)..];

      // Check the position of the next delimiter in the remaining span
      int nextDelimiterIndex = afterDelimiter.Length;
      foreach (var delimiter in SpecialTokens.To_API_Role.Keys) {
         // IndexOf will not throw an exception if the delimiter isn't found, it returns -1
         int nextIndex = afterDelimiter.IndexOf(delimiter.AsSpan());
         if (nextIndex >= 0 && nextIndex < nextDelimiterIndex) {
            nextDelimiterIndex = nextIndex;
         }
      }

      string[] stringSeparators = {"\r\n"};
      // Could throw an exception if the slice end index is out of range
      var section_lines = afterDelimiter[..nextDelimiterIndex]
         .Trim().ToString()
         .Split(stringSeparators, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

      //parse the section lines
      conversation_info = Conversation_Info.ParseConfig(section_lines);

      // Could throw an exception if the slice start index is out of range
      currentSpan = afterDelimiter[nextDelimiterIndex..];

      return true;
   }
}