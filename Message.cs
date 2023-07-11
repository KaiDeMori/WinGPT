using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace WinGPT;

/// <summary>
/// This uses the OpenAI API endpoint wording.
/// </summary>
public class Message2 {
   [JsonConverter(typeof(StringEnumConverter))]
   public Role Role { get; init; }

   public string Content { get; init; } = null!;
}