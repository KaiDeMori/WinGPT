using Newtonsoft.Json;
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
   private static Model[]     All = [];
   private static List<Model> Regular    { get; set; } = [];
   private static List<Model> Favourites { get; set; } = [];

   private const           string Endpoint            = "models";
   private const           string favourites_filename = "OpenAI/favourite_models.txt";
   private static readonly string Full_Endpoint_URL   = HTTP_Client.Get_full_URL_for_endpoint(Endpoint);


   public static void initialize_models_menu(ToolStripMenuItem all_models_root_menu) {
      read_all_models_for_api_key();
      initialize_favourites();

      all_models_root_menu.DropDownItems.Clear();
      all_models_root_menu.ToolTipText = "Model ID (\ud83d\udc41 = vision)";

      foreach (var model in Favourites)
         create_menu_items(all_models_root_menu, model);
      all_models_root_menu.DropDownItems.Add(new ToolStripSeparator());
      foreach (var model in Regular)
         create_menu_items(all_models_root_menu, model);

      var current_model = get_active_Model();

      //now we have to set the checked property of the correct menu item
      foreach (ToolStripMenuItem item in all_models_root_menu.DropDownItems.OfType<ToolStripMenuItem>())
         if (item.Tag == current_model)
            item.Checked = true;

      //and set the text of the main menu item
      all_models_root_menu.Text = current_model.friendly_name;
   }

   private static void create_menu_items(ToolStripMenuItem all_models_root_menu, Model model) {
      var model_label = model.friendly_name;
      var item        = new ToolStripMenuItem(model_label);
      item.Tag = model;
      item.Click += (sender, args) => {
         Config.Active.Language_Model = model.id;
         all_models_root_menu.Text    = model_label;
         Config.Save();
         foreach (ToolStripMenuItem oneitem in all_models_root_menu.DropDownItems.OfType<ToolStripMenuItem>())
            oneitem.Checked = oneitem == item;
         //realculate_all_token_counts();
      };
      item.MouseUp += (sender, args) => {
         if (args.Button == MouseButtons.Right)
            update_favourites(item, all_models_root_menu);
      };
      all_models_root_menu.DropDownItems.Add(item);
   }

   private static void read_all_models_for_api_key() {
      var http_client = HTTP_Client.Gimme();
      var response    = http_client.GetAsync(Full_Endpoint_URL).Result;

      if (!response.IsSuccessStatusCode)
         return;

      var json = response.Content.ReadAsStringAsync().Result;

      var r = JsonConvert.DeserializeObject<ModelListResponse>(json);
      All = r?.data.OrderBy(m => m.id).ToArray() ?? [];
   }

   public static Model get_active_Model() {
      string current = Config.Active.Language_Model;
      return All.First(m => m.id == current);
   }

   private static void initialize_favourites() {
      if (!File.Exists(favourites_filename))
         File.Create(favourites_filename).Close();

      var favourite_model_ids = File.ReadAllLines(favourites_filename)
         .Where(line => !string.IsNullOrWhiteSpace(line))
         .OrderBy(line => line)
         .ToList();

      var unkown_models = favourite_model_ids.Except(All.Select(m => m.id)).ToList();
      if (unkown_models.Count > 0)
         MessageBox.Show("The following models are not available in the current API key:\n" +
                         string.Join("\n", unkown_models), "Unknown Models", MessageBoxButtons.OK, MessageBoxIcon.Warning);

      favourite_model_ids.RemoveAll(unkown_models.Contains);

      // Always create sorted lists
      Favourites = All.Where(m => favourite_model_ids.Contains(m.id))
         .OrderBy(m => m.id)
         .ToList();
      Regular = All.Where(m => !favourite_model_ids.Contains(m.id))
         .OrderBy(m => m.id)
         .ToList();
   }

   public static void update_favourites(ToolStripMenuItem selected_item, ToolStripMenuItem all_models_root_menu) {
      //first we have to make sure, that there is exactly one separator in the menu.
      //if there is none, we don't have any favourites, yet (and obviously want to add the first one now).
      //if there are more than one, we throw an exception and explain what the situation is :-)
      var separators = all_models_root_menu.DropDownItems
         .OfType<ToolStripSeparator>()
         .ToList();

      if (separators.Count > 1)
         throw new InvalidOperationException(
            $"There are {separators.Count} separators in the models menu. " +
            "There should be exactly one. Please check the menu structure."
         );

      if (separators.Count == 0)
         // No separator yet, so we can add one now (when adding the first favourite)
         all_models_root_menu.DropDownItems.Add(new ToolStripSeparator());

      // Remove the selected item from the menu before we re-insert it
      all_models_root_menu.DropDownItems.Remove(selected_item);

      // Add or remove model from favourites
      var model = (Model) selected_item.Tag;

      if (Favourites.Contains(model)) {
         // Move from Favourites to Regular
         Favourites.Remove(model);
         Regular.Add(model);
      }
      else {
         // Move from Regular to Favourites
         Regular.Remove(model);
         Favourites.Add(model);
      }

      // Sort both lists
      Favourites = Favourites.OrderBy(m => m.id).ToList();
      Regular    = Regular.OrderBy(m => m.id).ToList();

      // Now re-insert the item in the correct position
      var separator_index = all_models_root_menu.DropDownItems
         .OfType<ToolStripSeparator>()
         .Select(x => all_models_root_menu.DropDownItems.IndexOf(x))
         .SingleOrDefault();

      if (Favourites.Contains(model)) {
         // Insert in the favourites section (before separator)
         for (int i = 0; i < separator_index; i++) {
            if (all_models_root_menu.DropDownItems[i] is ToolStripMenuItem t) {
               var compare_model = t.Tag as Model;
               if (string.CompareOrdinal(model.id, compare_model?.id) < 0) {
                  all_models_root_menu.DropDownItems.Insert(i, selected_item);
                  return;
               }
            }
         }

         // If we didn't insert before, insert at separator_index
         all_models_root_menu.DropDownItems.Insert(separator_index, selected_item);
      }
      else {
         // Insert in the regular section (after separator)
         for (int i = separator_index + 1; i < all_models_root_menu.DropDownItems.Count; i++) {
            if (all_models_root_menu.DropDownItems[i] is ToolStripMenuItem t) {
               var compare_model = t.Tag as Model;
               if (string.CompareOrdinal(model.id, compare_model?.id) < 0) {
                  all_models_root_menu.DropDownItems.Insert(i, selected_item);
                  return;
               }
            }
         }

         // If we didn't insert before, add at the end
         all_models_root_menu.DropDownItems.Add(selected_item);
      }

      save_favourites_file();
   }

   public static void save_available_models_info() {
      var message     = All.Order().Aggregate("Available models:\r\n", (current, model) => current + $"{model}\r\n");
      var models_file = Path.Join(Config.AdHoc_Downloads_Path.FullName, Config.models_text_filename);
      File.WriteAllText(models_file, message);
      MessageBox.Show($"Available models written to\r\n{models_file}", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
   }

   public static void save_favourites_file() {
      var favourite_model_ids = Favourites.Select(m => m.id).ToList();
      File.WriteAllLines(favourites_filename, favourite_model_ids);
   }
}

public class ModelListResponse {
   public required string      @object { get; set; }
   public required List<Model> data    { get; set; }
}