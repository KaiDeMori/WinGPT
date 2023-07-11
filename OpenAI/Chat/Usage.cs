namespace WinGPT.OpenAI.Chat;

public class Usage {
   public int prompt_tokens     { get; init; }
   public int completion_tokens { get; init; }
   public int total_tokens      { get; init; }
}