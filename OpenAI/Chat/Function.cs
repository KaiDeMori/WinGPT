// ReSharper disable UnusedMember.Global

#pragma warning disable IDE1006
#pragma warning disable CS8618
namespace WinGPT.OpenAI.Chat;

public interface IFunction {
}

public class Function<T> : IFunction where T : Parameters {
   public string name        { get; init; }
   public string description { get; init; }
   public T      parameters  { get; init; }
}

public interface IFunctionCall {
   public string name { get; init; }
}

public class FunctionCall<T> : IFunctionCall where T : ICallArguments {
   public string name      { get; init; }
   public T      arguments { get; init; }
}

public interface ICallArguments {
}

public abstract class Parameters {
   public string       type     { get; init; }
   public List<string> required { get; init; }
}

public class TaxonomyParameters : Parameters {
   public Taxonomy_Properties properties { get; init; }
}

public class Taxonomy_Properties {
   public ParameterDetail summary  { get; init; }
   public ParameterDetail filename { get; init; }
   public ParameterDetail category { get; init; }
}

public class Taxonomy_CallArguments : ICallArguments {
   public string summary  { get; init; }
   public string filename { get; init; }
   public string category { get; init; }
}

public class ParameterDetail {
   public string type        { get; init; }
   public string description { get; init; }
}

public class Example {
   public Function<Parameters>[] functions { get; init; }
}

public class SaveParameters : Parameters {
   public Save_Properties properties { get; init; }
}

public class Save_Properties {
   public ParameterDetail filename     { get; init; }
   public ParameterDetail text_content { get; init; }
}

public class Save_CallArguments : ICallArguments {
   public string filename     { get; init; }
   public string text_content { get; init; }
}