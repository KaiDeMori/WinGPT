using WinGPT.OpenAI.Chat;
using Message = WinGPT.OpenAI.Chat.Message;

namespace WinGPT.Tokenizer;

public static class openai_token_counter_taketwo {
   public static int estimate_token_count(Request request, string language_model) {
      Func<string, int> count_tokens = DeepTokenizer.count_tokens(language_model);

      var messages     = request.messages;
      var functions    = request.functions ?? new List<Function>();
      var functionCall = request.function_call;

      bool padded_system = false;
      int  tokens        = 0;

      // Count tokens in messages
      foreach (var message in messages) {
         var message_copy = message.Clone();
         if (message_copy.role == Role.system && functions.Any() && !padded_system) {
            if (!string.IsNullOrEmpty(message.content)) {
               // Create a new message with appended newline to content
               message_copy = new Message(message_copy.role, message_copy.content + "\n");
            }

            padded_system = true;
         }

         tokens += estimate_tokens_in_messages(message_copy, count_tokens);
      }

      // Add tokens for the prompt (3 tokens for the initial system message)
      tokens += 3;

      // Count tokens in functions
      if (functions.Any()) {
         tokens += estimate_tokens_in_functions(functions, count_tokens);
      }

      // Adjust token count based on system messages and function calls
      if (functions.Any() && messages.Any(m => m.role == Role.system)) {
         tokens -= 4; // Adjust for the system token
      }

      if (functionCall != null && functionCall.mode != Function_Call_Mode.auto) {
         if (functionCall.mode == Function_Call_Mode.none) {
            tokens += 1; // Add token for "none"
         }
         else {
            if (functionCall.name != null) {
               tokens += count_tokens(functionCall.name.name) + 4; // Add tokens for function call name and formatting
            }
         }
      }

      return tokens;
   }

   private static int estimate_tokens_in_messages(Message message, Func<string, int> count_tokens) {
      int tokens = 0;

      // Count tokens for the role
      tokens += count_tokens(message.role.ToString());

      // Count tokens for the content
      if (!string.IsNullOrEmpty(message.content)) {
         tokens += count_tokens(message.content);
      }

      // Count tokens for the name if it exists
      if (!string.IsNullOrEmpty(message.name)) {
         tokens += count_tokens(message.name) + 1; // Add 1 token for the space before the name
      }

      // Count tokens for the function call if it exists
      if (message.function_call != null) {
         if (!string.IsNullOrEmpty(message.function_call.name)) {
            tokens += count_tokens(message.function_call.name);
         }

         if (!string.IsNullOrEmpty(message.function_call.arguments)) {
            tokens += count_tokens(message.function_call.arguments);
         }

         tokens += 3; // Add tokens for the formatting of the function call
      }

      tokens += 3; // Add tokens for the message formatting

      // Adjust token count for function role
      if (message.role == Role.function) {
         tokens -= 2; // Subtract 2 tokens for the function role
      }

      return tokens;
   }

   private static int estimate_tokens_in_functions(IEnumerable<Function> functions, Func<string, int> count_tokens) {
      int tokens = 0;

      // Format the function definitions and count their tokens
      string formatted_functions = function_formatter_taketwo.format_function_definitions(functions);
      tokens += count_tokens(formatted_functions);

      // Add tokens for the prompt formatting
      tokens += 9;

      return tokens;
   }
}