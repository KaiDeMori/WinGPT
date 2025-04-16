using Newtonsoft.Json.Linq;

namespace WinGPT.OpenAI;

/// <summary>
/// We massively simplified this.
/// Now we read all available models from the API and populate the UI with all of them.
/// Since we get no additional information from the API endpoint, we can no longer determine
/// token window, vision capability, etc. automatically.
/// Seems like from now on, the user has to know what the model is capable of. ;-)
/// Additionally there is a favourite_models.txt file in the OpenAI folder now,
/// that can be used to reorder the models.
/// </summary>
public static class Models {
   public static readonly string nl = "\n";

   public static Model[] Available { get; set; } = [];

   private const           string Endpoint          = "models";
   private const           string json_filename     = "OpenAI/favourite_models.txt";
   private static readonly string Full_Endpoint_URL = HTTP_Client.Get_full_URL_for_endpoint(Endpoint);

   public static void initialize_available_models_for_api_key() {
      var http_client = HTTP_Client.Gimme();
      var response    = http_client.GetAsync(Full_Endpoint_URL).Result;

      if (!response.IsSuccessStatusCode) {
         Available = [];
         return;
      }

      var json = response.Content.ReadAsStringAsync().Result;
      var jObj = JObject.Parse(json);
      var data = jObj["data"];

      if (data is null) {
         Available = [];
         return;
      }

      var raw_models = data.ToObject<Model[]>();
      if (raw_models is null) {
         Available = [];
         return;
      }

      var model_list = raw_models.OrderBy(m => m.id).ToList();

      // If favourite_models.txt is present, reorder so its entries appear first
      if (File.Exists(json_filename)) {
         var favs = File.ReadAllLines(json_filename)
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .ToList();

         var sorted = new List<Model>();

         // Put favorites first, in order
         foreach (var fav in favs) {
            var found = model_list.FirstOrDefault(m => m.id == fav);
            if (found != null) {
               sorted.Add(found);
               model_list.Remove(found);
            }
         }

         // Add remaining models
         sorted.AddRange(model_list);

         Available = sorted.ToArray();
      }
      else {
         Available = model_list.ToArray();
      }
   }

   public static Model get_active_Model() {
      string current = Config.Active.Language_Model;
      return Available.First(m => m.id == current);
   }
}