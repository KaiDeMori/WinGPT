using WinGPT.OpenAI.Chat;

namespace WinGPT.ClaudeAI;

public class Response {
   public string Id { get; init; } = null!;

   public long         Created { get; init; }
   public List<Choice> Choices { get; init; } = null!;
   public Usage        Usage   { get; init; } = null!;
}