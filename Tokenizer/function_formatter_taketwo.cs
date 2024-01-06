using System.Text;
using WinGPT.OpenAI.Chat;
using String = System.String;

namespace WinGPT.Tokenizer;

public static class function_formatter_taketwo {
   public static string format_function_definitions(IEnumerable<Function> functions) {
      var sb = new StringBuilder();

      foreach (var function in functions) {
         sb.AppendLine($"- {function.name}: {function.description}");
         // Call format_object_properties to format the parameters of the function
         sb.Append(format_object_properties(function.parameters, 1));
      }

      return sb.ToString();
   }

   private static string format_object_properties(Parameters parameters, int indent) {
      var sb = new StringBuilder();

      foreach (var property in parameters.properties) {
         var key    = property.Key;
         var detail = property.Value;
         sb.Append(' ', indent * 2); // Indentation
         sb.AppendLine($"{key} ({format_type(detail)}): {detail.description}");
         if (detail.enumValues != null) {
            foreach (var enumValue in detail.enumValues) {
               sb.Append(' ', (indent + 1) * 2); // Indentation for enum values
               sb.AppendLine($"- {enumValue}");
            }
         }
      }

      return sb.ToString();
   }

   private static string format_type(ParameterDetail detail) {
      if (detail.enumValues != null && detail.enumValues.Any()) {
         return "enum";
      }
      else {
         return detail.type;
      }
   }
}