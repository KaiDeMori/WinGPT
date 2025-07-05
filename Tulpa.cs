using System.Collections.Immutable;
using System.Diagnostics;
using System.Text;
using Newtonsoft.Json;
using WinGPT.markf278digger;
using WinGPT.OpenAI;
using WinGPT.OpenAI.Chat;
using WinGPT.Taxonomy;
using WinGPT.Tokenizer;
using JsonSerializer = System.Text.Json.JsonSerializer;
using Message = WinGPT.OpenAI.Chat.Message;

// ReSharper disable MethodHasAsyncOverload

namespace WinGPT;

/// <summary>
/// Tulpa is a concept in Theosophy, mysticism, and the paranormal, of a materialized thought form,
/// typically in human form such as an imaginary friend or being that is created through spiritual practice and intense concentration.
/// https://en.wikipedia.org/wiki/Tulpa
/// </summary>
public class Tulpa : InterTulpa {
   [JsonIgnore]
   public int Token_Count { get; private set; }

   [JsonIgnore]
   public FileInfo? File { get; set; }

   public TulpaConfiguration Configuration { get; init; } = new();

   /// <summary>
   /// The internal Tulpa messages, usually a system message.
   /// </summary>
   public ImmutableArray<Simple_Message> Messages { get; init; } = new();

   //TADA should use Either monad here for error msg accumulation
   public static Tulpa? CreateFrom(FileInfo file) {
      if (!file.Exists) {
         MessageBox.Show($"The file {file} does not exist.", "Error", MessageBoxButtons.OK);
         return null;
      }

      //var name   = Path.GetFileNameWithoutExtension(file.Name);
      using var stream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
      using var reader = new StreamReader(stream);
      var       text   = reader.ReadToEnd();

      var result = TulpaParser.TryParse(text, file, out var tulpa);
      if (!result) {
         MessageBox.Show($"The file {file} could not be parsed.", "Error", MessageBoxButtons.OK);
         return null;
      }

      //why do we need this? somehow the [MaybeNullWhen(false)] attribute is not working
      if (tulpa is null)
         return null;

      //this isnt working, we need to refactore some stuff first
      //var dummy_request = tulpa.CreateRequest(
      //   new Message (Role.user, String.Empty),
      //   new Conversation(), 
      //   new FileInfo[] { });

      //in the end, we just want the token count for this tulpa, without any messages or conversations or files
      //just the raw tulpa messages without the last user message as a string

      // get a copy of the tulpa messages but without a last user message
      var tulpa_messages_togo = tulpa.Messages.Select(m => m.Clone()).ToList();
      remove_last_user_message(tulpa_messages_togo);

      return tulpa;
   }

   public Request Create_Multimodal_Request(Complex_Message user_message, Conversation conversation, FileInfo[]? associated_files = null) {
      // Pre-Production
      /////////////////////////

      var tulpa_messages_togo = Messages.Select(m => m.Clone()).ToList();
      // Special case for tulpas with an example prompt
      remove_last_user_message(tulpa_messages_togo);

      //DRAGONS not sure if we want to do this always
      //get the first system message or create a new one
      var first_system_message = tulpa_messages_togo.FirstOrDefault(m => m.role == Role.developer) ?? new Simple_Message {role = Role.developer};

      //It's a StringBuilder. You can append to it. What do expect?
      //Just don't enumerate it.
      var tuned_up_system_message_content = new StringBuilder();

      if (Config.Active.UIable.reenable_formatting)
         tuned_up_system_message_content.AppendLine(Config_UIable.reenable_formatting_spell);

      //add the content of the old system message to the new one
      tuned_up_system_message_content.Append(first_system_message.content);

      //not a function! just a pre-prompt for markf278down
      if (Config.Active.UIable.Use_Save_Via_Link)
         spice_up_system_message("Filetransfer/save_link_system_message.md", tuned_up_system_message_content);

      if (Config.Active.UIable.Math_Rendering)
         spice_up_system_message("markf278digger/math_render_block_system_message.md", tuned_up_system_message_content);

      // "Upload" not to system message anymore.
      //add_associated_files_to_system_message(associated_files, tuned_up_system_message_content);

      var system_message_content = tuned_up_system_message_content.ToString();
      //content is a list now!
      var new_system_message = new Simple_Message() {
         role    = Role.developer,
         content = system_message_content
      };
      if (!string.IsNullOrEmpty(system_message_content)) {
         //replace the first system message with the new one, make sure its at the same position as the old one
         tulpa_messages_togo.Remove(first_system_message);
         tulpa_messages_togo.Insert(Math.Max(tulpa_messages_togo.IndexOf(first_system_message), 0), new_system_message);
      }

      // concatenate the Tulpa_Messages and the conversation messages and the new prompt as a user message.
      List<Message> all_messages = [.. tulpa_messages_togo, .. conversation.Messages];

      if (conversation.useSysMsgHack)
         all_messages.Add(new_system_message);

      add_images_to_user_message(user_message, associated_files);
      add_documents_to_user_message(user_message, associated_files);
      //Add the code files to the **user message**, since the new models really don't like it in the system-message.
      add_associated_code_files_to_user_message(user_message, associated_files);

      all_messages.Add(user_message);

      var all_immutable = all_messages.ToImmutableList();

      double? temperature = Configuration.Temperature;
      if (Models.get_active_Model().no_temperature)
         temperature = null;

      Service_Tier st = Config.Active.UIable.service_tier;
      if (st == Service_Tier.flex && !Models.get_active_Model().supports_flex_tier)
         st = Service_Tier.auto;

      Request request = new() {
         messages    = all_immutable,
         model       = Config.Active.Language_Model,
         temperature = temperature,
         tool_choice = null,
         //functions   = save_function is not null ? [save_function] : null,
         max_tokens   = Config.Active.UIable.Max_Tokens,
         service_tier = st
      };

      //lezzgotools!
      if (Config.Active.UIable.Use_Save_Via_Prompt) {
         Enable_Save_via_Prompt_Function(request.tools);
         //set proper tool_choice
         //request.tool_choice = new ToolChoice(request.tools.First().function.name);
      }

      return request;
   }

