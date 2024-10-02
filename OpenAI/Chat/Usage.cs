namespace WinGPT.OpenAI.Chat;

/// <summary>
/// Represents the usage details of a chat session, including token counts and detailed breakdowns.
/// </summary>
public class Usage {
   public int prompt_tokens     { get; init; }
   public int completion_tokens { get; init; }
   public int total_tokens      { get; init; }

   /// <summary>
   /// Details about the prompt tokens, including cached tokens.
   /// </summary>
   public PromptTokensDetails prompt_tokens_details { get; init; } = new();

   /// <summary>
   /// Details about the completion tokens, including reasoning tokens.
   /// </summary>
   public CompletionTokensDetails completion_tokens_details { get; init; } = new();
}

/// <summary>
/// Represents detailed information about prompt tokens.
/// </summary>
public class PromptTokensDetails {
   public int cached_tokens { get; init; }
}

/// <summary>
/// Represents detailed information about completion tokens.
/// </summary>
public class CompletionTokensDetails {
   public int reasoning_tokens { get; init; }
}