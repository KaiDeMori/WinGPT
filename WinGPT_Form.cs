using System.Diagnostics;
using System.Reflection;
using WinGPT.OpenAI;
using Message = WinGPT.OpenAI.Chat.Message;

namespace WinGPT;

public partial class WinGPT_Form : Form
{
    public WinGPT_Form()
    {
        InitializeComponent();
        //Add assembly version to the title
        Text += $" v{Assembly.GetExecutingAssembly().GetName().Version} PRE-ALPHA";
        Enabled = false;
        Shown += WinGPT_Form_Shown;
        Initialize_Models_MenuItems();
    }

    private void Initialize_Models_MenuItems()
    {
        models_ToolStripMenuItem.DropDownItems.Clear();
        foreach (var model in Models.Supported)
        {
            var item = new ToolStripMenuItem(model);
            item.Click += (sender, args) => { Config.Active.LanguageModel = model; };
            models_ToolStripMenuItem.DropDownItems.Add(item);
        }

        //now we have to set the checked property of the correct menu item
        foreach (ToolStripMenuItem item in models_ToolStripMenuItem.DropDownItems)
        {
            if (item.Text == Config.Active.LanguageModel)
            {
                item.Checked = true;
            }
        }
    }

    private void WinGPT_Form_Shown(object? sender, EventArgs e)
    {
        Config.Load();

        Startup.AssertPrerequisitesOrFail(PromptUserForBaseDirectory);
        HTTP_Client.Init(Config.Active.OpenAI_API_Key);
        Config.Active.TokenCounter.LimitReached = () => MessageBox.Show("Token limit reached.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        sysmsghack_ToolStripMenuItem.Checked = Config.Active.UseSysMsgHack;

        var tulpas = Startup.CreateAllTulpas();
        if (tulpas.Count == 0)
        {
            MessageBox.Show("No tulpas found. Nothing to do here.", "Error", MessageBoxButtons.OK);
            Application.Exit();
        }

        CreateCharacterButtons(tulpas);

        //create directoryinfo for the conversation history directory
        DirectoryInfo history_location = new(
           Path.Join(
              Config.Active.BaseDirectory,
              Config.conversation_history_directory));
        //ConversationHistoryTraverser.StartWatching(history_location, conversation_history_treeView);
        baseDirectoryWatcher = new BaseDirectoryWatcher(history_location, conversation_history_treeView);
        baseDirectoryWatcher.InitializeTree();

        //TADA should be a user-set default in the Settings
        //select and activate the first tulpa
        var selected_tulpa = tulpas[0];
        Tulpa_to_RadioButton[selected_tulpa].Checked = true;
        ActivateTulpa(selected_tulpa);

        //For now, just create a new Conversation
        //TADA: load the last conversation
        var conversation = selected_tulpa.NewConversation();
        ActivateConversation(conversation);

        text_splitContainer.SplitterDistance = text_splitContainer.Width / 2;

        main_toolStripStatusLabel.Text = "Ready";
        main_toolStripProgressBar.Visible = false;

        Enabled = true;

        //check if we are in debug mode
        if (Debugger.IsAttached)
        {
            prompt_textBox.Text = "What is bigger than a town?";
        }
    }

    private void ActivateConversation(Conversation conversation)
    {
        Config.ActiveConversation = conversation;
        conversation.Info.TulpaFile = Config.ActiveTulpa.File.Name;
        response_textBox.Clear();
        prompt_textBox.Clear();

        var uiText = conversation.Create_UI_Text();
        response_textBox.Text = uiText;
        conversation_name_textBox.Text = conversation.Info.Name;
        Config.ActiveTulpa.Activate(conversation);
    }

    //we need a dictionary from Tulpa to RadioButton
    public readonly Dictionary<Tulpa, RadioButton> Tulpa_to_RadioButton = new();
    private BaseDirectoryWatcher? baseDirectoryWatcher;

    private void CreateCharacterButtons(List<Tulpa> tulpas)
    {
        characters_flowLayoutPanel.Controls.Clear();
        Tulpa_to_RadioButton.Clear();

        foreach (var tulpa in tulpas)
        {
            var button = new RadioButton
            {
                Text = tulpa.Configuration.Name,
                Checked = false,
                AutoSize = true,
                Appearance = Appearance.Button,
            };
            //set the button tooltip to tulpa.Configuration.Description
            main_toolTip.SetToolTip(button, tulpa.Configuration.Description);
            //add a click event that receives the button and the tulpa
            button.Click += (sender, args) => { ActivateTulpa(tulpa); };

            characters_flowLayoutPanel.Controls.Add(button);
            Tulpa_to_RadioButton.Add(tulpa, button);
        }
    }

    private void ActivateTulpa(Tulpa tulpa)
    {
        character_textBox.Text = tulpa.Configuration.Name;
        character_textBox.Enabled = true;
        character_textBox.ReadOnly = true;
        main_toolTip.SetToolTip(character_textBox, tulpa.Configuration.Description);
        tulpa.Activate(Config.ActiveConversation);
        Config.ActiveConversation.Info.TulpaFile = tulpa.File.Name;
        Config.ActiveTulpa = tulpa;
    }

    public string PromptUserForBaseDirectory()
    {
        if (base_directory_vistaFolderBrowserDialog.ShowDialog() == DialogResult.OK)
            return base_directory_vistaFolderBrowserDialog.SelectedPath;
        return string.Empty;
    }

    private void about_ToolStripMenuItem_Click(object sender, EventArgs e)
    {
        new AboutBox().ShowDialog();
    }

    private void openai_api_key_toolStripMenuItem_Click(object sender, EventArgs e)
    {
        var dialogResult = new API_KEY_Form().ShowDialog();
        if (dialogResult == DialogResult.OK)
        {
            HTTP_Client.Init(Config.Active.OpenAI_API_Key);
        }
    }

    private void tokenCounterToolStripMenuItem_Click(object sender, EventArgs e)
    {
        //show the TokenCounter dialog
        var dialogResult = new TokenCounter_Form().ShowDialog();
        if (dialogResult == DialogResult.OK)
            Config.Save();
    }

    private async void send_prompt_button_Click(object sender, EventArgs e)
    {
        string prompt = prompt_textBox.Text;
        Message user_message = new()
        {
            role = Role.user,
            content = prompt,
        };

        Config.ActiveConversation.useSysMsgHack = sysmsghack_ToolStripMenuItem.Checked;

        List<Message> responses = await Config.ActiveTulpa.SendAsync(user_message, Config.ActiveConversation);
        Config.ActiveConversation.Messages.Add(user_message);
        Config.ActiveConversation.Messages.AddRange(responses);
        var messages = string.Join("", responses);
        response_textBox.AppendText(user_message.ToString());
        response_textBox.AppendText(messages);

        Config.ActiveConversation.Save();
    }

    private void new_conversation_button_Click(object sender, EventArgs e)
    {
        var newConversation = Config.ActiveTulpa.NewConversation();
        ActivateConversation(newConversation);
    }

    private void conversation_history_treeView_AfterSelect(object sender, TreeViewEventArgs e)
    {
        if (e.Node?.Tag is not FileInfo selectedFile)
            return;

        if (!HistoryFileParser.TryParseConversationHistoryFile(selectedFile, out Conversation? conversation))
        {
            return;
        }

        //TADA later, we will have to find the tulpa that corresponds to the file in the conversation history

        ActivateConversation(conversation);
    }

    private void base_directory_toolStripMenuItem_Click(object sender, EventArgs e)
    {
        Startup.UpdateBaseDirectory();
    }

    private void sysmsghack_ToolStripMenuItem_Click(object sender, EventArgs e)
    {
        Config.Active.UseSysMsgHack = sysmsghack_ToolStripMenuItem.Checked;
        Config.Save();
    }

    private void conversation_name_textBox_MouseDoubleClick(object sender, MouseEventArgs e)
    {
        // Disable the textbox
        conversation_name_textBox.Enabled = false;

        // Run the task
        Task.Run(Config.ActiveConversation.CreateTitleAsync)
           .ContinueWith(t =>
           {
               // Once the task is done, update the UI on the UI thread
               if (t.IsFaulted)
               {
                   // Handle exception here
                   // For example, t.Exception.InnerException
               }
               else if (t.IsCompletedSuccessfully)
               {
                   Invoke(() =>
                   {
                       conversation_name_textBox.Text = Config.ActiveConversation.Info.Name;
                       conversation_name_textBox.Enabled = true; // Re-enable the textbox
                   });
               }
           });
    }

    private void prompt_textBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e is { Control: true, KeyCode: Keys.Enter })
        {
            send_prompt_button_Click(sender, e);
        }
    }

    private void conversation_name_textBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e is { Shift: false, Control: false, KeyCode: Keys.Enter })
        {
            e.SuppressKeyPress = true;
            var newName = conversation_name_textBox.Text;
            //try renaming it, if not possible, revert to the old name and show a message
            if (!Config.ActiveConversation.TryRenameConversationAndFile(newName))
            {
                conversation_name_textBox.Text = Config.ActiveConversation.Info.Name;
                MessageBox.Show("Could not rename the conversation. Please try again.");
            }
        }
    }
}