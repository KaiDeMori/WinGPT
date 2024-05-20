using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WinGPT.OpenAI.Chat;

public class ToolChoiceJsonConverter : JsonConverter<ToolChoice> {
   public override ToolChoice ReadJson(JsonReader reader, Type objectType, ToolChoice? existingValue, bool hasExistingValue, JsonSerializer serializer) {
      JObject jObject = JObject.Load(reader);

      return
         jObject["type"]!.Value<string>() == "function"
            ? new ToolChoice(jObject["function"]?["name"]?.Value<string>()!)
            : new ToolChoice(Enum.Parse<ToolChoice_Mode>(jObject["type"]?.Value<string>()!, true));
   }

   public override void WriteJson(JsonWriter writer, ToolChoice? value, JsonSerializer serializer) {
      JObject jObject = [];

      if (value?.Mode == ToolChoice_Mode.none) {
         jObject.Add("type",     "function");
         jObject.Add("function", new JObject {{"name", value.FunctionName}});
      }
      else {
         jObject.Add("type", value?.Mode.ToString());
      }

      jObject.WriteTo(writer);
   }
}