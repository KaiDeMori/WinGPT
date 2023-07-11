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
   }
}