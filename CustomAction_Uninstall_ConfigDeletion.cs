namespace WinGPT;

public partial class CustomAction_Uninstall_ConfigDeletion : Form
{
    public CustomAction_Uninstall_ConfigDeletion()
    {
        InitializeComponent();
        this.AcceptButton = keep_button;

        this.delete_button.DialogResult = DialogResult.Yes;
        this.keep_button.DialogResult = DialogResult.No;
    }
}