namespace WinGPT.Tokenizer;

public class CountTokenizer
{
   public static int count(string content, string language_model) {
      //For now, just return the length of the content.
      return content.Length;
   }
}