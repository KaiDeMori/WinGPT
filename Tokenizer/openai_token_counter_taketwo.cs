using WinGPT.OpenAI.Chat;
using Message = WinGPT.OpenAI.Chat.Message;

namespace WinGPT.Tokenizer;

public static class openai_token_counter_taketwo {
   public static int estimate_token_count(Request request) {
      Func<string, int> count_tokens =
         content => CountTokenizer.count(content, Config.Active.Language_Model);

      var messages     = request.messages;
      var functions    = request.functions ?? new List<Function>();
      var functionCall = request.function_call;

      bool padded_system = false;
      int  tokens        = 0;

      // Count tokens in messages
      foreach (var message in messages) {
         var message_copy = message.Clone();
         if (message_copy.role == Role.system && functions.Any() && !padded_system) {
            //append \n to all text content parts
            switch (message) {
               case Complex_Message complex_message: {
                  foreach (var content in complex_message.content)
                     if (content is text_content_part text_content)
                        text_content.text += "\n";
                  break;
               }
               case Simple_Message simple_message:
                  simple_message.content += "\n";
                  break;
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

      // Count tokens for the each text content part
      switch (message) {
         case Complex_Message complex_message:
            tokens += complex_message.content.OfType<text_content_part>()
               .Sum(text_content => count_tokens(text_content.text));
            break;
         case Simple_Message simple_message:
            tokens += count_tokens(simple_message.content);
            break;
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