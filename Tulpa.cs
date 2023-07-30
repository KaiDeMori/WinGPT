using System.Collections.Immutable;
using System.Diagnostics;
using System.Text;
using Newtonsoft.Json;
using WinGPT.Filetransfer;
using WinGPT.OpenAI.Chat;
using Message = WinGPT.OpenAI.Chat.Message;

namespace WinGPT;

/// <summary>
/// Tulpa is a concept in Theosophy, mysticism, and the paranormal, of a materialized thought form,
/// typically in human form such as an imaginary friend or being that is created through spiritual practice and intense concentration.
/// https://en.wikipedia.org/wiki/Tulpa
/// </summary>
public class Tulpa : InterTulpa {
   private string markdown_codeblock;

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
      var text   = System.IO.File.ReadAllText(file.FullName);
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
   /// <returns>the new messages to be added to the conversation</returns>
   public async Task<Message[]> SendAsync(
      Message      user_message,
      Conversation conversation,
      FileInfo[]?  associated_files = null) {
      //  Pre-Production
      List<Message>  new_messages        = new();
      List<Message>? tulpa_messages_togo = null;

      //  Post-Production
      /////////////////////////

      //create a copy of the messages
      tulpa_messages_togo = Messages.Select(m => m.Clone()).ToList();

      remove_last_user_message(tulpa_messages_togo);

      //DRAGONS not sure of we want to do this always
      //get the first system message or create a new one
      Message first_system_message = tulpa_messages_togo.FirstOrDefault(m => m.role == Role.system) ?? new Message {role = Role.system};

      //It's a StringBuilder. You can append to it. What do expect?
      //Just don't enumerate it.
      var tuned_up_system_message_content = new StringBuilder();
      //add the content of the old system message to the new one
      tuned_up_system_message_content.Append(first_system_message.content);

      // "Upload"
      add_associated_files_to_system_message(associated_files, tuned_up_system_message_content);

      // if the save_function is null, the parameter will just be ignored
      Function<SaveParameters>? save_function = null;
      if (Config.Active.UIable.Use_Save_Function)
         save_function = Enable_Save_via_Prompt_Function(tuned_up_system_message_content);

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
         functions   = save_function is not null ? new IFunction[] {save_function} : null
      };

      Response? response = await Completions.POST_Async(request);
      if (response == null) {
         // all error handling is done in the POST_Async method
         return new_messages.ToArray();
      }

      //Let's look for a function message
      //var function_messages = response.Choices.Where(c => c.message.role == Role.function).ToList();
      //and thats not how it's done.

      //So apparently we need to find all messages that have role=assitant, content=None, function_call != null
      //and select the first message in each choice
      Message[] function_messages = response.Choices
         .Where(c => c.message is {role: Role.assistant, content: null, function_call: not null})
         .Select(c => c.message)
         .ToArray();


      //The docs don't really say it,
      //but for now we assume that the AI will only ever call one function per request.
      if (function_messages.Length > 1) {
         MessageBox.Show("The API returned more than one function message.\r\nPlease call me!\r\nThis is potentiall a huge discovery!1!", "SUCCESS!",
            MessageBoxButtons.CancelTryContinue, MessageBoxIcon.Asterisk);
         return new_messages.ToArray();
      }

      var function_message = function_messages.FirstOrDefault();
      if (function_message?.function_call is not null) {
         //Also the docs are vague about the finish reason, so let's check that too
         var finish_reason = response.Choices.FirstOrDefault()?.finish_reason;
         if (finish_reason != Finish_Reason.function_call) {
            MessageBox.Show($"The API returned a finish reason: {finish_reason}.\r\nWhat's going on here?!", "?!?!?!1?", MessageBoxButtons.YesNoCancel,
               MessageBoxIcon.Question);
         }

         //see if function name is "saveFunction"
         if (function_message.function_call.name == "saveFile") {
            Save_CallArguments? save_CallArguments = JsonConvert.DeserializeObject<Save_CallArguments>(
               function_message.function_call.arguments);
            if (save_CallArguments is null) {
               throw new Exception("The arguments are is null!");
            }

            Debug.WriteLine($"saveFunctionCall: {save_CallArguments}");
            //see if the file is in the associated files
            var file_to_save =
               associated_files?.FirstOrDefault(f => f.Name == save_CallArguments.filename)
               ?? new FileInfo(Path.Join(Config.AdHoc_Downloads_Path.FullName, save_CallArguments.filename));
            //save the file
            System.IO.File.WriteAllText(file_to_save.FullName, save_CallArguments.text_content);
         }
      }


      //// done with post
      ///////////////////


      Choice? zeroth_Choice = response.Choices.FirstOrDefault();
      if (zeroth_Choice == null) {
         MessageBox.Show("The API returned an empty response.", "Error", MessageBoxButtons.OK);
         return new_messages.ToArray();
      }

      Message response_message = zeroth_Choice.message;
      new_messages.Add(response_message);

      return new_messages.ToArray();
   }

   private static Function<SaveParameters>? Enable_Save_via_Prompt_Function(StringBuilder tuned_up_system_message_content) {
      var saveFile_function_json = System.IO.File.ReadAllText("Filetransfer/saveFile_Function.json");
      Function<SaveParameters>? saveFile_function =
         JsonConvert.DeserializeObject<Function<SaveParameters>>(saveFile_function_json);

      var saveFile_function_sysmsg = System.IO.File.ReadAllText("Filetransfer/system_message_for_file_download.md");
      tuned_up_system_message_content.AppendLine(saveFile_function_sysmsg);
      return saveFile_function;
   }

   private void add_associated_files_to_system_message(FileInfo[]? associated_files, StringBuilder tuned_up_system_message_content) {
      if (associated_files is not null) {
         //add the associated files to the system message

         var sysmsg4code = System.IO.File.ReadAllText("Filetransfer/system_message_for_file_upload.md");
         tuned_up_system_message_content.AppendLine(Tools.nl);
         tuned_up_system_message_content.AppendLine(sysmsg4code);
         tuned_up_system_message_content.AppendLine(Tools.nl);
         tuned_up_system_message_content.AppendLine("-----");

         foreach (var file in associated_files) {
            markdown_codeblock = create_markdown_code_block(file);
            tuned_up_system_message_content.AppendLine(Tools.nl);
            tuned_up_system_message_content.AppendLine("-----");
            tuned_up_system_message_content.AppendLine(Tools.nl);
            tuned_up_system_message_content.AppendLine(markdown_codeblock);
         }
      }
   }

   private static void remove_last_user_message(List<Message> tulpa_messages_togo) {
      //now we need to check if the last message is a user role and if so,
      //remove it (it's considered to be a SamplePrompt)
      var last_message = tulpa_messages_togo.LastOrDefault();
      if (last_message?.role == Role.user)
         tulpa_messages_togo.Remove(last_message);
   }

   private static string create_markdown_code_block(FileInfo file) {
      var file_content = System.IO.File.ReadAllText(file.FullName);
      var markdown     = new StringBuilder();
      markdown.Append("### ");
      markdown.Append(file.Name);
      markdown.Append("{.external-filename}");
      markdown.Append(Tools.nl);
      markdown.Append("```");
      markdown.Append(Tools.nl);
      markdown.Append(file_content);
      markdown.Append(Tools.nl);
      markdown.Append("```");
      markdown.Append(Tools.nl);
      return markdown.ToString();
   }
}