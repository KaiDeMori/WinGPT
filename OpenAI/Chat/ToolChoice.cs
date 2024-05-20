using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace WinGPT.OpenAI.Chat;

/// <summary>
/// This can be either a string (from ToolChoice_Mode) or
/// a simplified function, with only the name.
/// </summary>
public class ToolChoice {
   public ToolChoice_Mode? Mode         { get; set; }
   public string?          FunctionName { get; set; }

   public ToolChoice(ToolChoice_Mode mode) {
      Mode = mode;
   }

   public ToolChoice(string functionName) {
      Mode         = ToolChoice_Mode.none;
      FunctionName = functionName;
   }
}

/// <summary>
/// Apparently, "required" means choose one of the tools if there are multiple.
/// To force a call to a specific tool:
/// "tool_choice": {
///     "type": "function",
///     "function": {
///         "name": "get_n_day_weather_forecast"
///     }
/// }
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum ToolChoice_Mode {
   none,
   auto,
   required
}