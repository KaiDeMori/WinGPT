using Newtonsoft.Json;
using WinGPT.OpenAI.Chat;

namespace WinGPT.OpenAI;

public class FunctionCallNameConverter : JsonConverter {
   public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
      if (value is Function_Call_Name functionCallName) {
         serializer.Serialize(writer, functionCallName.name);
      }
   }

   public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
      var name = serializer.Deserialize<string>(reader);
      return new Function_Call_Name(name);
   }

   public override bool CanConvert(Type objectType) {
      return objectType == typeof(Function_Call_Name);
   }
}