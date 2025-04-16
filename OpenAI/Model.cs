namespace WinGPT.OpenAI;

public class Model {
   //from API
   public required string id       { get; set; }
   public required string @object  { get; set; } // new property to match JSON's "object"
   public          int    created  { get; set; } // new property to match JSON's "created"
   public required string owned_by { get; set; }

   public string friendly_name => id;
}