namespace WinGPT.OpenAI.Chat;

public class Function {
   public string     name        { get; init; }
   public string?    description { get; init; }
   public Parameters parameters  { get; init; }
}

public class Parameters {
   public string       type       { get; init; }
   public Properties   properties { get; init; }
   public List<string> required   { get; init; }
}

public class Properties {
   public ParameterDetail summary  { get; init; }
   public ParameterDetail filename { get; init; }
   public ParameterDetail category { get; init; }
}

public class ParameterDetail {
   public string type        { get; init; }
   public string description { get; init; }
}