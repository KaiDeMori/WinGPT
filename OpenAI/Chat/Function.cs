#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
namespace WinGPT.OpenAI.Chat;

// Interface for a function, providing the name, description, and parameters.
public interface IFunction {
   string     name        { get; }
   string     description { get; }
   Parameters parameters  { get; }
}

// Generic class representing a function with typed parameters.
public class Function {
   public string name        { get; init; }
   public string description { get; init; }

   public Parameters parameters { get; init; }
   //Parameters IFunction.parameters  => parameters;
}

// Abstract base class for function parameters, containing common properties.
public class Parameters {
   public string                              type       { get; init; }
   public List<string>                        required   { get; init; }
   public Dictionary<string, ParameterDetail> properties { get; init; }
}

// Class representing the parameters for a save function.
public class SaveParameters : Parameters {
   //public new SaveProperties properties { get; init; }
}

// Class containing the properties specific to save parameters.
public class SaveProperties {
   // Filename to save the content to
   public ParameterDetail filename { get; init; }

   // Text content to be saved
   public ParameterDetail text_content { get; init; }
}

// Interface for a function call, providing the name of the function.
public interface IFunctionCall {
   string name { get; init; }
}

// Generic class representing a function call with typed arguments.
public class FunctionCall<TArguments> : IFunctionCall where TArguments : ICallArguments {
   public string     name      { get; init; }
   public TArguments arguments { get; init; }
}

// Interface for call arguments.
public interface ICallArguments {
}

// Class representing the parameters for a taxonomy function.
public class TaxonomyParameters : Parameters {
   //public new TaxonomyProperties properties { get; init; }
}

public class Taxonomy_CallArguments : ICallArguments {
   // Summary of the taxonomy
   public string summary { get; init; }

   // Filename associated with the taxonomy
   public string filename { get; init; }

   // Category of the taxonomy
   public string category { get; init; }
}

// Class representing the details of a parameter.
public class ParameterDetail {
   public string        type        { get; init; }
   public string        description { get; init; }
   public List<string>? enumValues  { get; init; } // Optional field for enum values
}

// Class representing the call arguments for a save function.
public class Save_CallArguments : ICallArguments {
   public string filename     { get; init; }
   public string text_content { get; init; }
}