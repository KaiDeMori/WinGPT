using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace WinGPT.OpenAI.Chat;

public class Message {
   public Message() {
   }

   public Message(Role role, string content) {
      this.role    = role;
      this.content = content;
   }

   [JsonConverter(typeof(StringEnumConverter))]
   public Role role { get; init; }

   public string? content { get; init; } = null!;

   [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
   public string? name { get; init; } = null!;

   /// <summary>
   /// **Not** the same thing as <see cref="Request.function_call"/>.
   /// </summary>
   [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
   public FunctionCall? function_call { get; init; } = null!;


   public override string ToString() {
      var    specialToken = role.ToSpecialToken();
      string text         = specialToken + content + "\r\n";
      return text;
   }

   public Message Clone() {
      return new Message(role, content) {
         name          = name,
         function_call = function_call
      };
   }
}

public class FunctionCall {
   public string name      { get; init; } = null!;
   public string arguments { get; init; } = null!;
}