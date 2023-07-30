using Message = WinGPT.OpenAI.Chat.Message;

namespace WinGPT;

public interface InterTulpa {
   public Task<Message?> SendAsync(
      Message      user_message,
      Conversation conversation,
      FileInfo[]?  associated_files = null);


   //public void Activate(Conversation conversation);
   //public Conversation NewConversation();
}