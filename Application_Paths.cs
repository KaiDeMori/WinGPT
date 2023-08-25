namespace WinGPT;

internal static class Application_Paths {
   /// <summary>
   /// The name of the application.
   /// </summary>
   public static readonly string AppName = AppDomain.CurrentDomain.FriendlyName; //"WinGPT";

   /// <summary>
   /// If the app was installed via MSI or OneClick, this file is present.
   /// If this file is present, WinGPT will use %LocalAppData% to store the configuration files.
   /// If it is absent (in which case you would probably not read this),
   /// WinGPT will store the configuration files in the same directory as the executable.
   /// </summary>
   private const string installed_marker_filename = "installed.marker";

   /// <summary>
   /// The parameter provided when the application is run to uninstall the application. :-)
   /// </summary>
   public const string Uninstall_Parameter = "/Uninstall";

   private const string Install_Parameter = "/Install";
   private const string POTP_directory    = "People_of_the_Prompt";

   public static readonly App_Modes APP_MODE;

   public enum App_Modes {
      Installed,
      Portable
   }

   public static readonly DirectoryInfo Config_Directory;

   private const          string   Config_filename = "Config.json";
   public static readonly FileInfo Config_File;

   private const          string   Treestate_Filename = "treestate.json";
   public static readonly FileInfo Treestate_File;

   static Application_Paths() {
      DirectoryInfo app_directory = new DirectoryInfo(Application.StartupPath);
      if (!File.Exists(installed_marker_filename) &&
          IsDirectoryWriteable(app_directory)) {
         APP_MODE         = App_Modes.Portable;
         Config_Directory = app_directory;
      }
      else {
         APP_MODE = App_Modes.Installed;
         Config_Directory = new DirectoryInfo(
            Path.Join(
               Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
               POTP_directory,
               AppName
            )
         );
      }

      Config_File    = new FileInfo(Path.Join(Config_Directory.FullName, Config_filename));
      Treestate_File = new FileInfo(Path.Join(Config_Directory.FullName, Treestate_Filename));
   }

   private static bool IsDirectoryWriteable(DirectoryInfo directory) {
      try {
         var ramdom_file = Path.Combine(
            directory.FullName,
            Path.GetRandomFileName()
         );
         using FileStream fs = File.Create(ramdom_file, 1);
         fs.Close();
         File.Delete(ramdom_file);
         return true;
      }
      catch (Exception) {
         return false;
      }
   }
}