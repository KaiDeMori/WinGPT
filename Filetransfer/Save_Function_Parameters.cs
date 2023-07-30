namespace WinGPT.Filetransfer;

public class Save_Function_Parameters {
   public string? filename     { get; set; }
   public string? text_content { get; set; }

   public override string ToString() {
      return $"{nameof(filename)}: {filename}\r\n" +
             $"{nameof(text_content)}: {text_content}\r\n";
   }
}