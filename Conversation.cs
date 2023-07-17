using System.Collections.Immutable;
using System.Text;
using WinGPT.OpenAI;
using WinGPT.OpenAI.Chat;
using WinGPT.Taxonomy;

namespace WinGPT;

using Message = OpenAI.Chat.Message;

public class Conversation {
   public Conversation_Info Info { get; set; } = new();

   /// <summary>
   /// The messages in the conversation.
   /// Does not contain system messages or other tulpa messages.
   /// </summary>
   public List<Message> Messages { get; set; } = new();

   /// <summary>
   /// Make setter private again
   /// </summary>
   public FileInfo HistoryFile { get; private set; }

   public bool useSysMsgHack;

   //DRAGONS set to true again!
   public bool taxonomy_required = true;

   public Conversation(FileInfo historyFile) {
      HistoryFile = historyFile;
   }

   /// <summary>
   /// Creates the content string for the history file.
   /// A preliminary prompt can be provided.
   /// Usually this is used to save the prompt before sending it.
   /// This message will not be included in the conversation history.
   /// This is used to save the prompt, in case sending it fails.
   /// </summary>
   /// <param name="temporary_prompt"></param>
   /// <returns>The content string for the history file.</returns>
   private string Create_history_file_content(Message? temporary_prompt) {
      StringBuilder sb = new();
      sb.Append(SpecialTokens.ConversationHistory);
      StringBuilder info = Info.Create_history_file_content();
      sb.Append(info);

      if (temporary_prompt is null) {
         foreach (var message in Messages) {
            sb.AppendLine(message.ToString());
         }
      }
      else {
         sb.AppendLine(temporary_prompt.ToString());
      }

      return sb.ToString();
   }

   public string Create_markf278down() {
      //we also need to replace the default newline /n with the system newline /r/n
      string messages = string.Join("", Messages.Select(m => m.ToString().Replace("\n", Environment.NewLine)));
      return messages;
   }

   /// <summary>
   /// Saves the conversation to the history file.
   /// </summary>
   public void Save() {
      if (HistoryFile is null)
         throw new Exception("HistoryFile must not be null!");

      var content = Create_history_file_content(null);
      File.WriteAllText(HistoryFile.FullName, content);
   }

   /// <summary>
   /// Saves a preliminary version of the conversation, containing the prompt.
   /// Since the conversation is not yet taxonomized, we save it with a temporary filename.
   /// </summary>
   /// <param name="first_prompt">The first prompt of the conversation, before sending it.</param>
   /// <param name="tulpa">The tulpa that will be used to create the filename.</param>
   public static Conversation Save_Preliminary(Message first_prompt, Tulpa tulpa) {
      var preliminary_filename = Create_preliminary_Conversation_filename(tulpa);
      var preliminary_file     = new FileInfo(Path.Join(Config.Preliminary_Conversations_Path.FullName, preliminary_filename));
      var conversation = new Conversation(preliminary_file) {
         Info = new Conversation_Info {
            TulpaFile = tulpa.File.Name,
         }
      };
      var content = conversation.Create_history_file_content(first_prompt);
      //DRAGONS a lot can go wrong here
      //make at least sure the whole directory exists and if not create it
      if (!preliminary_file.Directory.Exists) {
         preliminary_file.Directory.Create();
      }

      File.WriteAllText(preliminary_file.FullName, content);
      return conversation;
   }

   private static string Create_preliminary_Conversation_filename(Tulpa tulpa) {
      var tulpa_name     = tulpa.Configuration.Name;
      var valid_filename = Tools.To_valid_filename(tulpa_name);
      var name_only      = $"{valid_filename} - {DateTime.Now:yyyy-MM-dd HH-mm-ss}";
      var filename       = $"{name_only}{Config.marf278down_extenstion}";
      //TADA some directory stuff here
      return filename;
   }

   public FileUpdateLocationResult TryCategorizeFile(
      string        new_filename,
      string        category,
      bool          should_exist,
      DirectoryInfo base_directory
   ) {
      try {
         if (!HistoryFile.Exists) {
            return FileUpdateLocationResult.FileDoesNotExist;
         }

         var categoryDirectory = new DirectoryInfo(Path.Combine(base_directory.FullName, category));

         switch (should_exist) {
            case true when !categoryDirectory.Exists:
               return FileUpdateLocationResult.CategoryDoesNotExist;
            case false when categoryDirectory.Exists:
               return FileUpdateLocationResult.CategoryExistsButShouldNot;
            default: {
               if (!categoryDirectory.Exists) {
                  categoryDirectory.Create();
               }

               break;
            }
         }

         return create_updated_file_in_new_location_then_delete_the_old_one(categoryDirectory, new_filename);
      }
      catch (Exception) {
         return FileUpdateLocationResult.UnknownError;
      }
   }

