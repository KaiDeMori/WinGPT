using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace WinGPT.OpenAI.Chat;

public abstract class Message {
   [JsonProperty(Order = 1)]
   public Role role { get; set; }

   public object content { get; set; } = new();

   [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
   public string? name { get; init; }

   /// <summary>
   /// **Not** the same thing as <see cref="Request.function_call"/>.
   /// </summary>
   [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
   public FunctionCall? function_call { get; init; }

   // Make Clone method virtual to allow overriding in derived classes
   public virtual Message Clone() {
      var clone = (Message) MemberwiseClone();
      clone.content = this switch {
         Simple_Message simpleMessage   => new string(simpleMessage.content),
         Complex_Message complexMessage => complexMessage.content.Select(cp => cp.Clone()).ToList(),
         _                              => clone.content
      };

      return clone;
   }
}

public class Simple_Message : Message {
   [JsonProperty(Order = 2)]
   public new string content { get; set; }

   public override string ToString() {
      var specialToken = role.ToSpecialToken();
      var text         = specialToken + content + "\r\n";
      return text;
   }

   // Override Clone method to return Simple_Message
   public override Simple_Message Clone() {
      return new Simple_Message {
         role          = role,
         content       = content,
         name          = name,
         function_call = function_call
      };
   }
}

public class Complex_Message : Message {
   [JsonProperty(Order = 2)]
   public new List<content_part> content { get; set; } = [];

   public override string ToString() {
      var specialToken = role.ToSpecialToken();
      var text         = specialToken + string.Join("", content.OfType<text_content_part>()) + "\r\n";
      return text;
   }

   // Override Clone method to return Complex_Message
   public override Complex_Message Clone() {
      return new Complex_Message {
         role          = role,
         content       = content.Select(cp => cp.Clone()).ToList(),
         name          = name,
         function_call = function_call
      };
   }
}

public class FunctionCall {
   public string name      { get; init; } = null!;
   public string arguments { get; init; } = null!;
}

[JsonConverter(typeof(StringEnumConverter))]
public enum content_type {
   text,
   image_url
}

public abstract class content_part {
   public content_type type { get; set; }

   public abstract content_part Clone();
}

public class text_content_part : content_part {
   public new content_type type = content_type.text;

   public string text { get; set; }

   public override content_part Clone() {
      return new text_content_part {text = text, type = content_type.text};
   }

   public override string ToString() {
      return text;
   }
}

public class image_content_part : content_part {
   public new content_type type = content_type.image_url;

   public image_url image_url { get; set; }

   public override content_part Clone() {
      return new image_content_part {
         type      = content_type.image_url,
         image_url = image_url
      };
   }
}

public class image_url {
   public string url { get; set; }
}