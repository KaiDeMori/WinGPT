using System.Collections.Immutable;
using Newtonsoft.Json;
using WinGPT.Taxonomy;

namespace WinGPT.OpenAI.Chat;

public class Request {
   public string                 model    { get; init; }
   public ImmutableList<Message> messages { get; init; }

   //[JsonProperty("functions", NullValueHandling = NullValueHandling.Ignore)]
   //public List<Function>? functions { get; init; }

   [JsonProperty("tools", NullValueHandling = NullValueHandling.Ignore)]
   public List<Tool> tools { get; } = [];

   /// <summary>
   /// "none", "auto" or your function name like this
   /// {"name":\ "my_function"}
   /// </summary>
   [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
   public FunctionCallSettings? function_call { get; init; }

   [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
   public ToolChoice? tool_choice { get; set; }

   public double temperature { get; init; }

   [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
   public int? max_tokens { get; init; } = Config.Active.UIable.Max_Tokens;
}