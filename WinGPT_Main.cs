using System.Diagnostics;
using WinGPT.OpenAI.Chat;
using WinGPT.Taxonomy;

namespace WinGPT;

internal static class WinGPT_Main {
   /// <summary>
   ///  The main entry point for the application.
   /// </summary>
   [STAThread]
   static void Main() {
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
}