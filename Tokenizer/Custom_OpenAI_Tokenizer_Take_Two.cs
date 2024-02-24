using System.Collections.Immutable;
using WinGPT.OpenAI;
using WinGPT.OpenAI.Chat;
using Message = WinGPT.OpenAI.Chat.Message;

namespace WinGPT.Tokenizer;

public class Special_Token_Counts {
   public int msg_prefix  { get; set; }
   public int msg_postfix { get; set; }
   public int msg_name    { get; set; }
   public int req_postfix { get; set; }

   public int func_init { get; set; }
   public int prop_init { get; set; }
   public int prop_key  { get; set; }
   public int enum_init { get; set; }
   public int enum_item { get; set; }
   public int func_end  { get; set; }
}

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
   private static readonly Special_Token_Counts gpt_4 = new() {
      msg_prefix  = 3, // <|im_start|>user<|im_sep|>
      msg_postfix = 1, // <|im_end|>
      msg_name    = 1,
      req_postfix = 3, // <|im_start|>assistant<|im_sep|>
      func_init   = 7,
      prop_init   = 3,
      prop_key    = 3,
      enum_init   = -3,
      enum_item   = 3,
      func_end    = 12,
   };

   private static readonly Special_Token_Counts gpt_3_5_turbo = new() {
      msg_prefix  = 2, // <|im_start|>user
      msg_postfix = 1, // <|im_end|>
      msg_name    = 1,
      req_postfix = 2, // <|im_start|>assistant
      func_init   = 7,
      prop_init   = 3,
      prop_key    = 3,
      enum_init   = -3,
      enum_item   = 3,
      func_end    = 12,
   };

   private static readonly string[] gpt_4_models = {
      Models.gpt_4,
      Models.gpt_4_0125_preview,
      Models.gpt_4_0613,
      Models.gpt_4_1106_preview,
      Models.gpt_4_turbo_preview,
      Models.gpt_4_vision_preview,
   };

   private static readonly string[] gpt_3_5_turbo_models = {
      Models.gpt_3_5_turbo,
      Models.gpt_3_5_turbo_0301,
      Models.gpt_3_5_turbo_0613,
      Models.gpt_3_5_turbo_1106,
      Models.gpt_3_5_turbo_16k,
      Models.gpt_3_5_turbo_16k_0613,
      Models.gpt_3_5_turbo_instruct,
      Models.gpt_3_5_turbo_instruct_0914,
   };

   // This method calculates the token count for messages and IFunction array based on the model
   public static int count_tokens(ImmutableList<Message> messages, List<Function>? functions) {
      Special_Token_Counts? special_token_counts;

      if (gpt_4_models.Contains(Config.Active.LanguageModel))
         special_token_counts = gpt_4;
      else if (gpt_3_5_turbo_models.Contains(Config.Active.LanguageModel))
         special_token_counts = gpt_3_5_turbo;
      else
         throw new Exception("Model not supported");

      // Use the provided count function for token counting
      Func<string, int> count = DeepTokenizer.count_tokens(Config.Active.LanguageModel);

      int msg_token_count = 0;
      foreach (var message in messages) {
         msg_token_count += special_token_counts.msg_prefix;
         msg_token_count += count(message.content);
         if (message.name != null)
            msg_token_count += special_token_counts.msg_name; // Add tokens if name is set

         msg_token_count += special_token_counts.msg_postfix;
      }

      msg_token_count += special_token_counts.req_postfix;

      var func_token_count = 0;
      if (functions is not {Count: > 0}) {
         return msg_token_count;
      }

      foreach (var function in functions) {
         //if (iFunction is not Function<Parameters> function)
         //   continue;

         func_token_count += special_token_counts.func_init; // Add tokens for start of each function
         string f_name = function.name;
         string f_desc = function.description.TrimEnd('.');
         string line   = $"{f_name}:{f_desc}";
         func_token_count += count(line); // Add tokens for set name and description

         var properties = function.parameters.properties;
         if (properties.Count <= 0)
            continue;

         func_token_count += special_token_counts.prop_init; // Add tokens for start of each property
         foreach (var (p_name, p_value) in properties) {
            func_token_count += special_token_counts.prop_key; // Add tokens for each set property
            string p_type = p_value.type;
            string p_desc = p_value.description.TrimEnd('.');
            if (p_value.enumValues != null) {
               func_token_count += special_token_counts.enum_init; // Add tokens if property has enum list
               foreach (var item in p_value.enumValues) {
                  func_token_count += special_token_counts.enum_item;
                  func_token_count += count(item);
               }
            }

            line = $"{p_name}:{p_type}:{p_desc}";

            func_token_count += count(line);
         }
      }

      func_token_count += special_token_counts.func_end;

      return msg_token_count + func_token_count;
   }
}