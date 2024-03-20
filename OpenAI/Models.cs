using AutoUpdaterDotNET;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WinGPT.OpenAI.Chat;

namespace WinGPT.OpenAI;

public static class Models {
   public static readonly string nl = "\n";

   public static List<Model> Supported = read_supported_models();

   //public static readonly string[] Supported = {
   //   gpt_3_5_turbo,
   //   gpt_3_5_turbo_0301,
   //   gpt_3_5_turbo_0613,
   //   gpt_3_5_turbo_1106,
   //   gpt_3_5_turbo_16k,
   //   gpt_3_5_turbo_16k_0613,
   //   gpt_3_5_turbo_instruct,
   //   gpt_3_5_turbo_instruct_0914,
   //   gpt_4,
   //   gpt_4_0125_preview,
   //   gpt_4_0613,
   //   gpt_4_1106_preview,
   //   gpt_4_turbo_preview,
   //   gpt_4_vision_preview,
   //};

   //We need to create const properties for every entry in the Supported
   public const string gpt_3_5_turbo               = "gpt-3.5-turbo";
   public const string gpt_3_5_turbo_0301          = "gpt-3.5-turbo-0301";
   public const string gpt_3_5_turbo_0613          = "gpt-3.5-turbo-0613";
   public const string gpt_3_5_turbo_1106          = "gpt-3.5-turbo-1106";
   public const string gpt_3_5_turbo_16k           = "gpt-3.5-turbo-16k";
   public const string gpt_3_5_turbo_16k_0613      = "gpt-3.5-turbo-16k-0613";
   public const string gpt_3_5_turbo_instruct      = "gpt-3.5-turbo-instruct";
   public const string gpt_3_5_turbo_instruct_0914 = "gpt-3.5-turbo-instruct-0914";
   public const string gpt_4                       = "gpt-4";
   public const string gpt_4_0125_preview          = "gpt-4-0125-preview";
   public const string gpt_4_0613                  = "gpt-4-0613";
   public const string gpt_4_1106_preview          = "gpt-4-1106-preview";
   public const string gpt_4_turbo_preview         = "gpt-4-turbo-preview";
   public const string gpt_4_vision_preview        = "gpt-4-vision-preview";

   ////and dall-e 3
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
      if (data is not JArray dataArray) {
         return null;
      }

      var models =
         dataArray.Select(item => item["id"]?.ToString()).OfType<string>().ToArray();

      return models;
   }
}