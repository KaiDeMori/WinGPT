using System.Diagnostics.CodeAnalysis;
using System.Text;
using Newtonsoft.Json;

namespace WinGPT;

using Message = OpenAI.Chat.Message;

public class Conversation {
   [JsonIgnore]
   public static Conversation? Active { get; private set; }

   public required Conversation_Info Info { get; set; }

   public required FileInfo HistoryFile { get; set; }

   /// <summary>
   /// The messages in the conversation.
   /// Does not contain system messages or other tulpa messages.
   /// </summary>
   public List<Message> Messages { get; set; } = new();

   /// <summary>
   /// Make setter private again
   /// </summary>
   public bool useSysMsgHack;

   public bool taxonomy_required = true;

   private Conversation() {
   }

   public static void Update_Conversation(Message user_message, Message[] responses) {
      if (Active is null)
         throw new Exception("There is no active conversation!");

      Active.Messages.Add(user_message);
      Active.Messages.AddRange(responses);
   }

   [MemberNotNull(nameof(Active))]
   public static void Create_Conversation(Message first_prompt, Tulpa tulpa) {
      if (Active != null) {
         Active.Save();
         Active = null;
      }

      Active = Save_Preliminary(first_prompt, tulpa);
   }

   /// <summary>
   /// This loads the active conversation from the history file.
   /// Sneaky little side effect: It also saves the existing one.
   /// </summary>
   /// <param name="history_file"></param>
   /// <exception cref="Exception">Throws if a conversation is already active.</exception>
   [MemberNotNullWhen(true, nameof(Active))]
   public static bool Load(FileInfo history_file) {
      if (Active != null) {
         Active.Save();
         Active = null;
      }

      if (!TryParseConversationHistoryFile(history_file, out var conversation)) {
         return false;
      }

      Active = conversation;
      return true;
   }

   public static void Reset() {
      if (Active != null)
         throw new Exception("There is already an active conversation!");
      Active?.Save();
      Active = null;
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
   private static Conversation Save_Preliminary(Message first_prompt, Tulpa tulpa) {
      var preliminary_filename = Create_preliminary_Conversation_filename(tulpa);
      var preliminary_file     = new FileInfo(Path.Join(Config.Preliminary_Conversations_Path.FullName, preliminary_filename));
      var conversation = new Conversation() {
         HistoryFile = preliminary_file,
         Info = new Conversation_Info {
            TulpaFile = tulpa.File.Name,
         }
      };
      var content = conversation.Create_history_file_content(first_prompt);
      //DRAGONS a lot can go wrong here
      //make at least sure the whole directory exists and if not create it
      if (!preliminary_file.Directory!.Exists) {
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

   public static FileUpdateLocationResult TryRenameFile(
      string newFilename
   ) {
      if (Active == null)
         throw new Exception("There is no active conversation!");

      var history_file = Active.HistoryFile;
      try {
         if (!history_file.Exists) {
            return FileUpdateLocationResult.FileDoesNotExist;
         }

         return create_updated_file_in_new_location_then_delete_the_old_one(
            history_file.Directory!,
            newFilename
         );
      }
      catch (Exception) {
         return FileUpdateLocationResult.UnknownError;
      }
   }

   private static FileUpdateLocationResult create_updated_file_in_new_location_then_delete_the_old_one(
      DirectoryInfo destinationDirectory,
      string        new_filename
   ) {
      if (Active == null)
         throw new Exception("There is no active conversation!");

      if (!Active.HistoryFile.Exists)
         return FileUpdateLocationResult.FileDoesNotExist;

      FileInfo originalFile = Active.HistoryFile;

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
         Active.HistoryFile = newFile;
         Active.Save(); // Assuming Save() method uses the HistoryFile to know where to save the file.
         Active.HistoryFile.Refresh();
      }
      catch (Exception) {
         // Handle exceptions as you deem appropriate
         return FileUpdateLocationResult.FileCouldNotBeCreated;
      }

      // If new file has been created, try to delete the original file
      if (Active.HistoryFile.Exists) {
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

   //private static string AddTimestampToFilename(string filename) {
   //   return Path.GetFileNameWithoutExtension(filename)
   //          + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff")
   //          + Path.GetExtension(filename);
   //}

   public static readonly Dictionary<FileUpdateLocationResult, string> ErrorMessages = new() {
      {FileUpdateLocationResult.Success, "The file was successfully moved."},
      {FileUpdateLocationResult.SuccessWithRename, "The file was successfully moved, but had to be renamed due to an existing file with the same name."},
      {FileUpdateLocationResult.FileDoesNotExist, "The file you're trying to move does not exist."},
      {FileUpdateLocationResult.CategoryDoesNotExist, "The category you're trying to move the file to does not exist."},
      {FileUpdateLocationResult.CategoryExistsButShouldNot, "The category you're trying to create already exists."},
      {FileUpdateLocationResult.UnknownError, "An unknown error occurred while trying to move the file."},
   };

   private static bool TryParseConversationHistoryFile(FileInfo historyFile, [NotNullWhen(true)] out Conversation? conversation) {
      conversation = null;

      if (!historyFile.Exists)
         return false;

      //read into file_content with try catch
      string file_content;
      try {
         file_content = File.ReadAllText(historyFile.FullName);
      }
      catch (Exception) {
         return false;
      }

      var memory      = file_content.AsMemory();
      var currentSpan = memory.Span;

      // Process the ConversationHistory section and get the file name
      if (!HistoryFileParser.TryParseHistoryHeader(ref currentSpan, out var info)) {
         return false; // Couldn't parse ConversationHistory
      }

      var messages = new List<Message>();

      // Continue with the rest of the parsing as before
      while (!currentSpan.IsEmpty) {
         int     firstDelimiterIndex = currentSpan.Length;
         string? delimiterFound      = null;

         foreach (var delimiter in SpecialTokens.To_API_Role.Keys) {
            int index = currentSpan.IndexOf(delimiter.AsSpan());
            if (index >= 0 && index < firstDelimiterIndex) {
               firstDelimiterIndex = index;
               delimiterFound      = delimiter;
            }
         }

         // No delimiter found, break out of the loop
         if (delimiterFound == null) {
            break;
         }

         // Could throw an exception if the slice end index is out of range
         var afterDelimiter     = currentSpan[(firstDelimiterIndex + delimiterFound.Length)..];
         var nextDelimiterIndex = afterDelimiter.Length;

         foreach (var delimiter in SpecialTokens.To_API_Role.Keys) {
            int index = afterDelimiter.IndexOf(delimiter.AsSpan());
            if (index >= 0 && index < nextDelimiterIndex) {
               nextDelimiterIndex = index;
            }
         }

         // Could throw an exception if the slice end index is out of range
         var content = afterDelimiter[..nextDelimiterIndex].Trim();

         // Could throw an exception if the Role key doesn't exist in the dictionary
         messages.Add(new Message {
            role    = SpecialTokens.To_API_Role[delimiterFound],
            content = content.ToString()
         });

         // Could throw an exception if the slice start index is out of range
         currentSpan = afterDelimiter[nextDelimiterIndex..];
      }

      conversation = new Conversation {
         HistoryFile       = historyFile,
         Info              = info,
         Messages          = messages,
         taxonomy_required = false,
      };

      return true;
   }
}