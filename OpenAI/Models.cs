using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WinGPT.OpenAI;

public static class Models {
   public static readonly string nl = "\n";

   // Change Supported to a property with a backing field
   private static List<Model>? _supported;

   public static List<Model> Supported => _supported ??= read_supported_models();

   public const string dall_e_3 = "dall-e-3";

   private const           string Endpoint          = "models";
   private const           string json_filename     = "OpenAI/supported_models.json";
   private static readonly string Full_Endpoint_URL = HTTP_Client.Get_full_URL_for_endpoint(Endpoint);

   public static List<Model> read_supported_models() {
      var text             = File.ReadAllText(json_filename);
      var supported_models = JsonConvert.DeserializeObject<List<Model>>(text);
      if (supported_models == null)
         throw new Exception($"Failed to read supported models from file {json_filename}");

      return supported_models;
   }

   public static string[] get_available_models_for_api_key() {
      var http_client = HTTP_Client.Gimme();
      var response    = http_client.GetAsync(Full_Endpoint_URL).Result;

      string json;
      using (var content = response.Content) {
         json = content.ReadAsStringAsync().Result;
      }

      // Parse the JSON string into a JObject
      var jObject = JObject.Parse(json);

      // Extract the 'data' array
      var data = jObject["data"];

      // Check if 'data' is not null and is an array
      if (data is not JArray dataArray)
         return Array.Empty<string>();

      var models =
         dataArray.Select(item => item["id"]?.ToString()).OfType<string>().ToArray();

      return models;
   }

   public static Model? get_model_by_id(string id) {
      return Supported.FirstOrDefault(model => model.id == id);
   }
}