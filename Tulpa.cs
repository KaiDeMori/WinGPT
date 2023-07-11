using System.Collections.Immutable;
using Newtonsoft.Json;
using WinGPT.OpenAI.Chat;
using Message = WinGPT.OpenAI.Chat.Message;

namespace WinGPT;

/// <summary>
/// Tulpa is a concept in Theosophy, mysticism, and the paranormal, of a materialized thought form,
/// typically in human form such as an imaginary friend or being that is created through spiritual practice and intense concentration.
/// https://en.wikipedia.org/wiki/Tulpa
/// </summary>
public class Tulpa : InterTulpa {
   [JsonIgnore]
   public FileInfo File { get; init; }

   public TulpaConfiguration Configuration { get; init; } = new();

   /// <summary>
   /// The internal Tulpa messages, usually a system message.
   /// </summary>
   public List<Message> Messages { get; init; } = new();

   private const string Tulpa_Code_Token   = "📜Code";
   private const string Tulpa_Config_Token = "🛠️Configuration";

   //TADA should use Either monad here for error msg accumulation
   public static Tulpa? CreateFrom(FileInfo file) {
      if (!file.Exists) {
         MessageBox.Show($"The file {file} does not exist.", "Error", MessageBoxButtons.OK);
         return null;
      }

      var name   = Path.GetFileNameWithoutExtension(file.Name);
      var text   = System.IO.File.ReadAllText(file.FullName);
      var result = TulpaParser.TryParse(text, file, out var tulpa);
      if (!result) {
         MessageBox.Show($"The file {file} could not be parsed.", "Error", MessageBoxButtons.OK);
         return null;
      }

      return tulpa;
   }

   public void Activate(Conversation conversation) {
      //nothing to do here for now.
      //overwrite in custom InterTulpas
   }

   public Conversation NewConversation() {
      string conversation_Filename = Create_New_Conversation_Filename();
      var    full_HistoryDirectory = ConversationHistory.GetAbsoluteHistoryDirectory();
      //we need the absolute path here as a FileInfo
      var full_HistoryFile = new FileInfo(Path.Combine(full_HistoryDirectory.FullName, conversation_Filename));
      var conversation     = new Conversation(full_HistoryFile);
      return conversation;
   }

   /// <summary>
   /// Sends the API call.
   /// </summary>
   /// <param name="user_message">the new user prompt</param>
   /// <param name="conversation">The full conversation so far. Should usually not be mutated.</param>
   /// <returns>the new messages to be added to the conversation</returns>
   public async Task<List<Message>> SendAsync(Message user_message, Conversation conversation) {
      List<Message> new_messages = new();

      // concatenate the Tulpa_Messages and the conversation messages and the new prompt as a user message.
      List<Message> all_messages = new();
      all_messages.AddRange(this.Messages);
      all_messages.AddRange(conversation.Messages);
      if (conversation.useSysMsgHack)
         all_messages.AddRange(this.Messages);
      all_messages.Add(user_message);
      var all_immutable = all_messages.ToImmutableList();

      //debug as json
      var all_json = JsonConvert.SerializeObject(all_immutable, Formatting.Indented);
      System.IO.File.WriteAllText("_all_.json", all_json);

      Request request = new() {
         messages    = all_immutable,
         model       = Config.Active.LanguageModel,
         temperature = Configuration.Temperature,
      };

      Response? response = await Completion.POST_Async(request);
      if (response == null) {
         // all error handling is done in the POST_Async method
         return new_messages;
      }

      Choice? zeroth_Choice = response.Choices.FirstOrDefault();
      if (zeroth_Choice == null) {
         MessageBox.Show("The API returned an empty response.", "Error", MessageBoxButtons.OK);
         return new_messages;
      }

      Message response_message = zeroth_Choice.Message;
      new_messages.Add(response_message);

      return new_messages;
   }


   private string Create_New_Conversation_Filename() {
      var tulpa_name     = Configuration.Name;
      var valid_filename = Tools.To_valid_filename(tulpa_name);
      var name_only      = $"{valid_filename} - {DateTime.Now:yyyy-MM-dd HH-mm-ss}";
      var filename       = $"{name_only}{Config.marf278down_extenstion}";
      //TADA some directory stuff here
      return filename;
   }
}