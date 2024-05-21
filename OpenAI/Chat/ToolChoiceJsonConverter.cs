using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WinGPT.OpenAI.Chat;

public class ToolChoiceJsonConverter : JsonConverter<ToolChoice> {
   public override ToolChoice ReadJson(JsonReader reader, Type objectType, ToolChoice? existingValue, bool hasExistingValue, JsonSerializer serializer) {
      JObject jObject = JObject.Load(reader);

      return
         jObject[KEYS.type]!.Value<string>() == KEYS.function
            ? new ToolChoice(jObject[KEYS.function]?[KEYS.name]?.Value<string>()!)
            : new ToolChoice(Enum.Parse<ToolChoice_Mode>(jObject[KEYS.type]?.Value<string>()!, true));
   }

   public override void WriteJson(JsonWriter writer, ToolChoice? value, JsonSerializer serializer) {
      if (value?.Mode == null) {
         JObject jObject = new() {
            {KEYS.type, KEYS.function},
            {KEYS.function, new JObject {{KEYS.name, value.FunctionName}}}
         };
         jObject.WriteTo(writer);
      }
      else {
         // When it's not a function, write the mode directly as a string
         writer.WriteValue(value?.Mode.ToString());
      }
   }
}