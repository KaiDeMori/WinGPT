using Message = WinGPT.OpenAI.Chat.Message;

namespace WinGPT;

public interface InterTulpa {
   public Task<Message[]> SendAsync(Message user_message, Conversation conversation);


   //public void Activate(Conversation conversation);
   //public Conversation NewConversation();
}