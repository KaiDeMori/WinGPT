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

   [Category("markf278down text-box")]
   [DisplayName("Font")]
   [Description("This controls the font of the raw markf278down response text-box.")]
   public Font markf278down_Font { get; set; } = new Font("Arial", 12);

   // the corrected version would be
   [Category("markf278down text-box")]
   [DisplayName("Readonly")]
   [Description("If true, the response text-box is read-only.")]
   public bool markf278down_readonly { get; set; } = true;

   /// <summary>
   /// This controls the auto-scrolling of the markf278down text-box.
   /// </summary>
   [Category("markf278down text-box")]
   [DisplayName("Use Auto-Scroll")]
   [Description("If true, the response text-box is scrolled to the last response. ")]
   public bool Auto_Scroll { get; set; } = true;
}