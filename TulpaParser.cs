using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using WinGPT.OpenAI.Chat;

namespace WinGPT;

public static class TulpaParser {
   public static Dictionary<string, Role> Special_Tokens = new() {
      {SpecialTokens.System, Role.system},
      {SpecialTokens.User, Role.user},
      {SpecialTokens.Assistant, Role.assistant},
      {SpecialTokens.Function, Role.function}
   };

   public static bool TryParse(string content, FileInfo file, [MaybeNullWhen(false)] out Tulpa tulpa) {
      var content_memory = content.AsMemory();
      var current_role   = Role.system; // Default role.
      var message_start  = 0;
      var messages       = new List<Simple_Message>();

      // Default configuration.
      var tulpa_config = new TulpaConfiguration();

      // Handle the Configuration part at the beginning.
      if (content_memory.Span.StartsWith(SpecialTokens.Tulpa_Config_Token)) {
         int configEnd = -1;
         foreach (var specialToken in Special_Tokens) {
            configEnd = content_memory.Span.IndexOf(specialToken.Key);
            if (configEnd != -1) break;
         }

         if (configEnd == -1)
            configEnd = content_memory.Length;

         if (configEnd != -1) {
            var configContent = content_memory.Slice(message_start, configEnd - message_start).ToString();
            if (TryParseConfiguration(configContent, out var config)) {
               tulpa_config = config;
            }
            else {
               tulpa = null;
               return false;
            }

            message_start = configEnd;
         }
      }

      // Use the file name if Name property is empty.
      if (string.IsNullOrWhiteSpace(tulpa_config.Name)) {
         tulpa_config.Name = Path.GetFileNameWithoutExtension(file.Name);
      }

      int newLineLength = Environment.NewLine.Length;

      for (var i = message_start; i < content_memory.Length; i++) {
         foreach (var special_Token in Special_Tokens) {
            if (content_memory[i..].Span.StartsWith(special_Token.Key)) {
               //string messageContent = contentMemory.Slice(messageStart, i - messageStart - Environment.NewLine.Length).ToString();
               int content_length = i - message_start - newLineLength;

               // Ensure length is positive
               if (content_length < 0) content_length = 0;

               string message_content =
                  content_memory.Slice(message_start, content_length).ToString();

               if (!string.IsNullOrWhiteSpace(message_content)) {
                  messages.Add(new Simple_Message {
                     role = current_role, content = message_content
                  });
               }

               current_role  =  special_Token.Value;
               i             += special_Token.Key.Length;
               message_start =  i;
               break;
            }
         }
      }

      // Add the last message.
      var last_Message = content_memory.Slice(message_start).ToString();
      if (!string.IsNullOrWhiteSpace(last_Message)) {
         //We have this edge case, where the first line is one of the role tokens and we have to remove that from the message.
         //But the token will have a newline in front, so we first have to create a proper structure for that.
         //We get all Role tokens, remove the first newline and create a list from that.
         var role_Tokens = Special_Tokens.Keys.Select(k => k[Environment.NewLine.Length..]).ToList();
         //Then we check if the lastMessage starts with one of the role tokens.
         var matchingToken = role_Tokens.FirstOrDefault(last_Message.StartsWith);
         if (matchingToken != null) {
            //If it does, we remove the token from the start of lastMessage.
            last_Message = last_Message[matchingToken.Length..];
         }

         messages.Add(new Simple_Message {
            role = current_role, content = last_Message
         });
      }

      tulpa = new Tulpa {
         Configuration = tulpa_config,
         Messages      = [.. messages],
         File          = file
      };
      return true;
   }

   public static bool TryParseConfiguration(string lines, [NotNullWhen(true)] out TulpaConfiguration? config) {
      config = new TulpaConfiguration();
      var properties = typeof(TulpaConfiguration).GetProperties();
      string[] parsedLines = lines.Split(
         [Environment.NewLine], StringSplitOptions.RemoveEmptyEntries);

      foreach (var line in parsedLines) {
         foreach (var property in properties) {
            var property_name = property.Name + ":";
            if (line.StartsWith(property_name)) {
               string valueString = line[property_name.Length..].Trim();
               if (property.PropertyType == typeof(float)) {
                  if (!float.TryParse(valueString, NumberStyles.Float, CultureInfo.InvariantCulture, out float value)) {
                     config = null;
                     return false;
                  }

                  property.SetValue(config, value);
               }
               else {
                  property.SetValue(config, valueString);
               }

               break;
            }
         }
      }

      return true;
   }
}