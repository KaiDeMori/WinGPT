using System.Collections.Specialized;
using System.Reflection;
using System.Text;

namespace WinGPT;

public class Conversation_Info {
   /// <summary>
   /// A summary of the conversation.
   /// </summary>
   public string? Summary { get; set; }

   /// <summary>
   /// The relative path and filename of the last tulpa file that was used with this conversation.
   /// </summary>
   public string TulpaFile { get; set; } = string.Empty;

   /// <summary>
   /// This might become really important for categorizing conversations.
   /// And maybe for AutoSort ;-)
   /// </summary>
   public DateTimeOffset Started { get; set; } = DateTimeOffset.Now;

   //public float Temperature { get; set; } = 0f;

   public static Conversation_Info ParseConfig(string[] lines) {
      // Create a dictionary to map config keys to their corresponding properties
      var configMap = typeof(Conversation_Info).GetProperties()
         .ToDictionary(property => $"{property.Name}:");

      var info = new Conversation_Info();

      PropertyInfo last_property = null;
      foreach (var line in lines) {
         var configKey = configMap.Keys.FirstOrDefault(key => line.StartsWith(key, StringComparison.InvariantCultureIgnoreCase));
         if (configKey == null) {
            if (last_property.Name == nameof(Conversation_Info.Summary)) {
               string value = last_property.GetValue(info) as string;
               value = value + "\r\n" + line;
               last_property.SetValue(info, value);
            }

            continue;
         }

         var valueString = line[configKey.Length..].Trim();
         last_property = configMap[configKey];

         var parsedValue = ParseValue(last_property.PropertyType, valueString);
         last_property.SetValue(info, parsedValue);
      }

      return info;
   }

   private static object ParseValue(Type propertyType, string valueString) =>
      propertyType switch {
         _ when propertyType == typeof(float) =>
            float.TryParse(valueString, out var floatValue) ? floatValue : 0f,
         _ when propertyType == typeof(DateTimeOffset) =>
            DateTimeOffset.TryParse(valueString, out var dateValue) ? dateValue : DateTimeOffset.MinValue,
         _ => valueString,
      };


   public StringBuilder Create_history_file_content() {
      var infoProperties = typeof(Conversation_Info).GetProperties();
      var info           = new StringBuilder();

      foreach (var property in infoProperties) {
         var    value = property.GetValue(this);
         string valueString;

         if (value is DateTimeOffset dateTimeOffset) {
            valueString = dateTimeOffset.ToString("o");
         }
         else {
            valueString = value?.ToString() ?? "null";
         }

         info.AppendLine($"{property.Name}: {valueString}");
      }

      return info;
   }
}