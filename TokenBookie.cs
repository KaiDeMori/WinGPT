using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WinGPT;

public partial class TokenBookie : UserControl {
   public TokenBookie() {
      InitializeComponent();
   }
}

public class TokenBook {
   [Category("Request")]
   [DisplayName("Prompt          c"), Display(Order = 0)]
   public uint Prompt { get; set; } = 123;

   [Category("Request")]
   public uint Files { get; set; } = 123;

   [Category("Request")]
   public uint Tulpa { get; set; } = 123;

   [Category("Request")]
   [DisplayName("Total")]
   public uint Request_Total { get; set; } = 123;

   [Category("Response")]
   public uint Input { get; set; } = 123;

   [Category("Response")]
   public uint Output { get; set; } = 123;

   [Category("Response")]
   [DisplayName("Total")]
   public uint Response_Total { get; set; } = 123;
}