using System.Collections.Immutable;

namespace WinGPT.OpenAI.Chat;

public class Request {
   public string                 model       { get; init; }
   public ImmutableList<Message> messages    { get; init; }
   public double                 temperature { get; init; }
}