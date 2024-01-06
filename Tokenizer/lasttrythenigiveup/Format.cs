using WinGPT.OpenAI.Chat;

namespace WinGPT.Tokenizer.lasttrythenigiveup;

// Class containing methods to format function definitions
public static class Format {
   // Format the function definitions to a string
   public static string FormatFunctionDefinitions(List<Function> functions) {
      var lines = new List<string> {"namespace functions {", ""};

      foreach (var f in functions) {
         if (!string.IsNullOrEmpty(f.description)) {
            lines.Add("// " + f.description);
         }

         Parameters?                          parameters = f.parameters;
         Dictionary<string, ParameterDetail>? properties = parameters != null ? parameters.properties : null;

         if (properties == null || properties.Count == 0) {
            lines.Add("type " + f.name + " = () => any;");
         }
         else {
            lines.Add("type " + f.name + " = (_: " + "{");
            lines.Add(FormatObjectProperties(parameters, 0));
            lines.Add("}) => any;");
         }

         lines.Add("");
      }

      lines.Add("} // namespace functions");
      return string.Join("\n", lines);
   }

   // Format the object properties to a string
   public static string FormatObjectProperties(ObjectProp obj, int indent) {
      if (obj.properties == null) {
         return "";
      }

      var lines           = new List<string>();
      var required_params = obj.required ?? new List<string>();

      foreach (var entry in obj.properties) {
         var name       = entry.Key;
         var param      = entry.Value;
         var typeString = FormatType(param, indent);

         if (required_params.Contains(name)) {
            lines.Add(name + ": " + typeString + ",");
         }
         else {
            lines.Add(name + "?: " + typeString + ",");
         }
      }

      return string.Join("\n", lines.Select(line => new string(' ', indent) + line));
   }

   // Format the type to a string
   public static string FormatType(PropItem param, int indent) {
      switch (param) {
         case StringProp stringProp:
            return stringProp.@enum != null
               ? string.Join(" | ", stringProp.@enum.Select(v => "\"" + v + "\""))
               : "string";
         case NumberProp numberProp:
            return numberProp.@enum != null
               ? string.Join(" | ", numberProp.@enum.Select(v => v.ToString()))
               : "number";
         case BoolProp:
            return "boolean";
         case NullProp:
            return "null";
         case ArrayProp arrayProp:
            return arrayProp.items != null
               ? FormatType(arrayProp.items, indent) + "[]"
               : "any[]";
         case ObjectProp objectProp:
            return "{\n" + FormatObjectProperties(objectProp, indent + 2) + "\n}";
         default:
            return "any";
      }
   }
}

// Placeholder classes for OpenAIFunction, ObjectProp, PropItem, and their subclasses
// These should be implemented according to the structure expected by the formatting methods
//public class OpenAIFunction
//{
//   public string     name        { get; set; }
//   public string     description { get; set; }
//   public ObjectProp parameters  { get; set; }
//}

public class ObjectProp {
   public List<string>                 required   { get; set; }
   public Dictionary<string, PropItem> properties { get; set; }
}

public class PropItem {
   // Base class for property items
}

public class StringProp : PropItem {
   public List<string> @enum { get; set; }
}

public class NumberProp : PropItem {
   public List<double> @enum { get; set; }
}

public class BoolProp : PropItem {
   // Boolean property
}

public class NullProp : PropItem {
   // Null property
}

public class ArrayProp : PropItem {
   public PropItem items { get; set; }
}