namespace WinGPT.OpenAI.Chat;

public class Choice {
   public int     Index        { get; init; }
   public Message Message      { get; init; } = null!;
   public string  FinishReason { get; init; } = null!;
}