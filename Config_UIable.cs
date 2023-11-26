using System.ComponentModel;
using System.Drawing.Design;

namespace WinGPT;

public class Config_UIable {
   public int? Max_Tokens        { get; set; }
   public int? Vision_Max_Tokens { get; set; } = 1000;

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
        var font = nativeFontDialog.open((Font)value);

        //return base.EditValue(context, provider, value);
        return font;
   }
}


