using System.Diagnostics;

namespace WinGPT;

internal static class WinGPT_Main {
   [STAThread]
   static void Main(string[] args) {
      if (CustomAction_Uninstall_Check(args))
         return;

      Config.Load();
      Application.SetHighDpiMode(HighDpiMode.DpiUnaware);

      ApplicationConfiguration.Initialize();
      Application.Run(new WinGPT_Form());
   }

   private static bool CustomAction_Uninstall_Check(string[] args) {
      if (!args.Contains(Application_Paths.Uninstall_Parameter))
         return false;
      
      if (!Application_Paths.Config_File.Exists)
         return true;

      //maybe just search through all the windows and see if any of them are the installer window?
      var proc   = Process.GetProcessesByName("msiexec").FirstOrDefault(p => p.MainWindowTitle  == Application_Paths.AppName);
      var proc_e = Process.GetProcessesByName("explorer").FirstOrDefault(p => p.MainWindowTitle == Application_Paths.AppName);
      proc ??= proc_e;

      var form = new CustomAction_Uninstall_ConfigDeletion();

      DialogResult deleteConfig = DialogResult.No;
      if (proc != null) {
         var helper = new NativeWindow();
         helper.AssignHandle(proc.MainWindowHandle);
         deleteConfig = form.ShowDialog(helper);
         helper.ReleaseHandle();
      }
      //else {
      //   form.ShowDialog();
      //   deleteConfig = form.DialogResult;
      //}

      if (deleteConfig == DialogResult.Yes) {
         //delete the config file
         if (Application_Paths.Config_File.Exists) Application_Paths.Config_File.Delete();
      }

      return true;

      //just for future safety
      //if (args.Contains(Install_Parameter))
      //   return true;
   }
}