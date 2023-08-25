#pragma warning disable CS8618

namespace WinGPT.OpenAI;

internal class ErrorCode {
   public int      Code     { get; init; }
   public string   Name     { get; init; }
   public Overview Overview { get; init; }
   public Detail   Detail   { get; init; }
}

internal class Detail {
   public string   Description  { get; init; }
   public string[] Reasons      { get; init; }
   public string[] ResolveSteps { get; init; }
}

internal class Overview {
   public string Cause    { get; init; }
   public string Solution { get; init; }
}