using System.Diagnostics;
using System.Text;

namespace WinGPT;

using Message = OpenAI.Chat.Message;

public static class ConversationHistory {
   public static DirectoryInfo GetAbsoluteHistoryDirectory() {
      var activeBaseDirectory    = Config.Active.BaseDirectory;
      var activeHistoryDirectory = Path.Join(activeBaseDirectory, Config.conversation_history_directory);
      return new DirectoryInfo(activeHistoryDirectory);
   }

   public static string GetRelativeDirectory(FileInfo historyFile) {
      var activeBaseDirectory = Config.Active.BaseDirectory;
      //use built in function to get relative path
      if (activeBaseDirectory == null)
         Debugger.Break();
      var relativePath = Path.GetRelativePath(activeBaseDirectory, historyFile.DirectoryName);
      return relativePath;
   }
}