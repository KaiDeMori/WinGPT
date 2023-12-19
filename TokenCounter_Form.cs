namespace WinGPT;

public partial class TokenCounter_Form : Form {
   public TokenCounter_Form() {
      InitializeComponent();
      this.AcceptButton = ok_button;
      SetValues();
   }

   private void SetValues() {
      prompt_tokens_label.Text                  = Config.Active.TokenCounter.Prompt_Tokens.ToString();
      completion_tokens_label.Text              = Config.Active.TokenCounter.Completion_Tokens.ToString();
      total_tokens_label.Text                   = Config.Active.TokenCounter.Total_Tokens.ToString();
      token_limit_numericUpDown.Value           = Config.Active.TokenCounter.Token_Limit;
      show_token_limit_message_checkBox.Checked = Config.Active.TokenCounter.Show_Message;
   }

   private void token_limit_numericUpDown_ValueChanged(object sender, EventArgs e) {
      Config.Active.TokenCounter.Token_Limit = (int) token_limit_numericUpDown.Value;
   }

   private void reset_button_Click(object sender, EventArgs e) {
      Config.Active.TokenCounter.Reset();
      SetValues();
   }

   private void ok_button_Click(object sender, EventArgs e) {
      this.DialogResult = DialogResult.OK;
      this.Close();
   }

   private void show_token_limit_message_checkBox_CheckedChanged(object sender, EventArgs e) {
      token_limit_numericUpDown.Enabled       = show_token_limit_message_checkBox.Checked;
      Config.Active.TokenCounter.Show_Message = show_token_limit_message_checkBox.Checked;
   }
}