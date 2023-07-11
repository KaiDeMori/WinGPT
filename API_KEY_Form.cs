using System.Text.RegularExpressions;

namespace WinGPT;

public partial class API_KEY_Form : Form
{
    private ErrorProvider errorProvider = new ErrorProvider(); // create an instance of ErrorProvider
    private Regex key_regex = new Regex("^sk-[a-zA-Z0-9]{48}$");

    public API_KEY_Form()
    {
        InitializeComponent();
        this.AcceptButton = api_key_ok_button;
    }

    private void api_key_ok_button_Click(object sender, EventArgs e)
    {
        errorProvider.SetIconAlignment(api_key_textBox, ErrorIconAlignment.MiddleRight);
        errorProvider.SetIconPadding(api_key_textBox, -30);
        if (string.IsNullOrWhiteSpace(api_key_textBox.Text))
        {
            errorProvider.SetError(api_key_textBox, "API Key cannot be empty.");
            //MessageBox.Show("API Key cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //DialogResult = DialogResult.Cancel;
            return;
        }

        var apiKey = api_key_textBox.Text;
        if (!key_regex.IsMatch(apiKey))
        {
            // set error message on api_key_textBox
            errorProvider.SetError(api_key_textBox, "Invalid API Key format.");
            return;
        }

        // if the input is valid, clear the error message
        errorProvider.SetError(api_key_textBox, "");


        Config.Active.OpenAI_API_Key = api_key_textBox.Text;
        Config.Save();
        DialogResult = DialogResult.OK;
        Close();
    }
}