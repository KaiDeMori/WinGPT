using Message = WinGPT.OpenAI.Chat.Message;

namespace WinGPT;

public interface InterTulpa {
   public void                Activate(Conversation conversation);
   public Conversation        NewConversation();
   public Task<List<Message>> SendAsync(Message user_message, Conversation conversation);
}