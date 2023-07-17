namespace WinGPT.OpenAI.Chat;

public class Response {
   public string Id { get; init; } = null!;

   /// <summary>
   /// Totally unclear what this is.
   /// The documentation only mentions this one value and it's not even
   /// the endpoint's name (the endpoint has a plural "s" at the end).
   /// </summary>
   public string Object { get; init; } = "chat.completion";

   public long         Created { get; init; }
   public List<Choice> Choices { get; init; } = null!;
   public Usage        Usage   { get; init; } = null!;
}