   /// <summary>
   /// So far, only PDF is really supported. And only with vision models.
   /// <see href="https://platform.openai.com/docs/guides/pdf-files?api-mode=chat">API documentation</see>
   /// </summary>
   /// <param name="user_message"></param>
   /// <param name="associated_files"></param>
   private void add_documents_to_user_message(Complex_Message user_message, FileInfo[]? associated_files) {
      if (associated_files is null)
         return;

      if (!Models.get_active_Model().is_vision_model)
         return;

      foreach (var file in associated_files) {
         if (FileTypeIdentifier.GetFileType(file.FullName) != FileType.Document)
            continue;

         string base64DataUrl = FileHelper.get_base64_data_url(file);
         document_content_part documentContent = new() {
            file = new() {
               filename  = file.Name,
               file_data = base64DataUrl
            }
         };
         user_message.content.Add(documentContent);
      }
   }

   private void add_images_to_user_message(Complex_Message user_message, FileInfo[]? associated_files) {
      if (associated_files is null)
         return;

      if (!Models.get_active_Model().is_vision_model)
         return;

      foreach (var file in associated_files) {
         if (FileTypeIdentifier.GetFileType(file.FullName) != FileType.Image)
            continue;

         string base64DataUrl = FileHelper.get_base64_data_url(file);
         var imageContent = new image_content_part {
            image_url = new image_url {
               url    = base64DataUrl,
               detail = Config.Active.UIable.image_detail
            }
         };
         user_message.content.Add(imageContent);
      }
   }

   private static void Enable_Save_via_Prompt_Function(List<Tool> tools) {
      var saveFile_function_json = System.IO.File.ReadAllText("Filetransfer/saveFile_Function.json");
      //Function<SaveParameters>? saveFile_function = JsonConvert.DeserializeObject<Function<SaveParameters>>(saveFile_function_json);

      JsonSerializerSettings settings = new() {
         Error = (sender, args) => throw new Exception(args.ErrorContext.Error.Message)
      };
      Function? saveFile_function = JsonConvert.DeserializeObject<Function>(saveFile_function_json, settings);

      if (saveFile_function is null)
         throw new Exception("The save function could not be parsed!");

      //Let's see if we can get away with the function only.
      //var saveFile_function_sysmsg = System.IO.File.ReadAllText("Filetransfer/saveFile_system_message.md");
      //tuned_up_system_message_content.AppendLine(saveFile_function_sysmsg);

      tools.Add(new Tool(saveFile_function));
   }

   private static void add_associated_files_to_system_message(
      FileInfo[]?   associated_files,
      StringBuilder tuned_up_system_message_content) {
      if (associated_files is null)
         return;

      //add the associated files to the system message
      foreach (var file in associated_files) {
         Markf278DownHelper.create_markdown_for_file(tuned_up_system_message_content, file);
      }
   }

