using System.Collections.Immutable;
using System.IO.Compression;

namespace WinGPT;

/// <summary>
/// Everything here will be called on the UI thread.
/// </summary>
internal static class Startup {
   private static Func<string>? PromptUserForDirectory;

   private static readonly ImmutableArray<Func<bool>> Assertions = ImmutableArray.Create(
      AssertOpenAI_API_Key,
      AssertBaseDirectory,
      AssertSubdirectories,
      AssertOpenAI_API_Key_is_valid
   );


   public static void AssertPrerequisitesOrFail(Func<string> PromptUserForDirectory_callback) {
      Startup.PromptUserForDirectory = PromptUserForDirectory_callback;

      if (!Assertions.Aggregate(true, (acc, assertion) => acc && assertion())) {
         Application.Exit();
      }
   }

   private static bool AssertOpenAI_API_Key_is_valid() {
      //TADA - check if the API key is valid
      return true;
   }

   private static bool AssertSubdirectories() {
      CreateDirectoryIfNotExists(Config.Tulpa_Directory);
      CreateDirectoryIfNotExists(Config.History_Directory);
      CreateDirectoryIfNotExists(Config.Preliminary_Conversations_Path);
      CreateDirectoryIfNotExists(Config.AdHoc_Downloads_Path);

      return true;
   }

   private static void CreateDirectoryIfNotExists(DirectoryInfo directory) {
      if (!directory.Exists)
         directory.Create();
   }

   private static bool AssertOpenAI_API_Key() {
      if (!string.IsNullOrWhiteSpace(Config.Active.OpenAI_API_Key))
         return true;

      using var form   = new API_KEY_Form();
      var       result = form.ShowDialog();

      if (result == DialogResult.Cancel) {
         // user closed the form without clicking OK button.
         // Let's show a nice message and exit the program.
         MessageBox.Show("A valid OpenAI API Key is required. The program will now exit.", "Error", MessageBoxButtons.OK);
         return false;
      }

      return true;
   }

   /// <summary>
   /// Checks if the BaseDirectory is set and valid. If not, prompts the user to select a new one.
   /// </summary>
   public static bool AssertBaseDirectory() {
      // Check if BaseDirectory is not empty and exists
      if (!string.IsNullOrEmpty(Config.Active.BaseDirectory) && Directory.Exists(Config.Active.BaseDirectory)) {
         // If BaseDirectory is set and valid, just return
         return true;
      }

      // Inform the user about the necessity of setting a base directory
      string message = string.IsNullOrEmpty(Config.Active.BaseDirectory)
         ? "The base directory has not been set. Please choose a base directory."
         : "The existing base directory could not be found. Please choose a new base directory.";

      MessageBox.Show(message, "Base Directory Required", MessageBoxButtons.OK);

      return UpdateBaseDirectory();
   }

   // a better name would be "PromptUserForBaseDirectory", but that is already used by Config.cs
   // so instead, an even better name witout the prompt would be "SetBaseDirectory"
   public static bool UpdateBaseDirectory() {
      // If BaseDirectory is not set, invalid or does not exist, prompt user to select a new one
      string selectedDirectory = PromptUserForDirectory();

      // Check if user canceled the dialog without selecting a directory
      if (string.IsNullOrEmpty(selectedDirectory)) {
         MessageBox.Show("A valid base directory is required. The program will now exit.", "Error", MessageBoxButtons.OK);
         return false;
      }

      // Check if directory selected by the user is not empty
      if (Directory.EnumerateFileSystemEntries(selectedDirectory).Any()) {
         // Ask user for confirmation if directory is not empty
         DialogResult dialogResult = MessageBox.Show("The selected directory is not empty. Are you sure you want to use this directory?", "Confirmation",
            MessageBoxButtons.YesNo);
         if (dialogResult == DialogResult.Yes) {
            // If user confirms, update the BaseDirectory
            Config.Active.BaseDirectory = selectedDirectory;
            AssertSubdirectories();
         }
         else {
            DialogResult openExplorerResult =
               MessageBox.Show(
                  "You chose not to use the selected directory because it's not empty. If you want to review the directory contents, we can open it in the file explorer. Would you like to do that?",
                  "Review Directory?", MessageBoxButtons.YesNo);
            if (openExplorerResult == DialogResult.Yes) {
               // Open the directory in the file explorer
               System.Diagnostics.Process.Start("explorer.exe", selectedDirectory);
            }

            MessageBox.Show("We understand. The application will now close. Feel free to restart the application and choose another directory.", "Exiting",
               MessageBoxButtons.OK);
            return false;
         }
      }
      else {
         Config.Active.BaseDirectory = selectedDirectory;
         AssertSubdirectories();
      }

      //create default Tulpas from resources
      var tulpas_zipped = WinGPT.Properties.Resources.Tulpas;

      using MemoryStream zipStream = new MemoryStream(tulpas_zipped);
      using ZipArchive   archive   = new ZipArchive(zipStream, ZipArchiveMode.Read);
      foreach (ZipArchiveEntry entry in archive.Entries) {
         string fullname = Path.Join(Config.Tulpa_Directory.FullName, entry.FullName);
         try {
            entry.ExtractToFile(fullname, false);
         }
         catch {
            // ignored
         }
      }

      // Save the updated configuration
      Config.Save();

      return true;
   }
}