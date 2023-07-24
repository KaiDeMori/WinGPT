using System.Diagnostics;
using System.Reflection;
using CefSharp;
using Markdig;
using Markdig.Prism;
using Markdig.SyntaxHighlighting;
using Microsoft.VisualBasic;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
using WinGPT.OpenAI;
using WinGPT.Taxonomy;
using Message = WinGPT.OpenAI.Chat.Message;

namespace WinGPT;

public partial class WinGPT_Form : Form {
   public WinGPT_Form() {
      Config.Load();
      InitializeComponent();
      Enabled =  false;
      Load    += WinGPT_Form_Load;
      Shown   += WinGPT_Form_Shown;

      Text += $" v{Assembly.GetExecutingAssembly().GetName().Version} PRE-ALPHA";

      Initialize_Models_MenuItems();
   }

   private void WinGPT_Form_Load(object? sender, EventArgs e) {
      webView21.CoreWebView2InitializationCompleted +=
         WebView_CoreWebView2InitializationCompleted;
      webView21.NavigationStarting += CoreWebView2_NavigationStarting;

      //this makes sure our webview gets initialized
      webView21.Source = new Uri("about:blank");
      webView21.EnsureCoreWebView2Async().ContinueWith(task =>
         webView21.Invoke(() => {
            //For now, just create a new Conversation
            //TADA: load the last conversation
            //var conversation = Config.ActiveTulpa.NewConversation();

            //ActivateConversation(conversation);
            Enabled = true;
         }));
   }

