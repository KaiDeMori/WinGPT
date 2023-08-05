namespace WinGPT; 

public partial class Config_UI : Form
{
   public Config_UI(Config_UIable config) {
      InitializeComponent();
      config_ui_propertyGrid.SelectedObject = config;
   }

}