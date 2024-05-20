namespace WinGPT.OpenAI.Chat;

public class Response {
   public string id { get; init; } = null!;

   /// <summary>
   /// Totally unclear what this is.
   /// The documentation only mentions this one value and it's not even
   /// the endpoint's name (the endpoint has a plural "s" at the end).
   /// </summary>
   public string @object { get; init; } = "chat.completion";

   public long         created { get; init; }
   public List<Choice> choices { get; init; } = null!;
   public Usage        usage   { get; init; } = null!;
}