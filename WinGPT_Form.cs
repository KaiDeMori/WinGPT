using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using Markdig;
using Markdig.Prism;
using Markdig.SyntaxHighlighting;
using Microsoft.Web.WebView2.Core;
using Newtonsoft.Json.Linq;
using WinGPT.OpenAI;
using WinGPT.Taxonomy;
using Message = WinGPT.OpenAI.Chat.Message;

namespace WinGPT;

public partial class WinGPT_Form : Form {
   //we need a dictionary from Tulpa to RadioButton
   public readonly Dictionary<Tulpa, RadioButton> Tulpa_to_RadioButton = new();

   private BaseDirectoryWatcherAndTreeViewUpdater baseDirectoryWatcherAndTreeViewUpdater;
   private TulpaDirectoryWatcher                  tulpaDirectoryWatcher;

   private readonly TaskCompletionSource<bool> stupid_edge_mumble_mumble = new TaskCompletionSource<bool>();

   private readonly BindingList<FileInfo> Associated_files = new BindingList<FileInfo>();

   public WinGPT_Form() {
      Config.Load();
      InitializeComponent();

      if (Config.Active.WindowParameters != null) {
         this.StartPosition = Config.Active.WindowParameters.StartPosition;
         this.Location      = Config.Active.WindowParameters.Location;
         this.Size          = Config.Active.WindowParameters.Size;
      }

      Enabled =  false;
      Load    += WinGPT_Form_Load;
      Shown   += WinGPT_Form_Shown;
      Closing += WinGPT_Form_Closing;
      //HandleCreated += (sender, args) => 
      //   set_splitter_state();

      Text += $" v{Assembly.GetExecutingAssembly().GetName().Version} PRE-ALPHA";

      Initialize_Models_MenuItems();
      uploaded_files_comboBox.Items.Clear();
      uploaded_files_comboBox.DataSource    = Associated_files;
      uploaded_files_comboBox.DisplayMember = "Name";
      uploaded_files_comboBox.ValueMember   = "FullName";
      //Maybe we should put more init stuff here, instead of Load and Shown 
      prompt_textBox.Font = Config.Active.UIable.Prompt_Font;
   }

   protected override CreateParams CreateParams {
      get {
         CreateParams cp = base.CreateParams;

         if (Config.Active.WindowParameters?.WindowState == FormWindowState.Maximized) {
            cp.Style |= 0x01000000; // WS_MAXIMIZE
         }

         return cp;
      }
   }


   #region stay in Form

   private void WinGPT_Form_Load(object? sender, EventArgs e) {
      webView21.CoreWebView2InitializationCompleted +=
         WebView_CoreWebView2InitializationCompleted;
      webView21.NavigationStarting += CoreWebView2_NavigationStarting;

      //this makes sure our webview gets initialized
      //webView21.Source = new Uri("about:blank");
      webView21.EnsureCoreWebView2Async().ContinueWith(_ =>
         webView21.Invoke(Finally_the_stupid_edge_is_finished_initializing_and_we_can_actually_do_something));
   }

