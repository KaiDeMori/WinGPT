﻿using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;

namespace WinGPT;

internal class Config {
   public static Config        Active      = new();
   public static Tulpa         ActiveTulpa = new();
   public static Conversation? ActiveConversation;

   public bool UseSysMsgHack { get; set; }

   private const  string Config_filename  = "Config.json";
   internal const string tulpas_directory = "Characters";

   private const   string        history_directory = "Conversation_History";
   internal static DirectoryInfo History_Directory => new(Path.Join(Active.BaseDirectory, history_directory));

   internal const string marf278down_extenstion = ".md";
   internal const string marf278down_filter     = "*.md";

   internal static readonly string prism_css;
   internal static readonly string my_css;
   internal static readonly string prism_js;


   internal const string        conversation_title_prompt           = "Generate an ultra short title for this conversation.";
   private const  string        WebstuffsPrismFancyCss              = "webstuffs/prism_fancy.css";
   private const  string        WebstuffsPrismFancyJs               = "webstuffs/prism_fancy.js";
   private const  string        WebstuffsMyCss                      = "webstuffs/my.css";
   private const  string        Preliminary_Conversations_Directory = "tmp";
   public static  DirectoryInfo Preliminary_Conversations_Path => new(Path.Join(Active.BaseDirectory, Preliminary_Conversations_Directory));

   private static readonly object _lock   = new();
   public static           bool   loading = false;

   public string? BaseDirectory  { get; set; }
   public string  OpenAI_API_Key { get; set; } = "";
   public string  LanguageModel  { get; set; } = "gpt-4";

   public TokenCounter TokenCounter { get; set; } = new();

   static Config() {
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

      var configfile = Path.Join(Application.StartupPath, Config_filename);

      if (!File.Exists(configfile)) {
         ConfigErrorCase(false);
         loading = false;
         return;
      }

      JsonSerializerSettings settings = new() {
         ObjectCreationHandling = ObjectCreationHandling.Replace,
      };
      Config? loadedConfig = JsonConvert.DeserializeObject<Config>(File.ReadAllText(configfile), settings);
      if (Tools.HasNullProperties(loadedConfig)) {
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

      var configfile = Path.Join(Application.StartupPath, Config_filename);
      try {
         var contents = JsonConvert.SerializeObject(Active, Formatting.Indented);
         File.WriteAllText(configfile, contents);
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