   private void WebView_CoreWebView2InitializationCompleted(object? sender, CoreWebView2InitializationCompletedEventArgs e) {
      var settings = webView21.CoreWebView2.Settings;
      //settings.AreDefaultContextMenusEnabled = false;
      //settings.AreDevToolsEnabled               = false;
      settings.IsStatusBarEnabled               = false;
      settings.AreBrowserAcceleratorKeysEnabled = false;
      settings.IsBuiltInErrorPageEnabled        = false;
      settings.IsReputationCheckingRequired     = false;
      settings.IsSwipeNavigationEnabled         = false;

      //webView21.CreationProperties.AdditionalBrowserArguments = "--disable-web-security --allow-file-access-from-files --allow-file-access --allow-file-access-from-files --allow-universal-acce"

      webView21.AllowExternalDrop = false;

      webView21.CoreWebView2.ContextMenuRequested += CoreWebView2_ContextMenuRequested;
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

   private void WinGPT_Form_Shown(object? sender, EventArgs e) {
      Startup.AssertPrerequisitesOrFail(PromptUserForBaseDirectory);
      Template_Engine.Init();
      //AGI.Init();
      HTTP_Client.Init(Config.Active.OpenAI_API_Key);
      Config.Active.TokenCounter.LimitReached = () => MessageBox.Show("Token limit reached.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

      sysmsghack_ToolStripMenuItem.Checked = Config.Active.UseSysMsgHack;

      var tulpas = Startup.CreateAllTulpas();
      if (tulpas.Count == 0) {
         MessageBox.Show("No tulpas found!\r\nPlease select a valid base directory!", "Error", MessageBoxButtons.OK);
         //Application.Exit();
      }
      else {
         CreateCharacterButtons(tulpas);
         var selected_tulpa =
            tulpas.FirstOrDefault(tulpa => tulpa.File.Name == "Default_Assistant.md")
            ?? tulpas[0];

         Tulpa_to_RadioButton[selected_tulpa].Checked = true;
         ActivateTulpa(selected_tulpa);
      }


      //TADA can that go?
      //ConversationHistoryTraverser.StartWatching(Config.History_Directory, conversation_history_treeView);

      //baseDirectoryWatcher = new BaseDirectoryWatcher(Config.History_Directory, conversation_history_treeView);
      //baseDirectoryWatcher.InitializeTree();

      baseDirectoryWatcherAndTreeViewUpdater =
         new BaseDirectoryWatcherAndTreeViewUpdater(
            Config.History_Directory,
            conversation_history_treeView
         );

      //TADA should be a user-set default in the Settings
      //select and activate the default tulpa


      text_splitContainer.SplitterDistance = text_splitContainer.Width / 2;

      Busy(false);

      //check if we are in debug mode
      if (Debugger.IsAttached) {
         prompt_textBox.Text = "What is bigger than a town?";
      }
   }

   private void Update_Conversation() {
      if (Conversation.Active == null)
         throw new Exception("No active conversation!");

      var conversation = Conversation.Active;
      //we want the last used one
      conversation.Info.TulpaFile = Config.ActiveTulpa.File.Name;

      history_file_name_textBox.Text = conversation.HistoryFile.Name;
      main_toolTip.SetToolTip(history_file_name_textBox, conversation.Info.Summary ?? "no summary yet");

      if (conversation.taxonomy_required) {
         var updateLocationResult = Taxonomer.taxonomize(conversation);
         if (updateLocationResult == FileUpdateLocationResult.SuccessWithRename)
            main_toolStripStatusLabel.Text = Conversation.ErrorMessages[updateLocationResult];
         baseDirectoryWatcherAndTreeViewUpdater.SelectNode(conversation.HistoryFile);
      }
      else
         conversation.Save();

      Conversation.Load(new FileInfo(""));
      
      Show_markf278down();

      Busy(false);
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

   //we need a dictionary from Tulpa to RadioButton
   public readonly Dictionary<Tulpa, RadioButton>         Tulpa_to_RadioButton = new();
   private         BaseDirectoryWatcher?                  baseDirectoryWatcher;
   private         BaseDirectoryWatcherAndTreeViewUpdater baseDirectoryWatcherAndTreeViewUpdater;

   private void CreateCharacterButtons(List<Tulpa> tulpas) {
      characters_flowLayoutPanel.Controls.Clear();
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

         characters_flowLayoutPanel.Controls.Add(button);
         Tulpa_to_RadioButton.Add(tulpa, button);
      }
   }

   private void ActivateTulpa(Tulpa tulpa) {
      character_textBox.Text     = tulpa.Configuration.Name;
      character_textBox.Enabled  = true;
      character_textBox.ReadOnly = true;
      main_toolTip.SetToolTip(character_textBox, tulpa.Configuration.Description);
      //DRAGONS not completely sure what to do here.
      Config.ActiveTulpa = tulpa;
      if (Conversation.Active is not null) {
         Conversation.Active.Info.TulpaFile = tulpa.File.Name;
      }
   }

   public string PromptUserForBaseDirectory() {
      if (base_directory_vistaFolderBrowserDialog.ShowDialog() == DialogResult.OK)
         return base_directory_vistaFolderBrowserDialog.SelectedPath;
      return string.Empty;
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
      var responses = await Config.ActiveTulpa.SendAsync(user_message, Conversation.Active);
      if (autoclear_checkBox.Checked)
         prompt_textBox.Clear();
      Conversation.Update_Conversation(user_message, responses);
      Update_Conversation();
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

   /// <summary>
   /// little bit of a misnomer, we dont create a new converstion object,
   /// we just reset the UI to the default state.
   /// </summary>
   /// <param name="sender"></param>
   /// <param name="e"></param>
   private void new_conversation_button_Click(object sender, EventArgs e) {
      ResetUI();
   }

   private void ResetUI() {
      Conversation.Reset();
      prompt_textBox.Clear();
      history_file_name_textBox.Clear();
      response_textBox.Clear();
      //webView21.NavigateToString(string.Empty);
      webView21.Source = new Uri("about:blank");
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

      {
         void Action() {
            Taxonomer.taxonomize(Conversation.Active);
         }

         // Run the task
         Task.Run(Action)
            .ContinueWith(t => {
               // Once the task is done, update the UI on the UI thread
               if (t.IsFaulted) {
                  // Handle exception here
                  // For example, t.Exception.InnerException
               }
               else if (t.IsCompletedSuccessfully) {
                  Invoke(() => {
                     history_file_name_textBox.Text    = Conversation.Active.HistoryFile.Name;
                     history_file_name_textBox.Enabled = true; // Re-enable the textbox
                  });
               }
            });
      }
   }

   private void prompt_textBox_KeyDown(object sender, KeyEventArgs e) {
      if (e is {Control: true, KeyCode: Keys.Enter}) {
         send_prompt_button_Click(sender, e);
      }
   }

   private void conversation_name_textBox_KeyDown
      (object sender, KeyEventArgs e) {
      if (e is {Shift: false, Control: false, KeyCode: Keys.Enter}) {
         e.SuppressKeyPress = true;
         var newName = history_file_name_textBox.Text;
         //try renaming it, if not possible, revert to the old name and show a message
         if (Conversation.Active == null)
            return;
         var conversation = Conversation.Active;

         FileUpdateLocationResult renameResult = Conversation.TryRenameFile(newName);
         history_file_name_textBox.Text = conversation.HistoryFile.Name;
         if (renameResult == FileUpdateLocationResult.Success)
            conversation.Save();
         else
            Conversation.ShowError(renameResult);
      }
   }

   private void scroll_to_browser_bottom() {
      webView21.ExecuteScriptAsync("window.scrollTo(0, document.body.scrollHeight);");
   }

   private void upload_button_Click(object sender, EventArgs e) {
      upload_vistaOpenFileDialog.Multiselect     = true;
      upload_vistaOpenFileDialog.CheckFileExists = true;
      DialogResult result = upload_vistaOpenFileDialog.ShowDialog(this);
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
}