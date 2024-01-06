namespace WinGPT.Tokenizer.lasttrythenigiveup;

// Token counter class for estimating the number of tokens used by OpenAI API requests
public class TokenCounter {
   // Optional model name for token counting
   public string? model { get; set; }

   // Constructor
   public TokenCounter(string? model = null) {
      this.model = model;
   }

   // Estimate the number of tokens a prompt will use
   public int estimate_token_count(OpenAIRequest request) {
      var messages      = request.messages;
      var functions     = request.functions;
      var function_call = request.function_call;

      bool padded_system = false;
      int  tokens        = 0;

      foreach (var message in messages) {
         var message_copy = message.DeepCopy();
         if (message_copy.role == "system" && functions != null && !padded_system) {
            if (!string.IsNullOrEmpty(message_copy.content)) {
               message_copy.content += "\n";
            }

            padded_system = true;
         }

         tokens += estimate_tokens_in_messages(message_copy);
      }

      // Each completion seems to carry a 3-token overhead
      tokens += 3;

      if (functions != null) {
         tokens += estimate_tokens_in_functions(functions);
      }

      if (functions != null && messages.Any(m => m.role == "system")) {
         tokens -= 4;
      }

      if (function_call != null && function_call != "auto") {
         if (function_call == "none") {
            tokens += 1;
         }
         else if (function_call is Dictionary<string, object> && ((Dictionary<string, object>) function_call).ContainsKey("name")) {
            tokens += string_tokens((string) ((Dictionary<string, object>) function_call)["name"]) + 4;
         }
      }

      return tokens;
   }

   // Get the token count for a string
   public int string_tokens(string str) {
      var encoding = !string.IsNullOrEmpty(model) ? EncodingForModel(model) : GetEncoding("cl100k_base");
      return encoding.Encode(str).Length;
   }

   // Estimate token count for a single message
   public int estimate_tokens_in_messages(OpenAIMessage message) {
      int tokens = 0;

      if (!string.IsNullOrEmpty(message.role)) {
         tokens += string_tokens(message.role);
      }

      if (!string.IsNullOrEmpty(message.content)) {
         tokens += string_tokens(message.content);
      }

      if (!string.IsNullOrEmpty(message.name)) {
         tokens += string_tokens(message.name) + 1; // +1 for the name
      }

      if (message.function_call != null) {
         if (!string.IsNullOrEmpty(message.function_call.name)) {
            tokens += string_tokens(message.function_call.name);
         }

         if (!string.IsNullOrEmpty(message.function_call.arguments)) {
            tokens += string_tokens(message.function_call.arguments);
         }

         tokens += 3; // Additional tokens for function call
      }

      tokens += 3; // Add three per message

      if (message.role == "function") {
         tokens -= 2; // Subtract 2 if role is "function"
      }

      return tokens;
   }

   // Estimate token count for the functions
   public int estimate_tokens_in_functions(List<OpenAIFunction> functions) {
      var prompt_definition = FormatFunctionDefinitions(functions);
      var tokens            = string_tokens(prompt_definition);
      tokens += 9; // Additional tokens for function definition
      return tokens;
   }

   // Placeholder for encoding methods
   private Encoding EncodingForModel(string model) {
      // Placeholder: Implement the actual encoding logic or integrate with an existing library
      throw new NotImplementedException();
   }

   private Encoding GetEncoding(string encodingName) {
      // Placeholder: Implement the actual encoding logic or integrate with an existing library
      throw new NotImplementedException();
   }
}

// Placeholder classes for OpenAIRequest, OpenAIMessage, and OpenAIFunction
// These should be implemented according to the structure expected by the token counter
public class OpenAIRequest {
   public List<OpenAIMessage>  messages      { get; set; }
   public List<OpenAIFunction> functions     { get; set; }
   public object?              function_call { get; set; }
}

public class OpenAIMessage {
   public string?             role          { get; set; }
   public string?             content       { get; set; }
   public string?             name          { get; set; }
   public OpenAIFunctionCall? function_call { get; set; }

   // Method to create a deep copy of OpenAIMessage
   public OpenAIMessage DeepCopy() {
      // Placeholder: Implement the actual deep copy logic
      throw new NotImplementedException();
   }
}

public class OpenAIFunctionCall {
   public string? name      { get; set; }
   public string? arguments { get; set; }
}

// Placeholder for Encoding class
// This should be implemented or replaced with an actual encoding class
//public class Encoding {
//   public int[] Encode(string str) {
//      // Placeholder: Implement the actual encoding logic
//      throw new NotImplementedException();
//   }
//}