    private static void add_associated_code_files_to_user_message(Complex_Message user_message, FileInfo[]? associated_files) {
      if (associated_files is null)
         return;

      foreach (var file in associated_files) {
         if (FileTypeIdentifier.GetFileType(file.FullName) != FileType.Code)
            continue;

         string base64DataUrl = FileHelper.get_base64_data_url(file);
         document_content_part documentContent = new() {
            file = new() {
               filename  = file.Name,
               file_data = base64DataUrl
            }
         };
         user_message.content.Add(documentContent);
      }
   }

   /// <summary>
   /// Sends the API call.
   /// </summary>
   /// <param name="user_message">the new user prompt</param>
   /// <param name="conversation">The full conversation so far. Should usually not be mutated.</param>
   /// <param name="associated_files"></param>
   /// <returns>the new message to be added to the conversation</returns>
   public async Task<Message?> SendAsync(
      Complex_Message user_message,
      Conversation    conversation,
      FileInfo[]?     associated_files = null) {
      ////  Pre-Production
      ///////////////////////////

      Request request;
      // We only support complex requests for now. Some older or preview models may
      // not support this, but le's just see.
      request = Create_Multimodal_Request(user_message, conversation, associated_files);

      // done with Pre-Production
      /////////////////////////////

      Response? response = await Completions.POST_Async(request);
      if (response == null) {
         // all error handling is done in the POST_Async method
         return null;
      }

      Choice? zeroth_Choice = response.choices.FirstOrDefault();
      if (zeroth_Choice == null) {
         MessageBox.Show("The API returned no choices.", "Error", MessageBoxButtons.OK);
         return null;
      }

      //as far as I understand the docs, the response will always be simple

      Simple_Message response_message = new Simple_Message() {
         role    = Role.assistant,
         content = zeroth_Choice.message.content
      };

      //  Post-Production
      /////////////////////////

      //Let's look for a function message
      //var function_messages = response.Choices.Where(c => c.message.role == Role.function).ToList();
      //and thats not how it's done.

      //So apparently we need to find all messages that have role=assitant, content=None, function_call != null
      //and select the first message in each choice
      //Message[] function_call_messages = response.Choices
      //   .Where(c => c.message is {role: Role.assistant, content: null, function_call: not null})
      //   .Select(c => c.message)
      //   .ToArray();
      //and wrong *again*! *Of course* the AI can answer with a function call **and** content in the same message.

      //FunctionCall? function_call = zeroth_Choice.message.function_call;

      var           tool_calls            = zeroth_Choice.message.tool_calls ?? [];
      StringBuilder function_call_content = new StringBuilder();
      foreach (var tool_call in tool_calls) {
         if (tool_call.function.name == "saveFile") {
            Save_CallArguments? save_CallArguments = JsonConvert.DeserializeObject<Save_CallArguments>(tool_call.function.arguments);
            if (save_CallArguments is null) {
               throw new Exception("The arguments are is null!");
            }

            Debug.WriteLine($"saveFunctionCall: {save_CallArguments}");
            //see if the file is in the associated files use the new AssociatedFilesSaver
            AssociatedFilesSaver.SaveFile(save_CallArguments.filename, save_CallArguments.text_content, associated_files, out var dummy_assistant_content);

            //in case we have no content for the user, provide some feedback
            function_call_content.AppendLine(dummy_assistant_content);
         }
      }

      if (tool_calls.Length > 0 && string.IsNullOrWhiteSpace(response_message.content)) {
         response_message = new Simple_Message {
            role    = Role.assistant,
            content = function_call_content.ToString()
         };
      }


      //old code. now we support multiple tool calls
      //Tool_Call.Function? function_call = zeroth_Choice.message.tool_calls?.FirstOrDefault()?.function;

      //if (function_call is not null) {
      //   //Also the docs are vague about the finish reason, so let's check that too
      //   var finish_reason = zeroth_Choice.finish_reason;
      //   if (finish_reason != Finish_Reason.tool_calls) {
      //      MessageBox.Show($"The API returned a finish reason: {finish_reason}.\r\nWhat's going on here?!", "?!?!?!1?", MessageBoxButtons.YesNoCancel,
      //         MessageBoxIcon.Question);
      //   }

      //   //see if function name is "saveFunction"
      //   if (function_call.name == "saveFile") {
      //      Save_CallArguments? save_CallArguments = JsonConvert.DeserializeObject<Save_CallArguments>(
      //         function_call.arguments);
      //      if (save_CallArguments is null) {
      //         throw new Exception("The arguments are is null!");
      //      }

      //      Debug.WriteLine($"saveFunctionCall: {save_CallArguments}");
      //      //see if the file is in the associated files use the new AssociatedFilesSaver
      //      AssociatedFilesSaver.SaveFile(save_CallArguments.filename, save_CallArguments.text_content, associated_files, out var dummy_assistant_content);

      //      //in case we have no content for the user, provide some feedback
      //      if (string.IsNullOrWhiteSpace(response_message.content)) {
      //         response_message = new Simple_Message() {
      //            role    = Role.assistant,
      //            content = dummy_assistant_content
      //         };
      //      }
      //   }
      //}

      //// done with post
      ///////////////////

      return response_message;
   }

