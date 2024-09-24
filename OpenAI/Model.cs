namespace WinGPT.OpenAI;

public class Model {
   //from API
   public string id       { get; set; }
   public string owned_by { get; set; }

   //custom
   public int  context_window { get; set; }
   public bool is_alias       { get; set; }
   public bool is_preview     { get; set; } = false;

   public string friendly_name => $"{id} ({context_window:N0}) {(is_alias ? "*" : "")} {(is_preview ? "P" : "")}";
}