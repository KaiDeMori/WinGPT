using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using System.Windows.Forms;
using WinGPT.OpenAI.Chat;
using WinGPT.Taxonomy;

namespace WinGPT;

internal static class WinGPT_Main {
   /// <summary>
   ///  The main entry point for the application.
   /// </summary>
   [STAThread]
   static void Main(string[] args) {
      if (CustomAction_Uninstall_Check(args))
         return;

      //QuickTulpaTest.run();
      //QuickHistoryTest.run();

      //GypsyLogging.EnableLogging();


      // To customize application configuration such as set high DPI settings or default font,
      // see https://aka.ms/applicationconfiguration.
      ApplicationConfiguration.Initialize();
      Application.Run(new WinGPT_Form());

      //var x = new FunctionCallSettings("my_function_name");
      //var y = new FunctionCallSettings(Function_Call_Mode.auto);

      //DRAGONS be-gone!
      //Function_Parmeters parms = new() {
      //   summary             = "testsummyr",
      //   filename            = "file.md",
      //   new_category        = "Cat F",
      //   existing_categories = new[] {"Category A", "Category B", "Cat C", "CatD"},
      //   selected_category   = "Category B"
      //};
      //Application.Run(new Taxonomy_Form(parms));
      //Debug.WriteLine(parms);
   }

   private static bool CustomAction_Uninstall_Check(string[] args) {
      if (args.Contains(Application_Paths.Uninstall_Parameter)) {
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
      }

      //just for future safety
      //if (args.Contains(Install_Parameter))
      //   return true;

      return false;
   }
}