   /// <summary>
   /// Special requests for "preview" models.
   /// Actually they are so expensive, that I should remove them.
   /// Very limited functionality: NO functions, NO system messages, NO associated files, NO images
   /// </summary>
   /// <param name="user_message"></param>
   /// <param name="conversation"></param>
   /// <param name="associated_files"></param>
   /// <returns></returns>
   private Request Create_Preview_Request(Simple_Message user_message, Conversation conversation, FileInfo[]? associated_files = null) {
      // Pre-Production
      /////////////////////////

      var tulpa_messages_togo = Messages.Select(m => m.Clone()).ToList();
      // Special case for tulpas with an example prompt
      remove_last_user_message(tulpa_messages_togo);

      var first_system_message = tulpa_messages_togo.FirstOrDefault(m => m.role == Role.developer);

      //for now send as "USER" message
      Simple_Message fake_system_message = new() {
         role    = Role.user,
         content = first_system_message?.content ?? string.Empty
      };

      var tuned_up_fake_system_message_content = new StringBuilder();

      //add the content of the old system message to the new one
      tuned_up_fake_system_message_content.Append(fake_system_message.content);

      if (Config.Active.UIable.Math_Rendering)
         spice_up_system_message("markf278digger/math_render_block_system_message.md", tuned_up_fake_system_message_content);


      var system_message_content = tuned_up_fake_system_message_content.ToString();
      //content is a list now!
      var new_system_message = new Simple_Message() {
         role    = Role.user,
         content = system_message_content
      };
      if (!string.IsNullOrEmpty(system_message_content)) {
         //replace the first system message with the new one, make sure its at the same position as the old one
         tulpa_messages_togo.Remove(fake_system_message);
         tulpa_messages_togo.Insert(Math.Max(tulpa_messages_togo.IndexOf(fake_system_message), 0), new_system_message);
      }

      // concatenate the Tulpa_Messages and the conversation messages and the new prompt as a user message.
      List<Message> all_messages = [
         .. tulpa_messages_togo,
         .. conversation.Messages,
         user_message
      ];

      var all_immutable = all_messages.ToImmutableList();

      Request request = new() {
         messages = all_immutable,
         model    = Config.Active.Language_Model,
      };

      return request;
   }

   private void spice_up_system_message(string text_file, StringBuilder tuned_up_system_message_content) {
      var system_message = System.IO.File.ReadAllText(text_file);
      tuned_up_system_message_content.AppendLine(Tools.nl);
      tuned_up_system_message_content.AppendLine(system_message);
      tuned_up_system_message_content.AppendLine(Tools.nl);
      tuned_up_system_message_content.AppendLine("-----");
   }

   private static void remove_last_user_message(List<Simple_Message> tulpa_messages_togo) {
      //now we need to check if the last message is a user role and if so,
      //remove it (it's considered to be a SamplePrompt)
      var last_message = tulpa_messages_togo.LastOrDefault();
      if (last_message?.role == Role.user)
         tulpa_messages_togo.Remove(last_message);
   }

   public void update_token_count() {
      Token_Count = CountTokenizer.count(
         string.Join("\n", Messages.Select(m => m.content)),
         Config.Active.Language_Model);
   }

   public static void create_Default_Tulpa_if_none_exist() {
      if (Config.Tulpa_Directory.GetFiles(Config.marf278down_filter).Length > 0)
         return;

      var default_tulpa_file = new FileInfo(Path.Join(Config.Tulpa_Directory.FullName, Config.DefaultAssistant_Filename));
      System.IO.File.WriteAllText(default_tulpa_file.FullName, "");
   }
}