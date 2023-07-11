using System.Globalization;
using System.Text;

namespace WinGPT;

internal class GypsyLogging {
   internal static void EnableLogging() {
      // Add the event handler for handling UI thread exceptions to the event.
      Application.ThreadException += new ThreadExceptionEventHandler(GlobalExceptionHandler);

      // Set the unhandled exception mode to force all Windows Forms errors to go through our handler.
      Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

      // Add the event handler for handling non-UI thread exceptions to the event. 
      AppDomain.CurrentDomain.UnhandledException +=
         new UnhandledExceptionEventHandler(GlobalExceptionHandler);
   }

   static void GlobalExceptionHandler(object sender, EventArgs e) {
      // Get the exception object from the EventArgs
      Exception ex;
      if (e is ThreadExceptionEventArgs) {
         ex = ((ThreadExceptionEventArgs) e).Exception;
      }
      else {
         ex = (Exception) ((UnhandledExceptionEventArgs) e).ExceptionObject;
      }

      // Now log the exception to a file
      LogExceptionToFile(ex);
   }

   static void LogExceptionToFile(Exception ex) {
      var now               = DateTime.UtcNow;
      var datetime_ISO_8501 = now.ToString("o", CultureInfo.InvariantCulture);
      var datetime_filename = now.ToString("yyyy-MM-dd_HH-mm-ss");

      // Replace with path where you want to save the log file
      string logFilePath = $"Debug_Log_{datetime_filename}.txt";

      // Build the error message string
      StringBuilder sb = new();
      sb.AppendLine("*********************************");
      sb.AppendLine(datetime_ISO_8501);
      sb.AppendLine(ex.Message);
      if (ex.InnerException != null) {
         sb.AppendLine("Inner Exception: " + ex.InnerException.Message);
      }

      sb.AppendLine(ex.StackTrace);
      sb.AppendLine("*********************************");

      // Log the error to the file
      File.AppendAllText(logFilePath, sb.ToString(), Encoding.UTF8);
   }
}