using System.Collections.Immutable;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using WinGPT.Taxonomy;

namespace WinGPT.OpenAI.Chat;

public class Request {
   public string                 model    { get; init; }
   public ImmutableList<Message> messages { get; init; }

   //[JsonProperty("functions", NullValueHandling = NullValueHandling.Ignore)]
   //public List<Function>? functions { get; init; }

   [JsonProperty("tools",
      NullValueHandling = NullValueHandling.Ignore,
      DefaultValueHandling = DefaultValueHandling.Ignore)]
   public List<Tool> tools { get; } = [];

   /// <summary>
   /// Seriously dark newtonsoft magic.
   /// Don't try to understand, just believe.
   /// </summary>
   /// <returns></returns>
   public bool ShouldSerializetools() {
      return tools.Count > 0;
   }

   /// <summary>
   /// "none", "auto" or your function name like this
   /// {"name":\ "my_function"}
   /// </summary>
   [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
   public FunctionCallSettings? function_call { get; init; }

   [JsonProperty(Order = 4, NullValueHandling = NullValueHandling.Ignore)]
   public ToolChoice? tool_choice { get; set; }

   [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
   public double? temperature { get; set; } = null;

   [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
   public int? max_tokens { get; init; } = Config.Active.UIable.Max_Tokens;

   [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
   public reasoning_effort? reasoning_effort { get; set; } = null;

   public Service_Tier service_tier { get; init; } = Service_Tier.auto;
}

[JsonConverter(typeof(StringEnumConverter))]
public enum Service_Tier {
   auto,
   @default,
   flex
}