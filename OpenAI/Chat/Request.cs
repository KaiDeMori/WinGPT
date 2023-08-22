using System.Collections.Immutable;
using Newtonsoft.Json;

namespace WinGPT.OpenAI.Chat;

public class Request {
   public string                 model    { get; init; }
   public ImmutableList<Message> messages { get; init; }

   [JsonProperty("functions", NullValueHandling = NullValueHandling.Ignore)]
   public IFunction[]? functions { get; init; }

   /// <summary>
   /// "none", "auto" or your function name like this
   /// {"name":\ "my_function"}
   /// </summary>
   [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
   public FunctionCallSettings? function_call { get; init; }

   public double temperature { get; init; }

   public int? max_tokens { get; init; } = Config.Active.UIable.Max_Tokens;
}

public class FunctionCallSettings {
   public Function_Call_Mode? mode { get; private set; }
   public Function_Call_Name? name { get; private set; }

   public FunctionCallSettings(Function_Call_Mode mode) {
      this.mode = mode;
      name      = null;
   }

   public FunctionCallSettings(Function_Call_Name name) {
      this.name = name;
      mode      = null;
   }

   public object? ToObject() {
      return mode.HasValue ? mode.ToString() : (object?) name;
   }

   public static implicit operator FunctionCallSettings(Function_Call_Mode mode) {
      return new FunctionCallSettings(mode);
   }

   public static implicit operator FunctionCallSettings(Function_Call_Name name) {
      return new FunctionCallSettings(name);
   }
}

public enum Function_Call_Mode {
   none,
   auto
}

[JsonConverter(typeof(FunctionCallNameConverter))]
public class Function_Call_Name {
   public Function_Call_Name(string name) {
      this.name = name;
   }

   public string name { get; init; }

   public static implicit operator Function_Call_Name(string name) {
      return new Function_Call_Name(name);
   }
}