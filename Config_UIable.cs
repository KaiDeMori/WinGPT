using System.ComponentModel;

namespace WinGPT;

public class Config_UIable {
   /// <summary>
   /// This controls the function that enables saving via prompt.
   /// </summary>
   [DisplayName("Use Save via Prompt Function")]
   public bool Use_Save_Function { get; set; } = true;

   /// <summary>
   /// This controls the function that enables saving via link in the HTML-page.
   /// </summary>
   [DisplayName("Use Save via Link Function")]
   public bool Use_Save_Link { get; set; } = true;
}