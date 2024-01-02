using Newtonsoft.Json.Linq;
using WinGPT.OpenAI.Chat;
using Message = WinGPT.OpenAI.Chat.Message;

namespace WinGPT.Tokenizer;

public static class openai_token_counter {
   public static int estimate_token_count(Request request, string language_model) {
      Func<string, int> count_tokens = DeepTokenizer.count_tokens(language_model);

      var messages     = request.messages;
      var functions    = request.functions ?? Array.Empty<Function>();
      var functionCall = request.function_call;

      bool paddedSystem = false;

      int tokens = 0;
      foreach (var message in messages) {
         var messageCopy = message.Clone();
         if (messageCopy.role == Role.system && functions.Any() && !paddedSystem) {
            if (!string.IsNullOrEmpty(messageCopy.content))
               messageCopy = new Message(messageCopy.role, messageCopy.content + "\n");

            paddedSystem = true;
         }

         tokens += estimate_tokens_in_messages(messageCopy, count_tokens);
      }

      // Each completion (vs message) seems to carry a 3-token overhead
      tokens += 3;

      // If there are functions, add the function definitions as they count towards token usage
      if (functions.Any())
         tokens += estimate_tokens_in_functions(functions, count_tokens);

      // If there's a system message and functions are present, subtract four tokens
      if (functions.Any() && messages.Any(m => m.role == Role.system))
         tokens -= 4;

      // Corrected handling of functionCall
      if (functionCall == null || functionCall.mode == Function_Call_Mode.auto)
         return tokens;

      //if (functionCall.mode == Function_Call_Mode.none)
      //   tokens += 1;
      //else {
      //   var functionName = functionCall.name;
      //   if (!string.IsNullOrEmpty(functionName?.name)) {
      //      tokens += count_tokens(functionName.name) + 4;
      //   }
      //}

      tokens += functionCall.mode == Function_Call_Mode.none
         ? 1
         : count_tokens(functionCall.name?.name ?? string.Empty) + 4;

      return tokens;
   }

   private static int estimate_tokens_in_messages(Message message, Func<string, int> count_tokens) {
      int tokens = 0;

      tokens += count_tokens(message.role.ToString());

      if (!string.IsNullOrEmpty(message.content))
         tokens += count_tokens(message.content);

      if (!string.IsNullOrEmpty(message.name))
         tokens += count_tokens(message.name) + 1; // +1 for the name

      if (message.function_call != null) {
         if (!string.IsNullOrEmpty(message.function_call.name))
            tokens += count_tokens(message.function_call.name);

         if (!string.IsNullOrEmpty(message.function_call.arguments))
            tokens += count_tokens(message.function_call.arguments);

         tokens += 3; // Additional tokens for function call
      }

      tokens += 3; // Add three per message

      if (message.role == Role.function)
         tokens -= 2; // Subtract 2 if role is "function"

      return tokens;
   }

   private static int estimate_tokens_in_functions(IEnumerable<Function> functions, Func<string, int> count_tokens) {
      // Convert the function definitions to a string using the format logic from Python
      var promptDefinition = function_formatter.format_function_definitions(functions);
      int tokens           = count_tokens(promptDefinition);
      tokens += 9; // Additional tokens for function definition
      return tokens;
   }
}