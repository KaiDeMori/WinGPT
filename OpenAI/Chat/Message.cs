using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WinGPT.OpenAI.Chat;

public class Message {
   public Role               role    { get; set; }
   public List<content_part> content { get; set; } = new();

   public enum content_type {
      text,
      image_url
   }

   public abstract class content_part {
      public content_type type { get; set; }
   }

   public class text_content_part : content_part {
      public string text { get; set; }
   }

   public class image_content_part : content_part {
      public image_url image_url { get; set; }
   }

   public class image_url {
      public string url { get; set; }
   }


   /// <summary>
   /// **Not** the same thing as <see cref="Request.function_call"/>.
   /// </summary>
   [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
   public FunctionCall? function_call { get; init; } = null!;

   public override string ToString() {
      var specialToken = role.ToSpecialToken();
      var text         = specialToken + content + "\r\n";
      return text;
   }

   [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
   public string? name { get; init; }

   public class content_part_Converter : JsonConverter<content_part> {
      public override content_part ReadJson(JsonReader reader, Type objectType, content_part? existingValue, bool hasExistingValue, JsonSerializer serializer) {
         JObject item         = JObject.Load(reader);
         JToken  content_type = item["type"]!;
         return (content_type.Value<string>() switch {
            "text"      => item.ToObject<text_content_part>(serializer),
            "image_url" => item.ToObject<image_content_part>(serializer),
            _           => throw new NotImplementedException($"Not implemented for type: {content_type!.Value<string>()}")
         })!;
      }

      public override void WriteJson(JsonWriter writer, content_part? value, JsonSerializer serializer) {
         JObject jObject = new JObject();
         Type    type    = value!.GetType();

         if (type == typeof(text_content_part)) {
            jObject.Add("type", "text");
            jObject.Add("text", ((text_content_part) value).text);
         }
         else if (type == typeof(image_content_part)) {
            jObject.Add("type",      "image_url");
            jObject.Add("image_url", JObject.FromObject(((image_content_part) value).image_url));
         }

         jObject.WriteTo(writer);
      }
   }

   public Message Clone() {
      // Create a shallow copy of the current object
      Message clone = (Message) MemberwiseClone();

      // Manually create a deep copy of the List<content_part> property
      clone.content = new List<content_part>(content.Count);
      foreach (var contentPart in content) {
         switch (contentPart) {
            case text_content_part textPart:
               clone.content.Add(new text_content_part {type = textPart.type, text = textPart.text});
               break;
            case image_content_part imagePart:
               clone.content.Add(new image_content_part {type = imagePart.type, image_url = new image_url {url = imagePart.image_url.url}});
               break;
         }
      }

      return clone;
   }
}

public class FunctionCall {
   public string name      { get; init; } = null!;
   public string arguments { get; init; } = null!;
}