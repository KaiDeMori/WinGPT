namespace WinGPT.OpenAI;

public class Model {
   //from API
   public required string id       { get; set; }
   public required string @object  { get; set; } // new property to match JSON's "object"
   public          int    created  { get; set; } // new property to match JSON's "created"
   public required string owned_by { get; set; }

   //Ours
   public string friendly_name => id;

   /// <summary>
   /// Determines if the model is a vision/multimodal model.
   /// </summary>
   /// <returns>True if the model supports vision/multimodal input, otherwise false.</returns>
   public bool is_vision_model =>
      id.Contains("vision")            ||
      id.StartsWith("gpt-4o")          ||
      id.StartsWith("o1")              ||
      id.StartsWith("o3-mini")         ||
      id.StartsWith("gpt-4-turbo")     ||
      id.StartsWith("gpt-4.5-preview") ||
      id.StartsWith("gpt-4.1");
}