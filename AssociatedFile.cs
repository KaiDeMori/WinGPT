using System.ComponentModel;
using WinGPT.Tokenizer;

namespace WinGPT;

/// <summary>
/// A wrapper around FileInfo in order to display token counts in the UI
/// </summary>
public class AssociatedFile : INotifyPropertyChanged {
   public AssociatedFile(FileInfo file) {
      File     = file;
      FileType = FileTypeIdentifier.GetFileType(file);
      UpdateTokenCount();
   }

   public void UpdateTokenCount() {
      if (FileType is FileType.Image or FileType.Other) {
         TokenCount = 0;
         return;
      }

      try {
         var text_content = System.IO.File.ReadAllText(File.FullName);
         TokenCount = CountTokenizer.count(text_content, Config.Active.Language_Model);
         OnPropertyChanged(nameof(Name));
      }
      catch (Exception) {
         // ignored
      }
   }

   protected virtual void OnPropertyChanged(string propertyName) {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
   }

   public string Name => File.Name;

   //the display name property should be the file name plus the token count if the option is enabled
   public string DisplayName {
      get {
         string count = TokenCount != 0 ? TokenCount.ToString() : "?";
         var s = File.Name +
                 (Config.Active.UIable.Show_Live_Token_Count ? $" ({count})" : String.Empty);
         return s;
      }
   }

   public FileInfo File       { get; }
   public int      TokenCount { get; private set; }
   public FileType FileType   { get; }

   public event PropertyChangedEventHandler? PropertyChanged;
}