using Newtonsoft.Json;

namespace WinGPT.OpenAI;

internal static class ErrorCodes {
   private static ErrorCode[]? _fromDocumentation;

   public static ErrorCode[] FromDocumentation {
      get { return _fromDocumentation ??= CreateFromFile(); }
   }

   private static ErrorCode[] CreateFromFile() {
      ErrorCode[]? errorCodes = null;
      try {
         var json = File.ReadAllText("OpenAI/ErrorCodes.json");
         errorCodes ??= JsonConvert.DeserializeObject<ErrorCode[]>(json);
      }
      catch (Exception e) {
         MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }

      errorCodes ??= Array.Empty<ErrorCode>();
      return errorCodes;
   }
}