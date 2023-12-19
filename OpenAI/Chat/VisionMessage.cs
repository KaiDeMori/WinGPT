using Newtonsoft.Json;

namespace WinGPT.OpenAI.Chat;

// Base class for message content
public abstract class VisionMessageContent {
   [JsonProperty("type")]
   public abstract string Type { get; }
}

// Text content derived from the base class
public class TextContent : VisionMessageContent {
   public override string Type => "text";

   [JsonProperty("text")]
   public string Text { get; set; } = null!;
}

// Image content derived from the base class
public class ImageContent : VisionMessageContent {
   public override string Type => "image_url";

   [JsonProperty("image_url")]
   public ImageUrl ImageUrl { get; set; } = null!;
}

// Image URL class
public class ImageUrl {
   [JsonProperty("url")]
   public string Url { get; set; } = null!;
}

// VisionMessage class inheriting from Message and using the new MessageContent types
public class VisionMessage : Message {
   // Changed the type of content to List<MessageContent>
   public new List<VisionMessageContent>? content { get; init; } = null!;

   public VisionMessage() {
   }

   public VisionMessage(Role role, List<VisionMessageContent> content) : base(role, content.ToString()) {
      this.content = content;
   }

   public override string ToString() {
      var    specialToken = role.ToSpecialToken();
      string text         = specialToken + JsonConvert.SerializeObject(content) + "\r\n";
      return text;
   }

   public new VisionMessage Clone() {
      return new VisionMessage(role, content) {
         name          = name,
         function_call = function_call
      };
   }
}