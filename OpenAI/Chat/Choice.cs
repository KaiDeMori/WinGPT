namespace WinGPT.OpenAI.Chat;

public class Choice {
   public int           index         { get; init; }
   public Message       message       { get; init; } = null!;
   public Finish_Reason finish_reason { get; init; }
}

/// <summary>
/// <see href="https://platform.openai.com/docs/guides/gpt/chat-completions-api"/>
/// And the documentation is still wrong! *lol*
/// Literally straight from the dox:
/// Depending on input parameters (like providing functions as shown below),
/// the model response may include different information.
/// 🤣
/// </summary>
public enum Finish_Reason {
   stop,
   length,
   function_call, //NOT if we insist on calling THAT one function.
   content_filter,
   @null,
}