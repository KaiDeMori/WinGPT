using System.ComponentModel;
using WinGPT.Tokenizer;

namespace WinGPT;

/// <summary>
/// A wrapper around FileInfo in order to display token counts in the UI
/// </summary>
public class AssociatedFile : INotifyPropertyChanged {
   public AssociatedFile(FileInfo file) {
      File = file;
      UpdateTokenCount();
   }

   public void UpdateTokenCount() {
      try {
         var text_content = System.IO.File.ReadAllText(File.FullName);
         TokenCount = DeepTokenizer.count_tokens(
            text_content,
            Config.Active.LanguageModel);
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
         var s = File.Name +
                 (Config.Active.UIable.Show_Live_Token_Count ? $" ({TokenCount})" : String.Empty);
         return s;
      }
   }

   public FileInfo File       { get; }
   public int      TokenCount { get; private set; }

   public event PropertyChangedEventHandler? PropertyChanged;
}