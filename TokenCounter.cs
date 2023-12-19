using Newtonsoft.Json;
using WinGPT.OpenAI.Chat;

namespace WinGPT;

//DRAGONS I forsee trouble when multiple threads are involved.
public class TokenCounter {
   private int _totalTokens;

   //a callback for limit reached
   [JsonIgnore]
   public Action? LimitReached;

   public int Prompt_Tokens     { get; set; }
   public int Completion_Tokens { get; set; }
   public int Token_Limit       { get; set; } = 1000;

   public int Total_Tokens {
      get => _totalTokens;
      set {
         if (_totalTokens == value)
            return;

         _totalTokens = value;

         if (Config.loading)
            return;
         Config.Save();
         if (value >= Token_Limit && Show_Message) {
            LimitReached?.Invoke();
         }

         SanityCheck();
      }
   }

   public bool Show_Message { get; set; } = false;

   private void SanityCheck() {
      if (Prompt_Tokens + Completion_Tokens != Total_Tokens) {
         //throw new Exception("TokenCounter: Sanity check failed.");
         MessageBox.Show("TokenCounter: Sanity check failed. Someone forgot how to add!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
      }
   }

   public void Reset() {
      Prompt_Tokens     = 0;
      Completion_Tokens = 0;
      Total_Tokens      = 0;
   }

   public void Increment(Response response) {
      Prompt_Tokens     += response.Usage.prompt_tokens;
      Completion_Tokens += response.Usage.completion_tokens;
      Total_Tokens      += response.Usage.total_tokens;
   }
}