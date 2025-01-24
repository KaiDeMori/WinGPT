using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace WinGPT.OpenAI.Chat;

[JsonConverter(typeof(StringEnumConverter))]
public enum reasoning_effort {
   medium, //default
   low,
   high
}