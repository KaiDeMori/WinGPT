namespace WinGPT.OpenAI.Chat;

/// <summary>
/// Currently, the only "tool type" supported is "function". lmao
/// C'mon OpenAI. What are you doing here?
/// </summary>
public class Tool(Function function) {
   /// <summary>
   /// Nobody knows what this could possibly mean.
   /// </summary>
   public string type => "function";
   
   public Function function { get; init; } = function;
}