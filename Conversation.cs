using System.Collections.Immutable;
using System.Text;
using WinGPT.OpenAI;
using WinGPT.OpenAI.Chat;

namespace WinGPT;

using Message = OpenAI.Chat.Message;

public class Conversation {
   public Conversation_Info Info { get; set; } = new();

   /// <summary>
   /// The messages in the conversation.
   /// Does not contain system messages or other tulpa messages.
   /// </summary>
   public List<Message> Messages { get; set; } = new();

   private FileInfo HistoryFile;

   /// <summary>
   /// This will try to rename the conversation and move the file accordingly.
   /// If no <see cref="FileInfo"/> is provided, we will use the name to create the new conversation file.
   /// </summary>
   /// <param name="new_conversation_name">The new name of the conversation.
   /// Will be used to create the new file name if no <see cref="FileInfo"/> is provided.</param>
   /// <param name="new_file">If present, the new file to move the conversation to.</param>
   /// <returns>True if the rename and move was successful, false otherwise.
   /// If False, the conversation and file will not be renamed.</returns>
   public bool TryRenameConversationAndFile(string new_conversation_name, FileInfo? new_file = null) {
      if (new_file is null) {
         var new_filename = Tools.To_valid_filename(new_conversation_name) + Config.marf278down_extenstion;
         new_file = new FileInfo(Path.Join(HistoryFile.DirectoryName, new_filename));
      }

      if (new_file.Exists) {
         Console.WriteLine("File already exists.");
         return false;
      }

      //HistoryFile.MoveTo(new_file.FullName);
      //be careful to catch any exceptions that would lead to the new file not being created
      try {
         HistoryFile.MoveTo(new_file.FullName);
         Info.Name   = new_conversation_name;
         HistoryFile = new_file;
      }
      catch (Exception e) {
         Console.WriteLine(e);
         return false;
      }

      return true;
   }

   public bool useSysMsgHack;

   public Conversation(FileInfo historyFile) {
      HistoryFile = historyFile;
   }

   public string Create_history_file_content() {
      StringBuilder sb = new();
      sb.Append(SpecialTokens.ConversationHistory);
      StringBuilder info = Info.Create_history_file_content();
      sb.Append(info);
      foreach (var message in Messages)
         sb.AppendLine(message.ToString());

      return sb.ToString();
   }

   public string Create_marf278down() {
      //we also need to replace the default newline /n with the system newline /r/n
      string messages = string.Join("", Messages.Select(m => m.ToString().Replace("\n", Environment.NewLine)));
      return messages;
   }

   public async Task CreateTitleAsync() {
      List<Message> messages = new List<Message>() {
         new Message() {
            role    = Role.system,
            content = Config.conversation_title_prompt
         },
      };
      messages.AddRange(Messages);

      Request request = new Request() {
         messages    = messages.ToImmutableList(),
         model       = Models.gpt_3_5_turbo_16k,
         temperature = 0f
      };

      Response? response = await Completion.POST_Async(request);

      if (response is {Choices.Count: > 0}) {
         var newName = response.Choices[0].Message.content.Trim();
         TryRenameConversationAndFile(newName);
      }
   }


   public void Save() {
      var content = Create_history_file_content();
      File.WriteAllText(HistoryFile.FullName, content);
   }
}