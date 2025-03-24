using System.ComponentModel;
using System.Drawing.Design;
using WinGPT.OpenAI.Chat;

namespace WinGPT;

public class Config_UIable {
   public int? Max_Tokens        { get; set; }
   public int? Vision_Max_Tokens { get; set; } = 1000;

   /// <summary>
   /// This controls the function that enables saving via prompt.
   /// </summary>
   [Category("Features")]
   [DisplayName("Use Save via Prompt Function")]
   [Description("This controls the function that enables saving via prompt.")]
   public bool Use_Save_Via_Prompt { get; set; } = true;

   /// <summary>
   /// This controls the function that enables saving via link in the HTML-page.
   /// </summary>
   [Category("Features")]
   [DisplayName("Use Save via Link Function")]
   [Description("This controls the function that enables saving via link in the HTML-page.")]
   public bool Use_Save_Via_Link { get; set; } = true;

   [Category("Features")]
   [DisplayName("Taxonomy Model")]
   [Description("The ID of the model used by the Taxonomer.")]
   public string Taxonomy_Model { get; set; } = "gpt-4o-mini";

   [Category("Features")]
   [DisplayName("Auto-Save")]
   [Description("Automatically saves the conversation.")]
   public bool Auto_Save { get; set; } = false;

   [DisplayName("Prompt Font")]
   [Description("This controls the font of the prompt text-box.")]
   public Font Prompt_Font { get; set; } = new Font("Arial", 12);

   [Category("markf278down text-box")]
   [DisplayName("Font")]
   [Description("This controls the font of the raw markf278down response text-box.")]
   //[Category("Base"), Description("The font")]
   //[Editor(typeof(CustomFontEditor), typeof(UITypeEditor))]
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

   [Category("Performance")]
   [DisplayName("Show live token count.")]
   [Description("Shows the locally computed token count in various places.")]
   public bool Show_Live_Token_Count { get; set; } = false;

   [Category("Experimental")]
   [DisplayName("Remove toggle buttons")]
   [Description("Quick-Fix for the DPI error with Toggle-Button size and location.")]
   public bool Remove_Toggle_Buttons { get; set; } = false;

   [Category("Experimental")]
   [DisplayName("DPI awareness")]
   [Description("Sets the DPI-awareness. Needs app restart!")]
   public HighDpiMode High_DPI_Mode { get; set; } = HighDpiMode.DpiUnaware;

   [Category("Experimental")]
   [DisplayName("Auto Scale Mode")]
   [Description("Sets the AutoScaleMode. Needs app restart!")]
   public AutoScaleMode Auto_Scale_Mode { get; set; } = AutoScaleMode.Font;

   [Category("Experimental")]
   [DisplayName("Math rendering")]
   [Description("Uses MathJax and a custom prompt to create beautiful formulas!")]
   public bool Math_Rendering { get; set; } = true;

   /// <summary>
   /// This is the interval used for the tokenizer call in milliseconds.
   /// </summary>
   [Category("Performance")]
   [DisplayName("Interval for Token-Counting and Auto-Save. [ms]")]
   [Description("After this time of inactivity, the token-counting and auto-save will be triggered.")]
   public int prompt_actions_timer_interval { get; set; } = 2000;

   [Category("o1 / Reasoning Models")]
   [DisplayName("Reasoning Effort")]
   [Description(
      "Constrains effort on reasoning for reasoning models. Currently supported values are low, medium, and high. Reducing reasoning effort can result in faster responses and fewer tokens used on reasoning in a response.")]

   public reasoning_effort reasoning_effort { get; set; } = reasoning_effort.medium;

   [Category("Vision")]
   [DisplayName("Image Detail")]
   [Description(
      "The detail parameter tells the model what level of detail to use when processing and understanding the image (low, high, or auto to let the model decide). If you skip the parameter, the model will use auto.")]
   public image_detail image_detail { get; set; } = image_detail.auto;
}

/// <summary>
/// Trying to fix two bugs in the framework, but:
/// fogetaboutit!
/// * Font Size is wrongly calculated and off by either 0.25 of 0.111…
/// * Font Name and preview cannot be set in the FontDialog, for strange weights like "Arial Narrow Bold"
/// </summary>
public class CustomFontEditor : UITypeEditor {
   public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) {
      return UITypeEditorEditStyle.Modal;
   }

   public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value) {
      //FontDialog dlg = new FontDialog();
      //dlg = new FontDialog();

      //if (value is Font font)
      //{
      //    dlg.Font = font;
      //}

      //if (dlg.ShowDialog() == DialogResult.OK)
      //    return dlg.Font;

      var nativeFontDialog = new NativeFontDialog();
      var font             = nativeFontDialog.open((Font) value);

      //return base.EditValue(context, provider, value);
      return font;
   }
}