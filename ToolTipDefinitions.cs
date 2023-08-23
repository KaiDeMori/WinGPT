namespace WinGPT;

class ToolTipDefinitions {
   private static WinGPT_Form _form;

   public static void SetToolTips(WinGPT_Form form) {
      _form = form;
      stt(form.autoclear_checkBox,
         "Automatically clear the prompt text after sending the prompt.");
      stt(form.clear_button,
         "Clears the prompt text.");
      stt(form.history_file_name_textBox,
         "The name of the file to save the history to.\r\n" +
         "Double click to start Taxonoy!");
      stt(form.remove_file_button,
         "Removes the selected file from the list.");
   }

   private static void stt(Control control, string toolTipText) {
      _form.main_toolTip.SetToolTip(control, toolTipText);
   }
}