using System.Collections.Immutable;
using WinGPT.OpenAI;
using WinGPT.OpenAI.Chat;
using Message = WinGPT.OpenAI.Chat.Message;

namespace WinGPT.Tokenizer;

/// <summary>
/// Before we test anything:
/// This is based on
/// <a href="https://stackoverflow.com/a/77175648/4821032">this StackOverflow answer</a>.
/// Then translated to C# by GPT-4-Turbo.
/// Using the API via WinGPT, obviously :-)
/// It's a real shame that OpenAI does such a bad job at making their API accessible.
/// I don't expect them to implement everything, but a clean and precise documentation would be nice.
/// And I don't mean the API docs, I mean the docs for the models (although the API docs are also a real mess).
/// </summary>
internal class Custom_OpenAI_Tokenizer_Take_Two {
   // This method calculates the token count for messages and IFunction array based on the model
   public static int count_tokens(ImmutableList<Message> messages, IFunction[]? functions) {
      // Initialize message settings to 0
      int msg_init = 0;
      int msg_name = 0;
      int msg_end  = 0;

      // Initialize function settings to 0
      int func_init = 0;
      int prop_init = 0;
      int prop_key  = 0;
      int enum_init = 0;
      int enum_item = 0;
      int func_end  = 0;

      // Define the models that have specific token settings
      string[] specificModels = {
         Models.gpt_3_5_turbo_0613,
         Models.gpt_4_0613
      };

      // Check if the active model is one of the specific models
      if (specificModels.Contains(Config.Active.LanguageModel)) {
         // Set message settings for above models
         msg_init = 3;
         msg_name = 1;
         msg_end  = 3;

         // Set function settings for the above models
         func_init = 7;
         prop_init = 3;
         prop_key  = 3;
         enum_init = -3;
         enum_item = 3;
         func_end  = 12;
      }

      // Use the provided count function for token counting
      Func<string, int> count = DeepTokenizer.count_tokens(Config.Active.LanguageModel);

      int msg_token_count = 0;
      foreach (var message in messages) {
         msg_token_count += msg_init;               // Add tokens for each message
         msg_token_count += count(message.content); // Add tokens in set message
         if (message.name != null) {
            msg_token_count += msg_name; // Add tokens if name is set
         }
      }

      msg_token_count += msg_end; // Add tokens to account for ending

      var func_token_count = 0;
      if (functions is not {Length: > 0}) {
         return msg_token_count + func_token_count;
      }

      foreach (var iFunction in functions) {
         if (iFunction is Function<Parameters> function) {
            func_token_count += func_init; // Add tokens for start of each function
            string f_name = function.name;
            string f_desc = function.description.TrimEnd('.');
            string line   = $"{f_name}:{f_desc}";
            func_token_count += count(line); // Add tokens for set name and description

            var properties = function.parameters.properties;
            if (properties.Count > 0) {
               func_token_count += prop_init; // Add tokens for start of each property
               foreach (var property in properties) {
                  func_token_count += prop_key; // Add tokens for each set property
                  string p_name = property.Key;
                  string p_type = property.Value.type;
                  string p_desc = property.Value.description.TrimEnd('.');
                  if (property.Value.enumValues != null) {
                     func_token_count += enum_init; // Add tokens if property has enum list
                     foreach (var item in property.Value.enumValues) {
                        func_token_count += enum_item;
                        func_token_count += count(item);
                     }
                  }

                  line             =  $"{p_name}:{p_type}:{p_desc}";
                  func_token_count += count(line);
               }
            }
         }
      }

      func_token_count += func_end;

      return msg_token_count + func_token_count;
   }
}