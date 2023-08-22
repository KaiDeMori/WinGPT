using System.ComponentModel;

namespace WinGPT;

public class Config_UIable {
   public int? Max_Tokens { get; set; }

   /// <summary>
   /// This controls the function that enables saving via prompt.
   /// </summary>
   [DisplayName("Use Save via Prompt Function")]
   [Description("This controls the function that enables saving via prompt.")]
   public bool Use_Save_Function { get; set; } = true;

   /// <summary>
   /// This controls the function that enables saving via link in the HTML-page.
   /// </summary>
   [DisplayName("Use Save via Link Function")]
   [Description("This controls the function that enables saving via link in the HTML-page.")]
   public bool Use_Save_Link { get; set; } = true;

   [DisplayName("Prompt Font")]
   [Description("This controls the font of the prompt text-box.")]
   public Font Prompt_Font { get; set; } = new Font("Arial", 12);

   [DisplayName("Response Font")]
   [Description("This controls the font of the raw markf278down text-box.")]
   public Font markf278down_Font { get; set; } = new Font("Arial", 12);

   // the corrected version would be
   [DisplayName("markf278down text-box readonly")]
   [Description("This controls the readonly property of the markf278down text-box.")]
   public bool markf278down_readonly { get; set; } = true;
}