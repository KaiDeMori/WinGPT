using System.Text.RegularExpressions;

namespace WinGPT;

public partial class API_KEY_Form : Form {
   private readonly ErrorProvider errorProvider = new ErrorProvider(); // create an instance of ErrorProvider
   private readonly ErrorProvider infoProvider  = new ErrorProvider(); // create an instance of ErrorProvider

   private readonly Regex key_regex = new Regex("^sk-[a-zA-Z0-9]{48}$");

   private DialogResult currentDialogResult = DialogResult.Cancel;

   public API_KEY_Form() {
      InitializeComponent();
      this.AcceptButton = api_key_ok_button;
      Icon   originalIcon = SystemIcons.Information;
      Bitmap bmp          = new Bitmap(originalIcon.ToBitmap(), new Size(16, 16)); // Resize to desired dimensions
      IntPtr hIcon        = bmp.GetHicon();
      Icon   resizedIcon  = Icon.FromHandle(hIcon);
      infoProvider.Icon           =  resizedIcon;
      infoProvider.Icon           =  resizedIcon;
      api_key_textBox.TextChanged += api_key_textBox_TextChanged;
      errorProvider.SetIconAlignment(api_key_textBox, ErrorIconAlignment.MiddleRight);
      errorProvider.SetIconPadding(api_key_textBox, -30);
      infoProvider.SetIconAlignment(api_key_textBox, ErrorIconAlignment.MiddleRight);
      infoProvider.SetIconPadding(api_key_textBox, -30);
   }

   private void api_key_textBox_TextChanged(object? sender, EventArgs e) {
      Validate();
   }

   private new void Validate() {
      errorProvider.SetError(api_key_textBox, "");
      infoProvider.SetError(api_key_textBox, "");
      if (string.IsNullOrWhiteSpace(api_key_textBox.Text)) {
         errorProvider.SetError(api_key_textBox, "API Key cannot be empty.");
         //MessageBox.Show("API Key cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
         currentDialogResult       = DialogResult.Cancel;
         api_key_ok_button.Enabled = false;
         return;
      }

      var apiKey = api_key_textBox.Text.Trim();
      if (key_regex.IsMatch(apiKey)) {
         // set error message on api_key_textBox
         infoProvider.SetError(api_key_textBox, "Valid OpenAI API key.");
         currentDialogResult       = DialogResult.OK;
         api_key_ok_button.Enabled = true;
         return;
      }

      if (apiKey.Length == 32) {
         infoProvider.SetError(api_key_textBox, "PeopleOfThePrompt!");
         currentDialogResult       = DialogResult.OK;
         api_key_ok_button.Enabled = true;
         return;
      }

      infoProvider.SetError(api_key_textBox, "Not an OpenAI API key.");
      currentDialogResult       = DialogResult.Cancel;
      api_key_ok_button.Enabled = false;

      // if the input is valid, clear the error message
      //errorProvider.SetError(api_key_textBox, "");
   }

   private void api_key_ok_button_Click(object sender, EventArgs e) {
      if (currentDialogResult == DialogResult.OK) {
         Config.Active.OpenAI_API_Key = api_key_textBox.Text.Trim();
         try {
            Config.Save();
            DialogResult = DialogResult.OK;
         }
         catch (Exception exception) {
            MessageBox.Show($"Error saving config file:\r\n{exception.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }

      DialogResult = currentDialogResult;
   }
}