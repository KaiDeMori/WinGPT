using System.Text;
using WinGPT.OpenAI.Chat;

namespace WinGPT.Tokenizer;

public static class function_formatter {
   public static string format_function_definitions(IEnumerable<Function> functions) {
      var sb = new StringBuilder();
      sb.AppendLine("namespace functions {");
      sb.AppendLine();

      foreach (var function in functions) {
         if (!string.IsNullOrEmpty(function.description)) {
            sb.AppendLine($"// {function.description}");
         }

         var parameters = function.parameters;
         var properties = parameters.properties;

         if (!properties.Any()) {
            sb.AppendLine($"type {function.name} = () => any;");
         }
         else {
            sb.AppendLine($"type {function.name} = (_: " + "{");
            sb.AppendLine(FormatObjectProperties(parameters, 0));
            sb.AppendLine("}) => any;");
         }

         sb.AppendLine();
      }

      sb.AppendLine("} // namespace functions");
      return sb.ToString();
   }

   private static string FormatObjectProperties(Parameters parameters, int indent) {
      var sb             = new StringBuilder();
      var requiredParams = parameters.required ?? new List<string>();

      foreach (var (name, detail) in parameters.properties) {
         if (!string.IsNullOrEmpty(detail.description) && indent < 2) {
            sb.AppendLine($"// {detail.description}");
         }

         var typeName = FormatType(detail);
         if (requiredParams.Contains(name)) {
            sb.AppendLine($"{new string(' ', indent)}{name}: {typeName},");
         }
         else {
            sb.AppendLine($"{new string(' ', indent)}{name}?: {typeName},");
         }
      }

      return sb.ToString();
   }

   private static string FormatType(ParameterDetail detail) {
      if (detail.enumValues != null && detail.enumValues.Any()) {
         return string.Join(" | ", detail.enumValues.Select(v => $"\"{v}\""));
      }

      return detail.type switch {
         "string"  => "string",
         "number"  => "number",
         "boolean" => "boolean",
         "null"    => "null",
         "array"   => "any[]",
         "object"  => "{ }", // Simplified for now, may need to handle nested objects
         _         => "any"
      };
   }
}