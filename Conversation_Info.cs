using System.Text;

namespace WinGPT;

public class Conversation_Info {
   /// <summary>
   /// The name of the conversation.
   /// </summary>
   public string? Name { get;  set; }

   /// <summary>
   /// The relative path and filename of the last tulpa file that was used with this conversation.
   /// </summary>
   public string TulpaFile { get; set; } = string.Empty;

   //public float Temperature { get; set; } = 0f;

   public static Conversation_Info ParseConfig(string[] lines) {
      // Create a dictionary to map config keys to their corresponding properties
      var configMap = typeof(Conversation_Info).GetProperties()
         .ToDictionary(property => $"{property.Name}:");

      var info = new Conversation_Info();
      foreach (var line in lines) {
         foreach (var key in configMap.Keys) {
            if (!line.StartsWith(key, StringComparison.InvariantCultureIgnoreCase))
               continue;

            var value    = line[key.Length..].Trim();
            var property = configMap[key];
            if (property.PropertyType == typeof(float)) {
               // If the property is a float, parse the value as a float
               if (float.TryParse(value, out var tempValue))
                  property.SetValue(info, tempValue);
               else
                  property.SetValue(info, 0f);
            }
            else {
               // Otherwise, set the value directly
               property.SetValue(info, value);
            }

            break;
         }
      }

      return info;
   }

   public StringBuilder Create_history_file_content() {
      var infoProperties = typeof(Conversation_Info).GetProperties();
      var info           = new StringBuilder();

      foreach (var property in infoProperties) {
         var value = property.GetValue(this);
         info.AppendLine($"{property.Name}: {value}");
      }

      return info;
   }
}