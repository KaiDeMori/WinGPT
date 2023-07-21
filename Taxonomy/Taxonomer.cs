﻿using System.Collections.Immutable;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using WinGPT.OpenAI;
using WinGPT.OpenAI.Chat;
using Message = WinGPT.OpenAI.Chat.Message;

namespace WinGPT.Taxonomy;

public static class Taxonomer {
   private const           string taxonomy_function_name = "taxonomy";
   private static readonly string FunctionsJson          = File.ReadAllText("Taxonomy/functions.json");

   private const string example_function_call = @"
{
    ""name"": ""taxonomy"",
    ""arguments"": {
        ""summary"": ""A nice little summary."",
        ""filename"": ""My first chat.md"",
		""category"": ""simple times""
    }
}";

   /// <summary>
   /// Sets the summary and the history filename, returns a category-suggestion.
   /// The suggestion should be matched against existing categories.
   /// </summary>
   /// <param name="conversation">The conversation to taxonomize.</param>
   /// <param name="existing_categories">An array of already existing categories, i.e. directory names.</param>
   /// <returns></returns>
   private static Function_Parmeters? taxonomize(Conversation conversation, string[] existing_categories) {
      //DRAGONS AI function-magic
      string existing_categories_json = JsonConvert.SerializeObject(existing_categories);
      var    sysmsg_template          = File.ReadAllText("Taxonomy/system_message_template.md");
      string sysmsg                   = sysmsg_template.Replace($"{{{{{nameof(existing_categories)}}}}}", existing_categories_json);

      Function[]? functions = JsonConvert.DeserializeObject<Function[]>(FunctionsJson);

      List<Message> all_messages = new() {
         new Message(role: Role.system, content: sysmsg)
      };
      all_messages.AddRange(conversation.Messages);
      var all_immutable = all_messages.ToImmutableList();

      //var function_call = new {
      //   name = taxonomy_function_name,
      //};
      ////to json

      //var functionCallSettings = new FunctionCallSettings(taxonomy_function_name);

      //Let's get the request ready
      var request = new Request() {
         messages      = all_immutable,
         functions     = functions,
         function_call = new FunctionCallSettings("taxonomy"),
         model         = Models.gpt_3_5_turbo_16k,
         temperature   = 0.0
      };

      //string request_json = JsonConvert.SerializeObject(request, Formatting.Indented, new JsonSerializerSettings {
      //   NullValueHandling = NullValueHandling.Ignore,
      //});


      //and send it to the AI
      //for now just wait, no async
      //Response? response = Completions.POST_Async(request).ConfigureAwait(false).GetAwaiter().GetResult();
      Response? response = Completions.POST_Async(request).Result;
      if (!IsOK(response))
         return null;

      FunctionCall functionCall = response.Choices[0].message.function_call;

      Function_Parmeters function_parmeters = JsonConvert.DeserializeObject<Function_Parmeters>(functionCall.arguments);

      function_parmeters.existing_categories = existing_categories;

      var is_Existing_Category = existing_categories.Contains(function_parmeters.category);
      if (is_Existing_Category) {
         function_parmeters.selected_category = function_parmeters.category;
      }
      else {
         function_parmeters.new_category = function_parmeters.category;
      }

      return function_parmeters;
   }

   //we have to do a couple of sanity chekcs on the response before using it.
   private static bool IsOK(Response? response) {
      if (response is null)
         return false;
      var choice = response.Choices.FirstOrDefault();
      if (choice is null)
         return false;
      if (choice.finish_reason != Finish_Reason.stop)
         return false;
      //according to the docs:
      var message = choice.message;
      if (message.role != Role.assistant)
         return false;
      if (message.content != null)
         return false;
      if (message.function_call is null)
         return false;

      FunctionCall functionCall = message.function_call;

      //let's parse some stuff and see where it goess
      try {
         var     function_defined = JArray.Parse(FunctionsJson)[0];
         JSchema schema           = JSchema.Parse(function_defined.ToString());
         //serialize the functionCall to json
         string functionCall_json = JsonConvert.SerializeObject(functionCall);

         JObject jsonObject = JObject.Parse(functionCall_json);
         if (!jsonObject.IsValid(schema))
            return false;
      }
      catch (Exception e) {
         //TADA get some proper loggin' goin'
         Debug.WriteLine(e);
      }

      //everything seems to check out so far.
      return true;
   }

   /// <summary>
   /// Error handling?!
   /// </summary>
   /// <param name="conversation"></param>
   public static void taxonomize(Conversation conversation) {
      DirectoryInfo[]     directories         = Config.History_Directory.GetDirectories();
      string[]            existing_categories = directories.Select(d => d.Name).ToArray();
      Function_Parmeters? function_parameters = taxonomize(conversation, existing_categories);
      //DRAGONS where should we do the error handling?
      if (function_parameters == null)
         return;

      // check if function_parameters.filename ends with
      // Config.marf278down_extenstion and if not, add it
      if (!function_parameters.filename.EndsWith(Config.marf278down_extenstion))
         function_parameters.filename += Config.marf278down_extenstion;


      //show in Taxonomy_Form
      var taxonomy_form = new Taxonomy_Form(function_parameters);
      taxonomy_form.ShowDialog();
      string category;
      bool   should_exist;

      //DRAGONS Still a lot of nulls to check
      conversation.Info.Summary = function_parameters.summary;

      if (function_parameters.new_category is not null) {
         category     = function_parameters.new_category;
         should_exist = false;
      }
      else {
         //get the selected category
         category     = function_parameters.selected_category;
         should_exist = true;
      }

      //then, move the file (well, try at least :-) )
      var file_move_result = conversation.TryCategorizeFile(
         function_parameters.filename,
         category,
         should_exist,
         Config.History_Directory
      );

      if (file_move_result != FileUpdateLocationResult.Success)
         Conversation.ShowError(file_move_result);

      if (file_move_result == FileUpdateLocationResult.Success || file_move_result == FileUpdateLocationResult.SuccessWithRename)
         conversation.taxonomy_required = false;
   }
}