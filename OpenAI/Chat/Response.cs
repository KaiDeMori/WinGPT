namespace WinGPT.OpenAI.Chat; 

public class Response {
   public string       Id      { get; init; } = null!;
   public string       Object  { get; init; } = null!;
   public long         Created { get; init; }
   public List<Choice> Choices { get; init; } = null!;
   public Usage        Usage   { get; init; } = null!;
}