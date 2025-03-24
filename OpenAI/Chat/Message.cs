using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace WinGPT.OpenAI.Chat;

public abstract class Message {
   [JsonProperty(Order = 1)]
   public Role role { get; set; } = Role.user;

   public object content { get; set; } = new();

   [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
   public string? name { get; init; }

   /// <summary>
   /// **Not** the same thing as <see cref="Request.function_call"/>.
   /// </summary>
   [Obsolete("It's all tools now.")]
   [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
   public FunctionCall? function_call { get; init; }

   /// <summary>
   /// Controls which (if any) tool is called by the model.
   /// Tx only.
   /// </summary>
   public Tool_Choices? tool_choice { get; set; } = null!;

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

   public abstract string? ToString_Content_Only();


   public static List<Message> remove_image_parts(List<Message> messages) {
      var new_messages = new List<Message>();

      foreach (var message in messages) {
         if (message is Complex_Message complex_message) {
            var new_content = complex_message.content
               .Where(cp => cp.type != content_type.image_url)
               .ToList();

            var new_complex_message = new Complex_Message {
               role          = complex_message.role,
               content       = new_content,
               name          = complex_message.name,
               function_call = complex_message.function_call
            };

            new_messages.Add(new_complex_message);
         }
         else {
            new_messages.Add(message);
         }
      }

      return new_messages;
   }
}

public class Simple_Message : Message {
   [JsonProperty(Order = 2)]
   public new string? content { get; set; }

   [JsonProperty(Order = 3, NullValueHandling = NullValueHandling.Ignore)]
   public Tool_Call[]? tool_calls { get; init; }

   public override string ToString() {
      var specialToken = role.ToSpecialToken();
      var text         = specialToken + content + "\r\n";
      return text;
   }

   public override string? ToString_Content_Only() => content;

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

   public override string ToString_Content_Only() => string.Join("", content.OfType<text_content_part>());

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
   image_url,
   file
}

public abstract class content_part {
   public abstract content_type type { get; }

   public abstract content_part Clone();
}

public class text_content_part : content_part {
   public override content_type type => content_type.text;

   public string text { get; set; }

   public override content_part Clone() {
      return new text_content_part {text = text};
   }

   public override string ToString() {
      return text;
   }
}

public class image_content_part : content_part {
   public override content_type type => content_type.image_url;

   public image_url image_url { get; set; }

   public override content_part Clone() {
      return new image_content_part {
         image_url = image_url
      };
   }
}

public class document_content_part : content_part {
   public override content_type type => content_type.file;

   public file_content file { get; set; }

   public override content_part Clone() {
      return new document_content_part {
         file = file
      };
   }
}

public class image_url {
   public string url { get; set; }
}

public class file_content {
   public string filename { get; set; }

   /// <summary>
   /// Has to be a base64 encoded file url.
   /// </summary>
   public string file_data { get; set; }
}

public enum Tool_Choices {
   none,
   auto,
   required
}