using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;

namespace WinGPT;

public class Config {
   public static Config Active      = new();
   public static Tulpa  ActiveTulpa = new();

   //public static Conversation? ActiveConversation { get; set; }

   public bool UseSysMsgHack { get; set; }

   public Config_UIable UIable { get; set; } = new();

   private const   string        tulpas_directory = "Tulpas";
   internal static DirectoryInfo Tulpa_Directory => new(Path.Join(Active.BaseDirectory, tulpas_directory));

   private const   string        history_directory = "Conversation_History";
   internal static DirectoryInfo History_Directory => new(Path.Join(Active.BaseDirectory, history_directory));

   internal const string marf278down_extenstion = ".md";
   internal const string marf278down_filter     = "*.md";

   internal static readonly string prism_css;
   internal static readonly string my_css;
   internal static readonly string prism_js;

   public const  string        models_text_filename                = "models.txt";
   private const string        WebstuffsPrismFancyCss              = "webstuffs/prism_fancy.css";
   private const string        WebstuffsPrismFancyJs               = "webstuffs/prism_fancy.js";
   private const string        WebstuffsMyCss                      = "webstuffs/my.css";
   private const string        Preliminary_Conversations_Directory = "tmp";
   private const string        AdHoc_Downloads_Directory           = "Downloads";
   public const  string        DefaultAssistant_Filename           = "Default_Assistant.md";
   public static DirectoryInfo Preliminary_Conversations_Path => new(Path.Join(Active.BaseDirectory, Preliminary_Conversations_Directory));
   public static DirectoryInfo AdHoc_Downloads_Path           => new(Path.Join(Active.BaseDirectory, AdHoc_Downloads_Directory));

   public const string WIKI_URL = "https://wiki.peopleoftheprompt.org";

   private static readonly object _lock   = new();
   public static           bool   loading = false;

   /// <summary>
   /// This is the internal bookkeeping of the number of tokens used in total since the last reset.
   /// </summary>
   public TokenCounter TokenCounter { get; set; } = new();

   public string? BaseDirectory  { get; set; }
   public string  OpenAI_API_Key { get; set; } = "";
   public string  LanguageModel  { get; set; } = "gpt-4-1106-preview";
   public string  LastUsedTulpa  { get; set; } = DefaultAssistant_Filename;

   public double            MainSplitter_relative_position { get; set; } = .2;
   public double            TextSplitter_relative_position { get; set; } = .5;
   public WindowParameters? WindowParameters               { get; set; }


   static Config() {
      Application_Paths.Config_File.Directory.Create();

      try {
         prism_css = File.ReadAllText(WebstuffsPrismFancyCss);
         prism_js  = File.ReadAllText(WebstuffsPrismFancyJs);
         my_css    = File.ReadAllText(WebstuffsMyCss);
      }
      catch (Exception e) {
         MessageBox.Show(
            $"Error while loading Prism CSS and JS files.\r\n"                       +
            $"Please make sure that the files\r\n"                                   +
            $"{WebstuffsPrismFancyCss} and {WebstuffsPrismFancyJs} are present.\r\n" +
            $"{e.Message}",
            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
         Environment.Exit(1);
      }
   }

   public static void Load() {
      lock (_lock) {
         if (loading)
            throw new Exception("Config is already loading.");
         loading = true;
      }

      if (!Application_Paths.Config_File.Exists) {
         ConfigErrorCase(false);
         loading = false;
         return;
      }

      JsonSerializerSettings settings = new() {
         ObjectCreationHandling = ObjectCreationHandling.Replace,
      };
      Config? loadedConfig = JsonConvert.DeserializeObject<Config>(File.ReadAllText(Application_Paths.Config_File.FullName), settings);
      //if (Tools.HasNullProperties(loadedConfig)) {
      if (loadedConfig is null) {
         ConfigErrorCase();
      }
      else {
         Active = loadedConfig;
      }

      lock (_lock) {
         loading = false;
      }
   }

   public static void Save() {
      lock (_lock) {
         if (loading)
            return;
         //throw new Exception("Do not save while loading.");
      }

      if (string.IsNullOrWhiteSpace(Active.OpenAI_API_Key))
         throw new Exception("OpenAI API Key is empty.");

      try {
         var contents = JsonConvert.SerializeObject(Active, Formatting.Indented);
         File.WriteAllText(Application_Paths.Config_File.FullName, contents);
      }
      catch (Exception e) {
         MessageBox.Show($"Error while saving configuration file: {e.Message}", "Error", MessageBoxButtons.OK,
            MessageBoxIcon.Error);
      }
   }

   private static void ConfigErrorCase(bool invalid = true) {
      var msg = invalid ? "is invalid" : "was not found";
      MessageBox.Show($"The configuration file {msg}. Creating default configuration.", "Error", MessageBoxButtons.OK,
         MessageBoxIcon.Error);
      Active = new Config();
      Save();
   }
}

public class WindowParameters {
   public FormStartPosition StartPosition { get; set; } = FormStartPosition.Manual;
   public FormWindowState   WindowState   { get; set; } = FormWindowState.Normal;

   public Point Location { get; set; }
   public Size  Size     { get; set; }
}