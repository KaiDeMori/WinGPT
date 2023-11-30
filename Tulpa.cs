using System.Collections.Immutable;
using System.Diagnostics;
using System.Text;
using Newtonsoft.Json;
using WinGPT.OpenAI;
using WinGPT.OpenAI.Chat;
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
   public FileInfo? File { get; set; }

   public TulpaConfiguration Configuration { get; init; } = new();

   /// <summary>
   /// The internal Tulpa messages, usually a system message.
   /// </summary>
   public ImmutableArray<Message> Messages { get; init; } = new();

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

      return tulpa;
   }

   /// <summary>
   /// Sends the API call.
   /// </summary>
   /// <param name="user_message">the new user prompt</param>
   /// <param name="conversation">The full conversation so far. Should usually not be mutated.</param>
   /// <param name="associated_files"></param>
   /// <returns>the new message to be added to the conversation</returns>
   public async Task<Message?> SendAsync(
      Message      user_message,
      Conversation conversation,
      FileInfo[]?  associated_files = null) {
      //  Pre-Production
      /////////////////////////

      //create a copy of the messages
      List<Message> tulpa_messages_togo =
         Messages.Select(m => m.Clone()).ToList();

      remove_last_user_message(tulpa_messages_togo);
      Request request;
      if (Tools.isVisionModel()) {
         request = Create_Vision_Request(user_message, conversation, associated_files, tulpa_messages_togo);
      }
      //else if (Tools.isImageGenerationModel()) {
      //   //DRAGONS not sure how to do it yet and cost calculation is also non-trivial in this case
      //}
      else {
         request = Create_Textual_Request(user_message, conversation, associated_files, tulpa_messages_togo);
      }

      // done with Pre-Production
      /////////////////////////////

      Response? response = await Completions.POST_Async(request);
      if (response == null) {
         // all error handling is done in the POST_Async method
         return null;
      }

      Choice? zeroth_Choice = response.Choices.FirstOrDefault();
      if (zeroth_Choice == null) {
         MessageBox.Show("The API returned no choices.", "Error", MessageBoxButtons.OK);
         return null;
      }

      Message response_message = new() {
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

      FunctionCall? function_call = zeroth_Choice.message.function_call;

      if (function_call is not null) {
         //Also the docs are vague about the finish reason, so let's check that too
         var finish_reason = zeroth_Choice.finish_reason;
         if (finish_reason != Finish_Reason.function_call) {
            MessageBox.Show($"The API returned a finish reason: {finish_reason}.\r\nWhat's going on here?!", "?!?!?!1?", MessageBoxButtons.YesNoCancel,
               MessageBoxIcon.Question);
         }

         //see if function name is "saveFunction"
         if (function_call.name == "saveFile") {
            Save_CallArguments? save_CallArguments = JsonConvert.DeserializeObject<Save_CallArguments>(
               function_call.arguments);
            if (save_CallArguments is null) {
               throw new Exception("The arguments are is null!");
            }

            Debug.WriteLine($"saveFunctionCall: {save_CallArguments}");
            //see if the file is in the associated files use the new AssociatedFilesSaver
            AssociatedFilesSaver.SaveFile(save_CallArguments.filename, save_CallArguments.text_content, associated_files, out var dummy_assistant_content);

            //in case we have no content for the user, provide some feedback
            if (response_message.content is null) {
               response_message = new Message {
                  role    = Role.assistant,
                  content = dummy_assistant_content
               };
            }
         }
      }

      //// done with post
      ///////////////////

      return response_message;
   }

   private Request Create_Vision_Request(Message userMessage, Conversation conversation, FileInfo[]? associatedFiles, List<Message> tulpaMessagesTogo) {
      VisionMessage newVisionMessage = VisionPreviewHelper.add_vision_preview_user_message(userMessage.content, associatedFiles);

      var all_messages = new List<Message> {
         newVisionMessage
      };

      var all_immutable = all_messages.ToImmutableList();

      Request request = new() {
         messages    = all_immutable,
         model       = Config.Active.LanguageModel,
         temperature = Configuration.Temperature,
         max_tokens  = Config.Active.UIable.Vision_Max_Tokens
      };
      return request;
   }

   private Request Create_Textual_Request(Message user_message, Conversation conversation, FileInfo[]? associated_files, List<Message> tulpa_messages_togo) {
      //DRAGONS not sure if we want to do this always
      //get the first system message or create a new one
      Message first_system_message = tulpa_messages_togo.FirstOrDefault(m => m.role == Role.system) ?? new Message {role = Role.system};

      //It's a StringBuilder. You can append to it. What do expect?
      //Just don't enumerate it.
      var tuned_up_system_message_content = new StringBuilder();

      //add the content of the old system message to the new one
      tuned_up_system_message_content.Append(first_system_message.content);

      if (Config.Active.UIable.Use_Save_Via_Link)
         add_save_link_preprompt(tuned_up_system_message_content);

      // "Upload"
      add_associated_files_to_system_message(associated_files, tuned_up_system_message_content);

      // if the save_function is null, the parameter will just be ignored
      Function<SaveParameters>? save_function = null;
      if (Config.Active.UIable.Use_Save_Via_Prompt)
         save_function = Enable_Save_via_Prompt_Function();

      var new_system_message = new Message {
         role    = Role.system,
         content = tuned_up_system_message_content.ToString(),
      };

      //replace the first system message with the new one, make sure its at the same position as the old one
      tulpa_messages_togo.Remove(first_system_message);
      tulpa_messages_togo.Insert(Math.Max(tulpa_messages_togo.IndexOf(first_system_message), 0), new_system_message);

      // concatenate the Tulpa_Messages and the conversation messages and the new prompt as a user message.
      List<Message> all_messages = new();
      all_messages.AddRange(tulpa_messages_togo);
      all_messages.AddRange(conversation.Messages);
      if (conversation.useSysMsgHack)
         all_messages.Add(new_system_message);

      all_messages.Add(user_message);

      var all_immutable = all_messages.ToImmutableList();

      Request request = new() {
         messages    = all_immutable,
         model       = Config.Active.LanguageModel,
         temperature = Configuration.Temperature,
         functions   = save_function is not null ? new IFunction[] {save_function} : null,
         max_tokens  = Config.Active.UIable.Max_Tokens
      };
      return request;
   }

   private static Function<SaveParameters>? Enable_Save_via_Prompt_Function() {
      var saveFile_function_json = System.IO.File.ReadAllText("Filetransfer/saveFile_Function.json");
      Function<SaveParameters>? saveFile_function =
         JsonConvert.DeserializeObject<Function<SaveParameters>>(saveFile_function_json);

      //Let's see if we can get away with the function only.
      //var saveFile_function_sysmsg = System.IO.File.ReadAllText("Filetransfer/saveFile_system_message.md");
      //tuned_up_system_message_content.AppendLine(saveFile_function_sysmsg);
      return saveFile_function;
   }

   private void add_associated_files_to_system_message(FileInfo[]? associated_files, StringBuilder tuned_up_system_message_content) {
      if (associated_files is null)
         return;

      //add the associated files to the system message
      foreach (var file in associated_files) {
         //now we want to add code files in a code block, text files with the simple filename wrapper and all other files not at all
         //use the shiny new FileTypeIdentifier
         var fileType = FileTypeIdentifier.GetFileType(file.FullName);
         switch (fileType) {
            case FileType.Code:
               var markdown_codeblock = Markf278DownHelper.create_markdown_code_block(file);
               tuned_up_system_message_content.AppendLine(markdown_codeblock);
               break;
            case FileType.Text:
               var markdown_textblock = Markf278DownHelper.create_markdown_text_block(file);
               tuned_up_system_message_content.AppendLine(markdown_textblock);
               break;
            case FileType.Image:
               //not available. We need to use the Vision API for that!
               break;
            case FileType.Other:
               //not available. Maybe we can add a link to the file?
               //or do some Base64 encoding?
               break;
            default:
               throw new ArgumentOutOfRangeException();
         }
      }
   }

   private static void add_save_link_preprompt(StringBuilder tuned_up_system_message_content) {
      var system_message = System.IO.File.ReadAllText("Filetransfer/save_link_system_message.md");
      tuned_up_system_message_content.AppendLine(Tools.nl);
      tuned_up_system_message_content.AppendLine(system_message);
      tuned_up_system_message_content.AppendLine(Tools.nl);
      tuned_up_system_message_content.AppendLine("-----");
   }

   private static void remove_last_user_message(List<Message> tulpa_messages_togo) {
      //now we need to check if the last message is a user role and if so,
      //remove it (it's considered to be a SamplePrompt)
      var last_message = tulpa_messages_togo.LastOrDefault();
      if (last_message?.role == Role.user)
         tulpa_messages_togo.Remove(last_message);
   }
}