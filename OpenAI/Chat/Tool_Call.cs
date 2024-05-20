namespace WinGPT.OpenAI.Chat;

public class Tool_Call {
   public required string   id       { get; init; } // The ID of the tool call
   public required string   type     { get; init; } // The type of the tool call
   public required Function function { get; init; } // The function to be called

   // The Function class
   public class Function {
      public required string name      { get; init; } // The name of the function
      public required string arguments { get; init; } // The arguments for the function
   }
}