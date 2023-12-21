using WinGPT.OpenAI;
using WinGPT.OpenAI.Chat;
using Message = WinGPT.OpenAI.Chat.Message;

namespace WinGPT.Tokenizer;

internal class Custom_OpenAI_Tokenizer {

   // This method calculates the token count for messages and functions based on the model
   public static int count_tokens(List<Message> messages, List<Function<Parameters>> functions) {
      // Initialize message settings to 0
      int msg_init = 0;
      int msg_name = 0;
      int msg_end = 0;

      // Initialize function settings to 0
      int func_init = 0;
      int prop_init = 0;
      int prop_key = 0;
      int enum_init = 0;
      int enum_item = 0;
      int func_end = 0;

      // Check if the model is one of the specified ones and set the token counts accordingly
      if (Models.Supported.Contains(Models.gpt_3_5_turbo_0613) || Models.Supported.Contains(Models.gpt_4_0613)) {
         // Set message settings for above models
         msg_init = 3;
         msg_name = 1;
         msg_end = 3;

         // Set function settings for the above models
         func_init = 7;
         prop_init = 3;
         prop_key = 3;
         enum_init = -3;
         enum_item = 3;
         func_end = 12;
      }

      // Use the provided count function for token counting
      Func<string, int> count = DeepTokenizer.count_tokens(Config.Active.LanguageModel);

      int msg_token_count = 0;
      foreach (var message in messages) {
         msg_token_count += msg_init;  // Add tokens for each message initialization
         msg_token_count += count(message.content);  // Add tokens for message content
         if (message.name != null) {
            msg_token_count += msg_name;  // Add tokens if name is set
         }
      }
      msg_token_count += msg_end;  // Add tokens to account for message ending

      int func_token_count = 0;
      foreach (var function in functions) {
         func_token_count += func_init;  // Add tokens for the start of each function
         string f_name = function.name;
         string f_desc = function.description.TrimEnd('.');
         string line = $"{f_name}:{f_desc}";
         func_token_count += count(line);  // Add tokens for function name and description

         if (function.parameters.required != null) {
            foreach (var req in function.parameters.required) {
               func_token_count += prop_key;  // Add tokens for each required parameter
               func_token_count += count(req);  // Count tokens for the required parameter
            }
         }

         func_token_count += func_end;  // Add tokens for the end of the function
      }

      return msg_token_count + func_token_count;  // Return the total token count
   }
}