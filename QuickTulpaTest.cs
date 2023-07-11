using System.Diagnostics;
using System.Text;
using Newtonsoft.Json;
using Message = WinGPT.OpenAI.Chat.Message;
namespace WinGPT;

internal class QuickTulpaTest {
   public static void run() {
        // C:\Users\devboese\Documents\_dev\WinGPTTests\TestTulpas
        var filenames = Directory.GetFiles(@"../../../../WinGPTTests\TestTulpas", "*.md");
      foreach (var filename in filenames) {
         //var tul    = Tulpa.CreateFrom(filename);
         var allText = File.ReadAllText(filename);
         //var lines   = File.ReadAllLines(filename);
         var name    = Path.GetFileNameWithoutExtension(filename);
         var file    = new FileInfo(filename);
         var result  = TulpaParser.TryParse(allText, file, out var tulpa);
         if (!result) {
            Debugger.Break();
            break;
         }

         //create a json string from the tulpa object
         var json = JsonConvert.SerializeObject(tulpa, Formatting.Indented);
         //write the json string to a file
         File.WriteAllText($"Characters/{name}.json", json);
      }

      //MessageBox.Show("QuickTulpaTest passed!");
      Environment.Exit(0);
   }

   public static Tulpa ParseMessages(string content) {
      var specialTokens = new Dictionary<string, Role> {
         {SpecialTokens.System, Role.system},
         {SpecialTokens.User, Role.user},
         {SpecialTokens.Assistant, Role.assistant},
         {SpecialTokens.Function, Role.Function},
      };

      var contentMemory = content.AsMemory();
      var currentRole   = Role.system; // Default role.
      var messageStart  = 0;
      var messages      = new List<Message>();

      // Default configuration.
      var tulpa_config = new TulpaConfiguration();

      // Handle the Configuration part at the beginning.
      if (contentMemory.Span.StartsWith(SpecialTokens.Configuration)) {
         int configEnd = -1;
         foreach (var specialToken in specialTokens) {
            configEnd = contentMemory.Span.IndexOf(specialToken.Key);
            if (configEnd != -1) break;
         }

         if (configEnd != -1) {
            var configContent = contentMemory.Slice(messageStart, configEnd - messageStart).ToString();
            // Assume ParseConfig is your function to parse configuration.
            tulpa_config = ParseConfig(configContent);

            messageStart = configEnd;
         }
      }

      for (var i = messageStart; i < contentMemory.Length; i++) {
         foreach (var specialToken in specialTokens) {
            if (contentMemory.Slice(i).Span.StartsWith(specialToken.Key)) {
               var messageContent = contentMemory.Slice(messageStart, i - messageStart).ToString();

               if (!string.IsNullOrWhiteSpace(messageContent)) {
                  messages.Add(new Message {role = currentRole, content = messageContent});
               }

               currentRole  =  specialToken.Value;
               i            += specialToken.Key.Length;
               messageStart =  i;
               break;
            }
         }
      }

      // Add the last message.
      var lastMessage = contentMemory.Slice(messageStart).ToString();
      if (!string.IsNullOrWhiteSpace(lastMessage)) {
         messages.Add(new Message {role = currentRole, content = lastMessage});
      }

      return new Tulpa {Configuration = tulpa_config, Messages = messages};
   }


   private static TulpaConfiguration ParseConfig(string configContent) {
      Debug.WriteLine(configContent);
      return null;
   }
}