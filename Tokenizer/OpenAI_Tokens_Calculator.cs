using Message = WinGPT.OpenAI.Chat.Message;

namespace WinGPT.Tokenizer;

/// <summary>
/// This method replaces the Python function num_tokens_from_messages,
/// found in the <a href="https://github.com/openai/openai-cookbook/blob/main/examples/How_to_count_tokens_with_tiktoken.ipynb">OpenAI cookbook</a>.
/// And it's already outdated, since there is no *preview* or *vision* model :-(
/// It calculates the number of tokens from a list of Message objects based on the model used.
/// Useless.
/// </summary>
public static class OpenAI_Tokens_Calculator {
   public static int count_tokens(List<Message> messages, string model) {
      int tokensPerMessage;
      int tokensPerName;
      int numTokens = 0;

      // Model-specific token adjustments
      if (new HashSet<string> {
             "gpt-3.5-turbo-0613",
             "gpt-3.5-turbo-16k-0613",
             "gpt-4-0314",
             "gpt-4-32k-0314",
             "gpt-4-0613",
             "gpt-4-32k-0613",
          }.Contains(model)) {
         tokensPerMessage = 3;
         tokensPerName    = 1;
      }
      else if (model == "gpt-3.5-turbo-0301") {
         tokensPerMessage = 4;  // every message follows "start"{role/name}\n{content}"end"\n
         tokensPerName    = -1; // if there's a name, the role is omitted
      }
      else if (model.Contains("gpt-3.5-turbo")) {
         Console.WriteLine("Warning: gpt-3.5-turbo may update over time. Returning num tokens assuming gpt-3.5-turbo-0613.");
         return count_tokens(messages, "gpt-3.5-turbo-0613");
      }
      else if (model.Contains("gpt-4")) {
         Console.WriteLine("Warning: gpt-4 may update over time. Returning num tokens assuming gpt-4-0613.");
         return count_tokens(messages, "gpt-4-0613");
      }
      else {
         throw new NotImplementedException($"num_tokens_from_messages() is not implemented for model {model}.");
      }

      // Calculate the number of tokens for each message
      foreach (var message in messages) {
         numTokens += tokensPerMessage;
         numTokens += DeepTokenizer.count_tokens(message.content, model); // Count tokens for the content
         if (message.name != null) {
            numTokens += DeepTokenizer.count_tokens(message.name, model) + tokensPerName;
         }
      }

      numTokens += 3; // every reply is primed with "start"assistant"message"
      return numTokens;
   }
}