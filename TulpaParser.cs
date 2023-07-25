using System.Diagnostics.CodeAnalysis;
using Message = WinGPT.OpenAI.Chat.Message;

namespace WinGPT;

public static class TulpaParser {
   public static Dictionary<string, Role> specialTokens = new Dictionary<string, Role> {
      {SpecialTokens.System, Role.system},
      {SpecialTokens.User, Role.user},
      {SpecialTokens.Assistant, Role.assistant},
      {SpecialTokens.Function, Role.function},
   };


   public static bool TryParse(string content, FileInfo file, [NotNullWhen(true)] out Tulpa? tulpa) {
      // Default configuration.
      var  tulpa_config       = new TulpaConfiguration();
      var  messages           = new List<Message>();

      //We need this in order to **NOT** add the system message twice when there is only a system message
      bool systemMessageAdded = false;

      if (content.StartsWith(SpecialTokens.Tulpa_Config_Token)) {
         int configEnd                  = content.IndexOf(SpecialTokens.System);
         if (configEnd == -1) configEnd = content.Length;

         var configContent = content[0..configEnd];
         if (!TryParseConfiguration(configContent, out tulpa_config)) {
            tulpa = null;
            return false;
         }

         content = content[configEnd..];
      }
      else {
         messages.Add(new Message {role = Role.system, content = content});
         tulpa_config.Name  = Path.GetFileNameWithoutExtension(file.Name);
         systemMessageAdded = true;
      }

      int  newLineLength = Environment.NewLine.Length;
      int  messageStart  = 0;
      Role currentRole   = Role.system;

      for (var i = 0; i < content.Length; i++) {
         foreach (var specialToken in specialTokens) {
            if (content[i..].StartsWith(specialToken.Key)) {
               int contentLength                    = i - messageStart - newLineLength;
               if (contentLength < 0) contentLength = 0;

               string messageContent = content[messageStart..(messageStart + contentLength)];
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

      var lastMessage = content[messageStart..];
      if (!string.IsNullOrWhiteSpace(lastMessage) && !systemMessageAdded) {
         messages.Add(new Message {role = currentRole, content = lastMessage});
      }

      if (messages.Any()) {
         var samplePrompt = messages[^1];
         if (samplePrompt.role == Role.user && string.IsNullOrWhiteSpace(tulpa_config.SamplePrompt)) {
            tulpa_config.SamplePrompt = samplePrompt.content;
         }
      }


      tulpa = new Tulpa {
         Configuration = tulpa_config,
         Messages      = messages,
         File          = file
      };
      return true;
   }


   public static bool TryParse_weg(string content, FileInfo file, [NotNullWhen(true)] out Tulpa? tulpa) {
      var contentMemory = content.AsMemory();
      var currentRole   = Role.system; // Default role.
      var messageStart  = 0;
      var messages      = new List<Message>();

      // Default configuration.
      var tulpa_config = new TulpaConfiguration();

      // Handle the Configuration part at the beginning.
      if (contentMemory.Span.StartsWith(SpecialTokens.Tulpa_Config_Token)) {
         int configEnd = -1;
         foreach (var specialToken in specialTokens) {
            configEnd = contentMemory.Span.IndexOf(specialToken.Key);
            if (configEnd != -1) break;
         }

         if (configEnd != -1) {
            var configContent = contentMemory.Slice(messageStart, configEnd - messageStart).ToString();
            if (TryParseConfiguration(configContent, out var config)) {
               tulpa_config = config;
            }
            else {
               tulpa = null;
               return false;
            }

            messageStart = configEnd;
         }
      }

      // Use the file name if Name property is empty.
      if (string.IsNullOrWhiteSpace(tulpa_config.Name)) {
         tulpa_config.Name = Path.GetFileNameWithoutExtension(file.Name);
      }


      int newLineLength = Environment.NewLine.Length;

      for (var i = messageStart; i < contentMemory.Length; i++) {
         foreach (var specialToken in specialTokens) {
            if (contentMemory.Slice(i).Span.StartsWith(specialToken.Key)) {
               //string messageContent = contentMemory.Slice(messageStart, i - messageStart - Environment.NewLine.Length).ToString();
               int contentLength = i - messageStart - newLineLength;

               // Ensure length is positive
               if (contentLength < 0) contentLength = 0;

               string messageContent = contentMemory.Slice(messageStart, contentLength).ToString();

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
         //We have this edge case, where the first line is one of the role tokens and we have to remove that from the message.
         //But the token will have a newline in front, so we first have to create a proper structure for that.
         //We get all Role tokens, remove the first newline and create a list from that.
         var roleTokens = specialTokens.Keys.Select(x => x[Environment.NewLine.Length..]).ToList();
         //Then we check if the lastMessage starts with one of the role tokens.
         var matchingToken = roleTokens.FirstOrDefault(lastMessage.StartsWith);
         if (matchingToken != null) {
            //If it does, we remove the token from the start of lastMessage.
            lastMessage = lastMessage[matchingToken.Length..];
         }

         messages.Add(new Message {role = currentRole, content = lastMessage});
      }


      tulpa = new Tulpa {
         Configuration = tulpa_config,
         Messages      = messages,
         File          = file
      };
      return true;
   }

   public static bool TryParseConfiguration(string lines, [NotNullWhen(true)] out TulpaConfiguration? config) {
      config = new TulpaConfiguration();
      var      properties  = typeof(TulpaConfiguration).GetProperties();
      string[] parsedLines = lines.Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);

      foreach (var line in parsedLines) {
         foreach (var property in properties) {
            var propertyName = property.Name + ":";
            if (line.StartsWith(propertyName)) {
               string valueString = line.Substring(propertyName.Length).Trim();
               if (property.PropertyType == typeof(float)) {
                  if (!float.TryParse(valueString, out float value)) {
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