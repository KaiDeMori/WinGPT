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
      var text_content = System.IO.File.ReadAllText(File.FullName);
      TokenCount = DeepTokenizer.count_tokens(
         text_content,
         Config.Active.LanguageModel);
      OnPropertyChanged(nameof(Name));
   }

   protected virtual void OnPropertyChanged(string propertyName) {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
   }

   //the name property should be the file name plus the token count
   public string Name {
      get {
         var s = File.Name +
                 (Config.Active.UIable.Show_Live_Token_Count ? $" ({TokenCount})" : String.Empty);
         return s;
      }
   }

   public FileInfo File { get; }

   public int                                TokenCount { get; private set; }
   public event PropertyChangedEventHandler? PropertyChanged;
}