   public FileUpdateLocationResult TryRenameFile(
      string newFilename
   ) {
      try {
         if (!HistoryFile.Exists) {
            return FileUpdateLocationResult.FileDoesNotExist;
         }

         return create_updated_file_in_new_location_then_delete_the_old_one(
            HistoryFile.Directory,
            newFilename
         );
      }
      catch (Exception) {
         return FileUpdateLocationResult.UnknownError;
      }
   }

   private FileUpdateLocationResult create_updated_file_in_new_location_then_delete_the_old_one(
      DirectoryInfo destinationDirectory,
      string        new_filename
   ) {
      if (!HistoryFile.Exists)
         return FileUpdateLocationResult.FileDoesNotExist;

      FileInfo originalFile = HistoryFile;

      // Create new FileInfo instance for the new file
      FileInfo newFile = new FileInfo(Path.Combine(destinationDirectory.FullName, new_filename));

      // Check if the new file already exists
      if (newFile.Exists) {
         // Rename the file
         string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
         newFile = new FileInfo(Path.Combine(destinationDirectory.FullName,
            $"{Path.GetFileNameWithoutExtension(new_filename)}_{timestamp}{newFile.Extension}"));
      }

      // Try to create the new file
      try {
         HistoryFile = newFile;
         Save(); // Assuming Save() method uses the HistoryFile to know where to save the file.
         HistoryFile.Refresh();
      }
      catch (Exception) {
         // Handle exceptions as you deem appropriate
         return FileUpdateLocationResult.FileCouldNotBeCreated;
      }

      // If new file has been created, try to delete the original file
      if (HistoryFile.Exists) {
         try {
            originalFile.Delete();
         }
         catch (Exception) {
            // Handle exceptions as you deem appropriate
            return FileUpdateLocationResult.FileCouldNotBeDeleted;
         }

         // Check if we had to rename the new file
         return newFile.Name ==
                new_filename
            ? FileUpdateLocationResult.Success
            : FileUpdateLocationResult.SuccessWithRename;
      }

      // If we reach this point, something unexpected occurred
      return FileUpdateLocationResult.UnknownError;
   }


   //private FileMoveResult MoveFileToDestination(
   //   DirectoryInfo destinationDirectory,
   //   string        new_filename
   //) {
   //   var new_file_fullname = Path.Combine(destinationDirectory.FullName, new_filename);

   //   if (File.Exists(new_file_fullname)) {
   //      new_filename = AddTimestampToFilename(new_filename);

   //      // Update the destinationPath with the new filename
   //      new_file_fullname = Path.Combine(destinationDirectory.FullName, new_filename);

   //      HistoryFile.MoveTo(new_file_fullname);

   //      HistoryFile = new FileInfo(new_file_fullname);

   //      return FileMoveResult.SuccessWithRename;
   //   }

   //   HistoryFile.MoveTo(new_file_fullname);

   //   HistoryFile = new FileInfo(new_file_fullname);

   //   return FileMoveResult.Success;
   //}

   private static string AddTimestampToFilename(string filename) {
      return Path.GetFileNameWithoutExtension(filename)
             + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff")
             + Path.GetExtension(filename);
   }

   private static readonly Dictionary<FileUpdateLocationResult, string> ErrorMessages = new() {
      {FileUpdateLocationResult.Success, "The file was successfully moved."},
      {FileUpdateLocationResult.SuccessWithRename, "The file was successfully moved, but had to be renamed due to an existing file with the same name."},
      {FileUpdateLocationResult.FileDoesNotExist, "The file you're trying to move does not exist."},
      {FileUpdateLocationResult.CategoryDoesNotExist, "The category you're trying to move the file to does not exist."},
      {FileUpdateLocationResult.CategoryExistsButShouldNot, "The category you're trying to create already exists."},
      {FileUpdateLocationResult.UnknownError, "An unknown error occurred while trying to move the file."},
   };

   public static void ShowError(FileUpdateLocationResult result) {
      string title = result == FileUpdateLocationResult.Success || result == FileUpdateLocationResult.SuccessWithRename ? "File Move Success" : "File Move Error";
      MessageBox.Show(ErrorMessages[result], title, MessageBoxButtons.OK, MessageBoxIcon.Information);
   }
}