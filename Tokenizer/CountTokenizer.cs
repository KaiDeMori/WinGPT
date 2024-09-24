using Tiktoken;

namespace WinGPT.Tokenizer;

public class CountTokenizer {
   public static int count(string content, string language_model) {
      //Encoder encoder = ModelToEncoder.For(language_model);
      Encoder? encoder = ModelToEncoder.TryFor(language_model);
      var      count   = encoder?.CountTokens(content) ?? -1;
      return count;
   }
}