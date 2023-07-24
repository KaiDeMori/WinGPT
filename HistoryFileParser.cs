namespace WinGPT;

internal static class HistoryFileParser {
   public static bool TryParseHistoryHeader(ref ReadOnlySpan<char> currentSpan, out Conversation_Info conversation_info) {
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