   private async void WinGPT_Form_Shown(object? sender, EventArgs e) {
      set_splitter_state();

      Startup.AssertPrerequisitesOrFail(PromptUserForBaseDirectory);
      Template_Engine.Init();
      //AGI.Init();
      HTTP_Client.Init(Config.Active.OpenAI_API_Key);
      Config.Active.TokenCounter.LimitReached = () => MessageBox.Show("Token limit reached.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

      sysmsghack_ToolStripMenuItem.Checked = Config.Active.UseSysMsgHack;

      //var success = stupid_edge_mumble_mumble.WaitOne(TimeSpan.FromSeconds(10));
      //if (!success) {
      //   MessageBox.Show("Edge is taking too long to initialize. Please restart the program.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      //   Application.Exit();
      //}

      await stupid_edge_mumble_mumble.Task;
      Enabled = true;

      tulpaDirectoryWatcher = new TulpaDirectoryWatcher(CreateTulpaButtons_safe);

      //baseDirectoryWatcher = new BaseDirectoryWatcher(Config.History_Directory, conversation_history_treeView);
      //baseDirectoryWatcher.InitializeTree();
      baseDirectoryWatcherAndTreeViewUpdater =
         new BaseDirectoryWatcherAndTreeViewUpdater(
            Config.History_Directory,
            conversation_history_treeView
         );


      Busy(false);

      //TADA should be a user-set default in the Settings
      //select and activate the default tulpa

      if (Debugger.IsAttached) {
         prompt_textBox.Text = "What is bigger than a town?";
      }
   }

   private void WinGPT_Form_Closing(object? sender, CancelEventArgs e) {
      //maybe there is a better "definition" of Last Used
      if (Config.ActiveTulpa.File != null)
         Config.Active.LastUsedTulpa = Config.ActiveTulpa.File.Name;

      baseDirectoryWatcherAndTreeViewUpdater.treeViewPersistor.Save();

      save_splitter_state();

      Config.Active.WindowParameters = new WindowParameters {
         StartPosition = this.StartPosition,
         WindowState   = this.WindowState,
         Location      = this.Location,
         Size          = this.Size,
      };

      //this seems redundant, but better "Save" than sorry :P
      Config.Save();
   }

   #endregion

   #region Top Billing

   private async void send_prompt_button_Click(object sender, EventArgs e) {
      Busy(true);
      string prompt = prompt_textBox.Text;

      Message user_message = new() {
         role    = Role.user,
         content = prompt,
      };

      if (Conversation.Active == null) {
         //We have a new conversation and need to save it prelimiary
         Conversation.Create_Conversation(user_message, Config.ActiveTulpa);
      }

      Conversation.Active.useSysMsgHack = sysmsghack_ToolStripMenuItem.Checked;
      var response_message = await Config.ActiveTulpa.SendAsync(user_message, Conversation.Active, Associated_files.ToArray());

      if (response_message is null) {
         //we got an error.
         Busy(false);
         return;
      }

      if (autoclear_checkBox.Checked)
         prompt_textBox.Clear();

      Conversation.Update_Conversation(user_message, response_message);
      Update_Conversation();
   }

   private void conversation_history_treeView_AfterSelect(object sender, TreeViewEventArgs e) {
      if (e.Node?.Tag is not FileInfo selectedFile)
         return;
      //TADA later, we will have to find the tulpa that corresponds to the file in the conversation history

      ResetUI();

      if (Conversation.Load(selectedFile))
         Update_Conversation();
      else
         MessageBox.Show($"Could not load {selectedFile.FullName}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
   }

   private void new_conversation_button_Click(object sender, EventArgs e) {
      conversation_history_treeView.SelectedNode = null;
      Conversation.Clear();
      ResetUI();
   }

   #endregion

   #region All the clickys

   private void settings_ToolStripMenuItem_Click(object sender, EventArgs e) {
      var uiconfig = new Config_UI(Config.Active.UIable);
      uiconfig.ShowDialog();
      //set the font
      prompt_textBox.Font = Config.Active.UIable.Prompt_Font;
      Config.Save();
   }

   private void about_ToolStripMenuItem_Click(object sender, EventArgs e) {
      new AboutBox().ShowDialog();
   }

   private void openai_api_key_toolStripMenuItem_Click(object sender, EventArgs e) {
      var dialogResult = new API_KEY_Form().ShowDialog();
      if (dialogResult == DialogResult.OK) {
         HTTP_Client.Init(Config.Active.OpenAI_API_Key);
      }
   }

   private void tokenCounterToolStripMenuItem_Click(object sender, EventArgs e) {
      //show the TokenCounter dialog
      var dialogResult = new TokenCounter_Form().ShowDialog();
      if (dialogResult == DialogResult.OK)
         Config.Save();
   }

   private void base_directory_toolStripMenuItem_Click(object sender, EventArgs e) {
      Startup.UpdateBaseDirectory();
   }

   private void sysmsghack_ToolStripMenuItem_Click(object sender, EventArgs e) {
      Config.Active.UseSysMsgHack = sysmsghack_ToolStripMenuItem.Checked;
      Config.Save();
   }

   private void conversation_name_textBox_MouseDoubleClick(object sender, MouseEventArgs e) {
      // Disable the textbox
      history_file_name_textBox.Enabled = false;
      if (Conversation.Active == null) {
         return;
      }

      // Run the task
      ((Task) Task.Run(() =>
         Taxonomer.taxonomize(Conversation.Active))).ContinueWith(t => {
         // Once the task is done, update the UI on the UI thread
         if (t.IsFaulted) {
            // Handle exception here
            // For example, t.Exception.InnerException
         }
         else if (t.IsCompletedSuccessfully) {
            Invoke(show_conversation_info);
         }
      });
   }

   private void prompt_textBox_KeyDown(object sender, KeyEventArgs e) {
      if (e is {Control: true, KeyCode: Keys.Enter}) {
         send_prompt_button_Click(sender, e);
      }
   }

   private void conversation_name_textBox_KeyDown(object sender, KeyEventArgs e) {
      if (e is {Shift: false, Control: false, KeyCode: Keys.Enter}) {
         e.SuppressKeyPress = true;
         var newName = history_file_name_textBox.Text;
         //try renaming it, if not possible, revert to the old name and show a message
         if (Conversation.Active == null)
            return;
         var conversation = Conversation.Active;

         FileUpdateLocationResult renameResult = Conversation.TryRenameFile(newName);
         show_conversation_info(renameResult);
         //Conversation.ShowError(renameResult);
      }
   }

   private void upload_button_Click(object sender, EventArgs e) {
      upload_vistaOpenFileDialog.Multiselect                  = true;
      upload_vistaOpenFileDialog.CheckFileExists              = true;
      upload_vistaOpenFileDialog.SupportMultiDottedExtensions = true;
      //example filter:
      // "Text files (*.txt)|*.txt|Images (*.png, *.jpg)|*.png;*.jpg|All files (*.*)|*.*"
      upload_vistaOpenFileDialog.Filter =
         "All|*.*|Markdown files (*.md)|*.md|Code|*.cs;*.json;*.py";

      DialogResult result = upload_vistaOpenFileDialog.ShowDialog(this);

      if (result == DialogResult.OK) {
         foreach (string file in upload_vistaOpenFileDialog.FileNames) {
            FileInfo fileInfo = new FileInfo(file);
            if (fileInfo.Exists) {
               Associated_files.Add(fileInfo);
            }
         }
      }
   }

   private void clear_button_Click(object sender, EventArgs e) {
      prompt_textBox.Clear();
   }

   private void conversation_history_treeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e) {
      // Check if the clicked node is the root node
      if (e.Node == conversation_history_treeView.TopNode && e.Node.Tag is DirectoryInfo directoryInfo) {
         var psi = new System.Diagnostics.ProcessStartInfo() {
            FileName        = directoryInfo.FullName,
            UseShellExecute = true
         };
         System.Diagnostics.Process.Start(psi);
      }
   }

   private void conversation_history_treeView_BeforeCollapse(object sender, TreeViewCancelEventArgs e) {
      // Prevent collapsing when the root node is double clicked
      if (e.Node == conversation_history_treeView.TopNode && e.Action == TreeViewAction.Collapse) {
         e.Cancel = true;
      }
   }

   private void del_button_Click(object sender, EventArgs e) {
      if (uploaded_files_comboBox.SelectedItem is FileInfo fileInfo)
         Associated_files.Remove(fileInfo);
   }

   #endregion

   #region UI affairs

   public string PromptUserForBaseDirectory() {
      if (base_directory_vistaFolderBrowserDialog.ShowDialog() == DialogResult.OK)
         return base_directory_vistaFolderBrowserDialog.SelectedPath;
      return string.Empty;
   }

   private void Update_Conversation() {
      if (Conversation.Active == null)
         throw new Exception("No active conversation!");

      var conversation = Conversation.Active;
      //we want the last used one
      conversation.Info.TulpaFile = Config.ActiveTulpa.File.Name;

      //history_file_name_textBox.Text = conversation.HistoryFile.Name;
      //main_toolTip.SetToolTip(history_file_name_textBox, conversation.Info.Summary ?? "no summary yet");

      if (conversation.taxonomy_required) {
         var updateLocationResult = Taxonomer.taxonomize(conversation);
         show_conversation_info(updateLocationResult);
         //if (updateLocationResult == FileUpdateLocationResult.SuccessWithRename)
         //   main_toolStripStatusLabel.Text = Conversation.ErrorMessages[updateLocationResult];
         baseDirectoryWatcherAndTreeViewUpdater.SelectNode(conversation.HistoryFile);
      }
      else {
         conversation.Save();
         show_conversation_info();
      }

      Show_markf278down();

      Busy(false);

      prompt_textBox.Focus();
   }

   private void ActivateTulpa(Tulpa tulpa) {
      SetCharacterTextBox(tulpa);
      SetActiveTulpaAndSaveConversation(tulpa);
   }

   private void Show_markf278down() {
      if (Conversation.Active == null)
         throw new Exception("No active conversation!");
      var conversation = Conversation.Active;
      var markf278down = conversation.Create_markf278down();
      response_textBox.Text = markf278down;

      //now we want to use markdig to transform the messages to html
      var pipeline = new MarkdownPipelineBuilder()
         //.Configure("typographer")
         .UseAdvancedExtensions()
         .UseEmojiAndSmiley()
         .UseEmphasisExtras()
         //.UseSyntaxHighlighting()
         .UseSmartyPants()
         //.UseTaskLists()
         //.UseTypographer()
         .UsePrism()
         .Build();


      var html_fragment = Markdown.ToHtml(markf278down, pipeline);
      var htmlFromFile  = Template_Engine.CreateFullHtml_FromFile(html_fragment);

      //DRAGONS be gone!
      if (Debugger.IsAttached)
         File.WriteAllText("PAGE.HTML", htmlFromFile);

      webView21.NavigateToString(htmlFromFile);
   }

   private void SetCharacterTextBox(Tulpa tulpa) {
      character_textBox.Text     = tulpa.Configuration.Name;
      character_textBox.Enabled  = true;
      character_textBox.ReadOnly = true;
      main_toolTip.SetToolTip(character_textBox, tulpa.Configuration.Description);
   }

   private void SetActiveTulpaAndSaveConversation(Tulpa tulpa) {
      Config.ActiveTulpa = tulpa;

      if (string.IsNullOrWhiteSpace(prompt_textBox.Text)) {
         var last_message = tulpa.Messages.LastOrDefault();

         if (last_message is not null && last_message.role == Role.user) {
            prompt_textBox.Text = last_message.content;
         }
         else {
            prompt_textBox.Text = string.Empty;
         }
      }

      if (Conversation.Active is not null) {
         Conversation.Active.Info.TulpaFile = tulpa.File.Name;
         Conversation.Active.Save();
      }
   }

   private void Busy(bool isBusy) {
      main_toolStripProgressBar.Visible = isBusy;
      if (main_toolStripStatusLabel.Text.Length < 10)
         main_toolStripStatusLabel.Text = isBusy ? "Busy" : "Ready";
      //Enabled                           = !isBusy;
      foreach (Control c in this.Controls) {
         c.Enabled = !isBusy;
      }
   }

   private void ResetUI() {
      prompt_textBox.Clear();
      history_file_name_textBox.Clear();
      response_textBox.Clear();
      //webView21.NavigateToString(string.Empty); //works
      //webView21.Source = new Uri("about:blank"); //no works
      webView21.CoreWebView2.Navigate("about:blank"); //works
      prompt_textBox.Focus();
   }

   private void show_conversation_info() {
      if (Conversation.Active == null)
         throw new NullReferenceException("Conversation.Active is null");

      history_file_name_textBox.Text = Conversation.Active.HistoryFile.Name;
      main_toolTip.SetToolTip(history_file_name_textBox, Conversation.Active.Info.Summary ?? "no summary yet");
      history_file_name_textBox.Enabled = true;

      //DRAGONS maybe this is the right place to update the treeview?
      //this seems to work, but I dont get why.
      //Shouldn't this loop infinitely at some point?
      baseDirectoryWatcherAndTreeViewUpdater?.SelectNode(Conversation.Active.HistoryFile);
   }

   private void show_conversation_info(FileUpdateLocationResult result) {
      var errorMessage = Conversation.ErrorMessages[result];

      switch (result) {
         case FileUpdateLocationResult.Success:
         case FileUpdateLocationResult.UserAborted:
            break;
         case FileUpdateLocationResult.SuccessWithRename:
            main_toolStripStatusLabel.Text = errorMessage;
            break;
         default:
            MessageBox.Show(errorMessage, "File Move Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            break;
      }

      show_conversation_info();
   }

   #endregion

   #region Init et al.

   public void CreateTulpaButtons_safe(List<Tulpa> tulpas, Tulpa? selected_tulpa) {
      if (tulpas_flowLayoutPanel.InvokeRequired) {
         tulpas_flowLayoutPanel.Invoke(() => CreateTulpaButtons(tulpas, selected_tulpa));
      }
      else {
         CreateTulpaButtons(tulpas, selected_tulpa);
      }
   }

   private void CreateTulpaButtons(List<Tulpa> tulpas, Tulpa? selected_tulpa) {
      if (tulpas.Count == 0) {
         MessageBox.Show("No tulpas found!\r\nPlease select a valid base directory!", "Error", MessageBoxButtons.OK);
         //Application.Exit();
         return;
      }

      selected_tulpa ??= Config.ActiveTulpa;

      ////////////////////////
      tulpas_flowLayoutPanel.SuspendLayout();

      tulpas_flowLayoutPanel.Controls.Clear();
      Tulpa_to_RadioButton.Clear();

      foreach (var tulpa in tulpas) {
         var button = new RadioButton {
            Text       = tulpa.Configuration.Name,
            Checked    = false,
            AutoSize   = true,
            Appearance = Appearance.Button,
         };
         //set the button tooltip to tulpa.Configuration.Description
         main_toolTip.SetToolTip(button, tulpa.Configuration.Description);
         //add a click event that receives the button and the tulpa
         button.Click += (sender, args) => { ActivateTulpa(tulpa); };

         tulpas_flowLayoutPanel.Controls.Add(button);
         Tulpa_to_RadioButton.Add(tulpa, button);
      }

      Tulpa_to_RadioButton[selected_tulpa].Checked = true;

      tulpas_flowLayoutPanel.ResumeLayout();
      ////////////////////////

      ActivateTulpa(selected_tulpa);
   }

   private void Initialize_Models_MenuItems() {
      models_ToolStripMenuItem.DropDownItems.Clear();
      foreach (var model in Models.Supported) {
         var item = new ToolStripMenuItem(model);
         item.Click += (sender, args) => {
            Config.Active.LanguageModel = model;
            Config.Save();
            foreach (ToolStripMenuItem oneitem in models_ToolStripMenuItem.DropDownItems)
               oneitem.Checked = oneitem == item;
         };
         models_ToolStripMenuItem.DropDownItems.Add(item);
      }

      //now we have to set the checked property of the correct menu item
      foreach (ToolStripMenuItem item in models_ToolStripMenuItem.DropDownItems) {
         if (item.Text == Config.Active.LanguageModel) {
            item.Checked = true;
         }
      }
   }

   /// <summary>
   /// This saves the relative position of the splitters.
   /// </summary>
   private void save_splitter_state() {
      var main_panel_absolute_width = main_panel.Width;
      var treeview_absolute_width   = conversation_history_treeView.Width;
      var treeview_relative_width   = (double) treeview_absolute_width / main_panel_absolute_width;
      Config.Active.MainSplitter_relative_position = treeview_relative_width;

      var text_splitter_total_width  = text_splitContainer.Width;
      var text_splitter_abs_position = text_splitContainer.SplitterDistance;
      var text_splitter_rel_position = (double) text_splitter_abs_position / text_splitter_total_width;
      Config.Active.TextSplitter_relative_position = text_splitter_rel_position;
   }

   /// <summary>
   /// This sets the relative position of the splitters.
   /// </summary>
   private void set_splitter_state() {
      //check if Config.Active.MainSplitter_relative_position is between 0 and 1
      //if not, set it to 0.2
      if (Config.Active.MainSplitter_relative_position < 0 || Config.Active.MainSplitter_relative_position > 1)
         Config.Active.MainSplitter_relative_position = 0.2;

      //check if Config.Active.TextSplitter_relative_position is between 0 and 1
      //if not, set it to 0.5
      if (Config.Active.TextSplitter_relative_position < 0 || Config.Active.TextSplitter_relative_position > 1)
         Config.Active.TextSplitter_relative_position = 0.5;

      var main_panel_absolute_width = main_panel.Width;
      var treeview_absolute_width   = (int) (main_panel_absolute_width * Config.Active.MainSplitter_relative_position);
      conversation_history_treeView.Width = treeview_absolute_width;

      var text_splitter_total_width  = text_splitContainer.Width;
      var text_splitter_abs_position = (int) (text_splitter_total_width * Config.Active.TextSplitter_relative_position);
      text_splitContainer.SplitterDistance = text_splitter_abs_position;
   }

   #endregion

   #region all the non-sensical edge stuff

   private void Finally_the_stupid_edge_is_finished_initializing_and_we_can_actually_do_something() {
      webView21.CoreWebView2.Navigate("about:blank");

      stupid_edge_mumble_mumble.TrySetResult(true);

      //For now, just create a new Conversation
      //TADA: load the last conversation
   }

   private void WebView_CoreWebView2InitializationCompleted(object? sender, CoreWebView2InitializationCompletedEventArgs e) {
      var settings = webView21.CoreWebView2.Settings;
      settings.AreDefaultContextMenusEnabled    = false;
      settings.AreDevToolsEnabled               = false;
      settings.IsStatusBarEnabled               = false;
      settings.AreBrowserAcceleratorKeysEnabled = false;
      settings.IsBuiltInErrorPageEnabled        = false;
      settings.IsReputationCheckingRequired     = false;
      settings.IsSwipeNavigationEnabled         = false;

      //webView21.CreationProperties.AdditionalBrowserArguments = "--disable-web-security --allow-file-access-from-files --allow-file-access --allow-file-access-from-files --allow-universal-acce"

      webView21.AllowExternalDrop = false;

      webView21.CoreWebView2.ContextMenuRequested += CoreWebView2_ContextMenuRequested;
      webView21.CoreWebView2.WebMessageReceived   += CoreWebView2_WebMessageReceived;
   }

   private void CoreWebView2_WebMessageReceived(object? sender, CoreWebView2WebMessageReceivedEventArgs e) {
      var json_from_browser = e.WebMessageAsJson;
      //string dataFromJavaScript = e.TryGetWebMessageAsString();
      Debug.WriteLine(json_from_browser);
      var message_from_browser = JObject.Parse(json_from_browser);

      var file_name    = message_from_browser["file_name"]?.ToString();
      var code_content = message_from_browser["code_content"]?.ToString();

      if (file_name is not null && code_content is not null) {
         //find the file in the Associated_files
         var fileinfo = Associated_files.FirstOrDefault(f => f.Name == file_name);
         if (fileinfo is not null && fileinfo.Exists) {
            //write the code_content to the file
            File.WriteAllText(fileinfo.FullName, code_content);
            Invoke(() => main_toolStripStatusLabel.Text = $"File {fileinfo.Name} saved to {fileinfo.DirectoryName}");
         }
      }
   }

   private void CoreWebView2_ContextMenuRequested(object? sender, CoreWebView2ContextMenuRequestedEventArgs args) {
      IList<CoreWebView2ContextMenuItem> menuList = args.MenuItems;
      CoreWebView2ContextMenuTargetKind  context  = args.ContextMenuTarget.Kind;

      HashSet<string> namesToRemove = new() {
         "back", "forward", "reload", "other", "share"
      };
      for (int index = menuList.Count - 1; index >= 0; index--) {
         var menuItem = menuList[index];
         if (namesToRemove.Contains(menuItem.Name)) {
            menuList.RemoveAt(index);
         }
      }
   }

   private void CoreWebView2_NavigationStarting(object sender, CoreWebView2NavigationStartingEventArgs e) {
      if (e.IsUserInitiated)
         e.Cancel = true;
   }

   #endregion
}