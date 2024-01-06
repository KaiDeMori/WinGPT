using Newtonsoft.Json;
using WinGPT.Tokenizer.lasttrythenigiveup;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
namespace WinGPT.OpenAI.Chat;

// Generic class representing a function with typed parameters.
public class Function {
   public string     name        { get; init; }
   public string     description { get; init; }
   public Parameters parameters  { get; init; }
}

// Abstract base class for function parameters, containing common properties.
public class Parameters {
   public string                              type       { get; init; }
   public List<string>                        required   { get; init; }
   public Dictionary<string, ParameterDetail> properties { get; init; }
}

public interface ICallArguments {
}

// Class representing the details of a parameter.
public class ParameterDetail {
   public string        type        { get; init; }
   public string        description { get; init; }

   [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
   public List<string>? enumValues  { get; init; } // Optional field for enum values
}

// Class representing the call arguments for a save function.
public class Save_CallArguments : ICallArguments {
   public string filename     { get; init; }
   public string text_content { get; init; }
}