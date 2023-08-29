namespace WinGPT
{
    partial class WinGPT_Form
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            TreeNode treeNode6 = new TreeNode("Node2");
            TreeNode treeNode7 = new TreeNode("Chat1", new TreeNode[] { treeNode6 });
            TreeNode treeNode8 = new TreeNode("Node3");
            TreeNode treeNode9 = new TreeNode("Node4");
            TreeNode treeNode10 = new TreeNode("Conversation History Root", new TreeNode[] { treeNode7, treeNode8, treeNode9 });
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WinGPT_Form));
            main_toolTip = new ToolTip(components);
            history_file_name_textBox = new TextBox();
            autoclear_checkBox = new CheckBox();
            clear_button = new Button();
            text_splitContainer = new SplitContainer();
            toggle_LEFT_button = new Button();
            prompt_textBox = new TextBox();
            prompt_buttons_panel = new Panel();
            remove_file_button = new Button();
            uploaded_files_comboBox = new ComboBox();
            upload_button = new Button();
            send_prompt_button = new Button();
            tulpa_textBox = new TextBox();
            toggle_RIGHT_button = new Button();
            preview_tabControl = new TabControl();
            webview2_tabPage = new TabPage();
            webView21 = new Microsoft.Web.WebView2.WinForms.WebView2();
            markf278down_tabPage = new TabPage();
            response_textBox = new TextBox();
            submit_edits_button = new Button();
            new_conversation_button = new Button();
            conversation_history_treeView = new TreeView();
            main_splitter = new Splitter();
            main_menuStrip = new MenuStrip();
            base_directory_toolStripMenuItem = new ToolStripMenuItem();
            settings_ToolStripMenuItem = new ToolStripMenuItem();
            tools_ToolStripMenuItem = new ToolStripMenuItem();
            openai_api_key_toolStripMenuItem = new ToolStripMenuItem();
            tokenCounter_ToolStripMenuItem = new ToolStripMenuItem();
            sysmsghack_ToolStripMenuItem = new ToolStripMenuItem();
            models_ToolStripMenuItem = new ToolStripMenuItem();
            openConfigDirectory_ToolStripMenuItem = new ToolStripMenuItem();
            help_ToolStripMenuItem = new ToolStripMenuItem();
            about_ToolStripMenuItem = new ToolStripMenuItem();
            main_panel = new Panel();
            tulpas_tableLayoutPanel = new TableLayoutPanel();
            tulpas_flowLayoutPanel = new FlowLayoutPanel();
            placeholder_radioButton = new RadioButton();
            main_statusStrip = new StatusStrip();
            main_toolStripStatusLabel = new ToolStripStatusLabel();
            main_toolStripProgressBar = new ToolStripProgressBar();
            base_directory_vistaFolderBrowserDialog = new Ookii.Dialogs.WinForms.VistaFolderBrowserDialog();
            upload_vistaOpenFileDialog = new Ookii.Dialogs.WinForms.VistaOpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)text_splitContainer).BeginInit();
            text_splitContainer.Panel1.SuspendLayout();
            text_splitContainer.Panel2.SuspendLayout();
            text_splitContainer.SuspendLayout();
            prompt_buttons_panel.SuspendLayout();
            preview_tabControl.SuspendLayout();
            webview2_tabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)webView21).BeginInit();
            markf278down_tabPage.SuspendLayout();
            main_menuStrip.SuspendLayout();
            main_panel.SuspendLayout();
            tulpas_tableLayoutPanel.SuspendLayout();
            tulpas_flowLayoutPanel.SuspendLayout();
            main_statusStrip.SuspendLayout();
            SuspendLayout();
            // 
            // history_file_name_textBox
            // 
            history_file_name_textBox.BackColor = SystemColors.Info;
            history_file_name_textBox.Dock = DockStyle.Top;
            history_file_name_textBox.Location = new Point(12, 12);
            history_file_name_textBox.Name = "history_file_name_textBox";
            history_file_name_textBox.PlaceholderText = "Filename";
            history_file_name_textBox.Size = new Size(330, 23);
            history_file_name_textBox.TabIndex = 3;
            history_file_name_textBox.KeyDown += history_file_name_textBox_KeyDown;
            history_file_name_textBox.Leave += history_file_name_textBox_Leave;
            history_file_name_textBox.MouseDoubleClick += conversation_name_textBox_MouseDoubleClick;
            // 
            // autoclear_checkBox
            // 
            autoclear_checkBox.AutoSize = true;
            autoclear_checkBox.Checked = true;
            autoclear_checkBox.CheckState = CheckState.Checked;
            autoclear_checkBox.Location = new Point(81, 10);
            autoclear_checkBox.Name = "autoclear_checkBox";
            autoclear_checkBox.Size = new Size(78, 19);
            autoclear_checkBox.TabIndex = 6;
            autoclear_checkBox.Text = "auto clear";
            autoclear_checkBox.UseVisualStyleBackColor = true;
            // 
            // clear_button
            // 
            clear_button.Location = new Point(0, 6);
            clear_button.Name = "clear_button";
            clear_button.Size = new Size(75, 25);
            clear_button.TabIndex = 5;
            clear_button.Text = "Clear";
            clear_button.UseVisualStyleBackColor = true;
            clear_button.Click += clear_button_Click;
            // 
            // text_splitContainer
            // 
            text_splitContainer.BackColor = SystemColors.ControlDark;
            text_splitContainer.Dock = DockStyle.Fill;
            text_splitContainer.Location = new Point(161, 37);
            text_splitContainer.Name = "text_splitContainer";
            // 
            // text_splitContainer.Panel1
            // 
            text_splitContainer.Panel1.BackColor = SystemColors.Control;
            text_splitContainer.Panel1.Controls.Add(toggle_LEFT_button);
            text_splitContainer.Panel1.Controls.Add(prompt_textBox);
            text_splitContainer.Panel1.Controls.Add(prompt_buttons_panel);
            text_splitContainer.Panel1.Controls.Add(tulpa_textBox);
            text_splitContainer.Panel1.Padding = new Padding(12);
            // 
            // text_splitContainer.Panel2
            // 
            text_splitContainer.Panel2.BackColor = SystemColors.Control;
            text_splitContainer.Panel2.Controls.Add(toggle_RIGHT_button);
            text_splitContainer.Panel2.Controls.Add(preview_tabControl);
            text_splitContainer.Panel2.Controls.Add(new_conversation_button);
            text_splitContainer.Panel2.Controls.Add(history_file_name_textBox);
            text_splitContainer.Panel2.Padding = new Padding(12);
            text_splitContainer.Size = new Size(721, 324);
            text_splitContainer.SplitterDistance = 360;
            text_splitContainer.SplitterWidth = 7;
            text_splitContainer.TabIndex = 0;
            text_splitContainer.MouseDoubleClick += text_splitContainer_MouseDoubleClick;
            // 
            // toggle_LEFT_button
            // 
            toggle_LEFT_button.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            toggle_LEFT_button.Font = new Font("Consolas", 6F, FontStyle.Regular, GraphicsUnit.Point);
            toggle_LEFT_button.Location = new Point(350, 0);
            toggle_LEFT_button.Margin = new Padding(0);
            toggle_LEFT_button.Name = "toggle_LEFT_button";
            toggle_LEFT_button.Size = new Size(12, 324);
            toggle_LEFT_button.TabIndex = 6;
            toggle_LEFT_button.Text = "<";
            toggle_LEFT_button.UseVisualStyleBackColor = true;
            toggle_LEFT_button.Click += toggle_LEFT_button_Click;
            // 
            // prompt_textBox
            // 
            prompt_textBox.AllowDrop = true;
            prompt_textBox.Dock = DockStyle.Fill;
            prompt_textBox.Location = new Point(12, 35);
            prompt_textBox.Multiline = true;
            prompt_textBox.Name = "prompt_textBox";
            prompt_textBox.PlaceholderText = "Prompt";
            prompt_textBox.ScrollBars = ScrollBars.Both;
            prompt_textBox.Size = new Size(336, 214);
            prompt_textBox.TabIndex = 0;
            prompt_textBox.KeyDown += prompt_textBox_KeyDown;
            // 
            // prompt_buttons_panel
            // 
            prompt_buttons_panel.Controls.Add(clear_button);
            prompt_buttons_panel.Controls.Add(remove_file_button);
            prompt_buttons_panel.Controls.Add(uploaded_files_comboBox);
            prompt_buttons_panel.Controls.Add(upload_button);
            prompt_buttons_panel.Controls.Add(send_prompt_button);
            prompt_buttons_panel.Controls.Add(autoclear_checkBox);
            prompt_buttons_panel.Dock = DockStyle.Bottom;
            prompt_buttons_panel.Location = new Point(12, 249);
            prompt_buttons_panel.Name = "prompt_buttons_panel";
            prompt_buttons_panel.Size = new Size(336, 63);
            prompt_buttons_panel.TabIndex = 3;
            // 
            // remove_file_button
            // 
            remove_file_button.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            remove_file_button.Location = new Point(269, 38);
            remove_file_button.Name = "remove_file_button";
            remove_file_button.Size = new Size(67, 25);
            remove_file_button.TabIndex = 4;
            remove_file_button.Text = "remove";
            remove_file_button.UseVisualStyleBackColor = true;
            remove_file_button.Click += del_button_Click;
            // 
            // uploaded_files_comboBox
            // 
            uploaded_files_comboBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            uploaded_files_comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            uploaded_files_comboBox.FormattingEnabled = true;
            uploaded_files_comboBox.Items.AddRange(new object[] { "File 1", "File 2", "File 3.md" });
            uploaded_files_comboBox.Location = new Point(101, 39);
            uploaded_files_comboBox.Name = "uploaded_files_comboBox";
            uploaded_files_comboBox.Size = new Size(162, 23);
            uploaded_files_comboBox.TabIndex = 3;
            // 
            // upload_button
            // 
            upload_button.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            upload_button.FlatStyle = FlatStyle.System;
            upload_button.Location = new Point(0, 38);
            upload_button.Name = "upload_button";
            upload_button.Size = new Size(95, 25);
            upload_button.TabIndex = 2;
            upload_button.Text = "Upload File(s)";
            upload_button.UseVisualStyleBackColor = true;
            upload_button.Click += upload_button_Click;
            // 
            // send_prompt_button
            // 
            send_prompt_button.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            send_prompt_button.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            send_prompt_button.FlatStyle = FlatStyle.System;
            send_prompt_button.Location = new Point(192, 6);
            send_prompt_button.Name = "send_prompt_button";
            send_prompt_button.Size = new Size(144, 25);
            send_prompt_button.TabIndex = 1;
            send_prompt_button.Text = "Send Prompt ->";
            send_prompt_button.UseVisualStyleBackColor = true;
            send_prompt_button.Click += send_prompt_button_Click;
            // 
            // character_textBox
            // 
            tulpa_textBox.Dock = DockStyle.Top;
            tulpa_textBox.Enabled = false;
            tulpa_textBox.Location = new Point(12, 12);
            tulpa_textBox.Name = "tulpa_textBox";
            tulpa_textBox.PlaceholderText = "Current Character";
            tulpa_textBox.Size = new Size(336, 23);
            tulpa_textBox.TabIndex = 2;
            // 
            // toggle_RIGHT_button
            // 
            toggle_RIGHT_button.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            toggle_RIGHT_button.Font = new Font("Consolas", 6F, FontStyle.Regular, GraphicsUnit.Point);
            toggle_RIGHT_button.Location = new Point(-2, 0);
            toggle_RIGHT_button.Margin = new Padding(0);
            toggle_RIGHT_button.Name = "toggle_RIGHT_button";
            toggle_RIGHT_button.Size = new Size(12, 324);
            toggle_RIGHT_button.TabIndex = 5;
            toggle_RIGHT_button.Text = ">";
            toggle_RIGHT_button.UseVisualStyleBackColor = true;
            toggle_RIGHT_button.Click += toggle_RIGHT_button_Click;
            // 
            // preview_tabControl
            // 
            preview_tabControl.Controls.Add(webview2_tabPage);
            preview_tabControl.Controls.Add(markf278down_tabPage);
            preview_tabControl.Dock = DockStyle.Fill;
            preview_tabControl.Location = new Point(12, 35);
            preview_tabControl.Multiline = true;
            preview_tabControl.Name = "preview_tabControl";
            preview_tabControl.SelectedIndex = 0;
            preview_tabControl.Size = new Size(330, 252);
            preview_tabControl.TabIndex = 4;
            // 
            // webview2_tabPage
            // 
            webview2_tabPage.Controls.Add(webView21);
            webview2_tabPage.Location = new Point(4, 24);
            webview2_tabPage.Name = "webview2_tabPage";
            webview2_tabPage.Padding = new Padding(3);
            webview2_tabPage.Size = new Size(322, 224);
            webview2_tabPage.TabIndex = 2;
            webview2_tabPage.Text = "WebView2";
            webview2_tabPage.UseVisualStyleBackColor = true;
            // 
            // webView21
            // 
            webView21.AllowExternalDrop = true;
            webView21.CreationProperties = null;
            webView21.DefaultBackgroundColor = Color.White;
            webView21.Dock = DockStyle.Fill;
            webView21.Location = new Point(3, 3);
            webView21.Name = "webView21";
            webView21.Size = new Size(316, 218);
            webView21.TabIndex = 0;
            webView21.ZoomFactor = 1D;
            // 
            // markf278down_tabPage
            // 
            markf278down_tabPage.Controls.Add(response_textBox);
            markf278down_tabPage.Controls.Add(submit_edits_button);
            markf278down_tabPage.Location = new Point(4, 24);
            markf278down_tabPage.Name = "markf278down_tabPage";
            markf278down_tabPage.Padding = new Padding(3);
            markf278down_tabPage.Size = new Size(322, 224);
            markf278down_tabPage.TabIndex = 1;
            markf278down_tabPage.Text = "markf278down";
            markf278down_tabPage.UseVisualStyleBackColor = true;
            // 
            // response_textBox
            // 
            response_textBox.Dock = DockStyle.Fill;
            response_textBox.Location = new Point(3, 26);
            response_textBox.Multiline = true;
            response_textBox.Name = "response_textBox";
            response_textBox.PlaceholderText = "Conversation";
            response_textBox.ReadOnly = true;
            response_textBox.ScrollBars = ScrollBars.Both;
            response_textBox.Size = new Size(316, 195);
            response_textBox.TabIndex = 1;
            response_textBox.Enter += response_textBox_Enter;
            response_textBox.Leave += response_textBox_Leave;
            // 
            // submit_edits_button
            // 
            submit_edits_button.Dock = DockStyle.Top;
            submit_edits_button.Location = new Point(3, 3);
            submit_edits_button.Name = "submit_edits_button";
            submit_edits_button.Size = new Size(316, 23);
            submit_edits_button.TabIndex = 2;
            submit_edits_button.Text = "Submit Edits";
            submit_edits_button.UseVisualStyleBackColor = true;
            submit_edits_button.Click += submit_edit_button_Click;
            // 
            // new_conversation_button
            // 
            new_conversation_button.AutoSize = true;
            new_conversation_button.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            new_conversation_button.Dock = DockStyle.Bottom;
            new_conversation_button.Location = new Point(12, 287);
            new_conversation_button.Name = "new_conversation_button";
            new_conversation_button.Size = new Size(330, 25);
            new_conversation_button.TabIndex = 2;
            new_conversation_button.Text = "New Conversation";
            new_conversation_button.UseVisualStyleBackColor = true;
            new_conversation_button.Click += new_conversation_button_Click;
            // 
            // conversation_history_treeView
            // 
            conversation_history_treeView.Dock = DockStyle.Left;
            conversation_history_treeView.HideSelection = false;
            conversation_history_treeView.Indent = 10;
            conversation_history_treeView.Location = new Point(0, 37);
            conversation_history_treeView.Name = "conversation_history_treeView";
            treeNode6.Name = "Node2";
            treeNode6.Text = "Node2";
            treeNode7.Name = "Node1";
            treeNode7.Text = "Chat1";
            treeNode8.Name = "Node3";
            treeNode8.Text = "Node3";
            treeNode9.Name = "Node4";
            treeNode9.Text = "Node4";
            treeNode10.Name = "RootNode";
            treeNode10.Text = "Conversation History Root";
            conversation_history_treeView.Nodes.AddRange(new TreeNode[] { treeNode10 });
            conversation_history_treeView.PathSeparator = "/";
            conversation_history_treeView.Size = new Size(161, 324);
            conversation_history_treeView.TabIndex = 0;
            conversation_history_treeView.BeforeCollapse += conversation_history_treeView_BeforeCollapse;
            conversation_history_treeView.AfterSelect += conversation_history_treeView_AfterSelect;
            conversation_history_treeView.NodeMouseDoubleClick += conversation_history_treeView_NodeMouseDoubleClick;
            // 
            // main_splitter
            // 
            main_splitter.Location = new Point(161, 37);
            main_splitter.Name = "main_splitter";
            main_splitter.Size = new Size(3, 324);
            main_splitter.TabIndex = 1;
            main_splitter.TabStop = false;
            main_splitter.MouseDoubleClick += main_splitter_MouseDoubleClick;
            // 
            // main_menuStrip
            // 
            main_menuStrip.Items.AddRange(new ToolStripItem[] { base_directory_toolStripMenuItem, settings_ToolStripMenuItem, tools_ToolStripMenuItem, help_ToolStripMenuItem });
            main_menuStrip.Location = new Point(0, 0);
            main_menuStrip.Name = "main_menuStrip";
            main_menuStrip.ShowItemToolTips = true;
            main_menuStrip.Size = new Size(882, 24);
            main_menuStrip.TabIndex = 2;
            main_menuStrip.Text = "Main Menu";
            // 
            // base_directory_toolStripMenuItem
            // 
            base_directory_toolStripMenuItem.Name = "base_directory_toolStripMenuItem";
            base_directory_toolStripMenuItem.Size = new Size(94, 20);
            base_directory_toolStripMenuItem.Text = "&Base Directory";
            base_directory_toolStripMenuItem.ToolTipText = "Specify the base directory.";
            base_directory_toolStripMenuItem.Click += base_directory_toolStripMenuItem_Click;
            // 
            // settings_ToolStripMenuItem
            // 
            settings_ToolStripMenuItem.Name = "settings_ToolStripMenuItem";
            settings_ToolStripMenuItem.Size = new Size(61, 20);
            settings_ToolStripMenuItem.Text = "Settings";
            settings_ToolStripMenuItem.Click += settings_ToolStripMenuItem_Click;
            // 
            // tools_ToolStripMenuItem
            // 
            tools_ToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { openai_api_key_toolStripMenuItem, tokenCounter_ToolStripMenuItem, sysmsghack_ToolStripMenuItem, models_ToolStripMenuItem, openConfigDirectory_ToolStripMenuItem });
            tools_ToolStripMenuItem.Name = "tools_ToolStripMenuItem";
            tools_ToolStripMenuItem.Size = new Size(46, 20);
            tools_ToolStripMenuItem.Text = "&Tools";
            // 
            // openai_api_key_toolStripMenuItem
            // 
            openai_api_key_toolStripMenuItem.Image = (Image)resources.GetObject("openai_api_key_toolStripMenuItem.Image");
            openai_api_key_toolStripMenuItem.Name = "openai_api_key_toolStripMenuItem";
            openai_api_key_toolStripMenuItem.Size = new Size(193, 22);
            openai_api_key_toolStripMenuItem.Text = "OpenAI API Key";
            openai_api_key_toolStripMenuItem.Click += openai_api_key_toolStripMenuItem_Click;
            // 
            // tokenCounter_ToolStripMenuItem
            // 
            tokenCounter_ToolStripMenuItem.Image = Properties.Resources.TokenCounter;
            tokenCounter_ToolStripMenuItem.Name = "tokenCounter_ToolStripMenuItem";
            tokenCounter_ToolStripMenuItem.Size = new Size(193, 22);
            tokenCounter_ToolStripMenuItem.Text = "Token Counter";
            tokenCounter_ToolStripMenuItem.Click += tokenCounterToolStripMenuItem_Click;
            // 
            // sysmsghack_ToolStripMenuItem
            // 
            sysmsghack_ToolStripMenuItem.CheckOnClick = true;
            sysmsghack_ToolStripMenuItem.Image = Properties.Resources.AddField;
            sysmsghack_ToolStripMenuItem.Name = "sysmsghack_ToolStripMenuItem";
            sysmsghack_ToolStripMenuItem.Size = new Size(193, 22);
            sysmsghack_ToolStripMenuItem.Text = "GPT4 sysmsghack";
            sysmsghack_ToolStripMenuItem.Click += sysmsghack_ToolStripMenuItem_Click;
            // 
            // models_ToolStripMenuItem
            // 
            models_ToolStripMenuItem.Image = Properties.Resources.ColumnGroup;
            models_ToolStripMenuItem.Name = "models_ToolStripMenuItem";
            models_ToolStripMenuItem.Size = new Size(193, 22);
            models_ToolStripMenuItem.Text = "Models";
            // 
            // openConfigDirectory_ToolStripMenuItem
            // 
            openConfigDirectory_ToolStripMenuItem.Name = "openConfigDirectory_ToolStripMenuItem";
            openConfigDirectory_ToolStripMenuItem.Size = new Size(193, 22);
            openConfigDirectory_ToolStripMenuItem.Text = "Open Config Directory";
            openConfigDirectory_ToolStripMenuItem.Click += openConfigDirectoryToolStripMenuItem_Click;
            // 
            // help_ToolStripMenuItem
            // 
            help_ToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { about_ToolStripMenuItem });
            help_ToolStripMenuItem.Name = "help_ToolStripMenuItem";
            help_ToolStripMenuItem.Size = new Size(44, 20);
            help_ToolStripMenuItem.Text = "&Help";
            // 
            // about_ToolStripMenuItem
            // 
            about_ToolStripMenuItem.Image = (Image)resources.GetObject("about_ToolStripMenuItem.Image");
            about_ToolStripMenuItem.Name = "about_ToolStripMenuItem";
            about_ToolStripMenuItem.Size = new Size(180, 22);
            about_ToolStripMenuItem.Text = "&About...";
            about_ToolStripMenuItem.Click += about_ToolStripMenuItem_Click;
            // 
            // main_panel
            // 
            main_panel.Controls.Add(main_splitter);
            main_panel.Controls.Add(text_splitContainer);
            main_panel.Controls.Add(conversation_history_treeView);
            main_panel.Controls.Add(tulpas_tableLayoutPanel);
            main_panel.Dock = DockStyle.Fill;
            main_panel.Location = new Point(0, 24);
            main_panel.Name = "main_panel";
            main_panel.Size = new Size(882, 361);
            main_panel.TabIndex = 2;
            // 
            // characters_tableLayoutPanel
            // 
            tulpas_tableLayoutPanel.AutoSize = true;
            tulpas_tableLayoutPanel.ColumnCount = 1;
            tulpas_tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tulpas_tableLayoutPanel.Controls.Add(tulpas_flowLayoutPanel, 0, 0);
            tulpas_tableLayoutPanel.Dock = DockStyle.Top;
            tulpas_tableLayoutPanel.Location = new Point(0, 0);
            tulpas_tableLayoutPanel.Name = "tulpas_tableLayoutPanel";
            tulpas_tableLayoutPanel.RowCount = 1;
            tulpas_tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tulpas_tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tulpas_tableLayoutPanel.Size = new Size(882, 37);
            tulpas_tableLayoutPanel.TabIndex = 3;
            // 
            // tulpas_flowLayoutPanel
            // 
            tulpas_flowLayoutPanel.Anchor = AnchorStyles.Top;
            tulpas_flowLayoutPanel.AutoSize = true;
            tulpas_flowLayoutPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tulpas_flowLayoutPanel.Controls.Add(placeholder_radioButton);
            tulpas_flowLayoutPanel.Location = new Point(365, 3);
            tulpas_flowLayoutPanel.Name = "tulpas_flowLayoutPanel";
            tulpas_flowLayoutPanel.Size = new Size(152, 31);
            tulpas_flowLayoutPanel.TabIndex = 2;
            // 
            // placeholder_radioButton
            // 
            placeholder_radioButton.Appearance = Appearance.Button;
            placeholder_radioButton.AutoSize = true;
            placeholder_radioButton.Location = new Point(3, 3);
            placeholder_radioButton.Name = "placeholder_radioButton";
            placeholder_radioButton.Size = new Size(146, 25);
            placeholder_radioButton.TabIndex = 2;
            placeholder_radioButton.TabStop = true;
            placeholder_radioButton.Text = "_TULPA_PLACEHOLDER_";
            placeholder_radioButton.UseVisualStyleBackColor = true;
            // 
            // main_statusStrip
            // 
            main_statusStrip.Items.AddRange(new ToolStripItem[] { main_toolStripStatusLabel, main_toolStripProgressBar });
            main_statusStrip.Location = new Point(0, 385);
            main_statusStrip.Name = "main_statusStrip";
            main_statusStrip.Size = new Size(882, 22);
            main_statusStrip.TabIndex = 4;
            main_statusStrip.Text = "statusStrip1";
            // 
            // main_toolStripStatusLabel
            // 
            main_toolStripStatusLabel.Name = "main_toolStripStatusLabel";
            main_toolStripStatusLabel.Size = new Size(50, 17);
            main_toolStripStatusLabel.Text = "Loading";
            // 
            // main_toolStripProgressBar
            // 
            main_toolStripProgressBar.Name = "main_toolStripProgressBar";
            main_toolStripProgressBar.Size = new Size(100, 16);
            main_toolStripProgressBar.Style = ProgressBarStyle.Marquee;
            // 
            // base_directory_vistaFolderBrowserDialog
            // 
            base_directory_vistaFolderBrowserDialog.Description = "Select Base Directory";
            // 
            // upload_vistaOpenFileDialog
            // 
            upload_vistaOpenFileDialog.AddExtension = false;
            upload_vistaOpenFileDialog.Filter = null;
            upload_vistaOpenFileDialog.Multiselect = true;
            upload_vistaOpenFileDialog.SupportMultiDottedExtensions = true;
            upload_vistaOpenFileDialog.Title = "Choose wisely!";
            // 
            // WinGPT_Form
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(882, 407);
            Controls.Add(main_panel);
            Controls.Add(main_menuStrip);
            Controls.Add(main_statusStrip);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = main_menuStrip;
            Name = "WinGPT_Form";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "WinGPT";
            text_splitContainer.Panel1.ResumeLayout(false);
            text_splitContainer.Panel1.PerformLayout();
            text_splitContainer.Panel2.ResumeLayout(false);
            text_splitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)text_splitContainer).EndInit();
            text_splitContainer.ResumeLayout(false);
            prompt_buttons_panel.ResumeLayout(false);
            prompt_buttons_panel.PerformLayout();
            preview_tabControl.ResumeLayout(false);
            webview2_tabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)webView21).EndInit();
            markf278down_tabPage.ResumeLayout(false);
            markf278down_tabPage.PerformLayout();
            main_menuStrip.ResumeLayout(false);
            main_menuStrip.PerformLayout();
            main_panel.ResumeLayout(false);
            main_panel.PerformLayout();
            tulpas_tableLayoutPanel.ResumeLayout(false);
            tulpas_tableLayoutPanel.PerformLayout();
            tulpas_flowLayoutPanel.ResumeLayout(false);
            tulpas_flowLayoutPanel.PerformLayout();
            main_statusStrip.ResumeLayout(false);
            main_statusStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private SplitContainer text_splitContainer;
        private TreeView conversation_history_treeView;
        private TextBox prompt_textBox;
        private TextBox response_textBox;
        private Splitter main_splitter;
        private MenuStrip main_menuStrip;
        private Button send_prompt_button;
        private Panel main_panel;
        private Button new_conversation_button;
        private ToolStripMenuItem base_directory_toolStripMenuItem;
        private ToolStripMenuItem tools_ToolStripMenuItem;
        private ToolStripMenuItem openai_api_key_toolStripMenuItem;
        private ToolStripMenuItem help_ToolStripMenuItem;
        private ToolStripMenuItem about_ToolStripMenuItem;
        internal TextBox history_file_name_textBox;
        private TextBox tulpa_textBox;
        private TableLayoutPanel tulpas_tableLayoutPanel;
        private FlowLayoutPanel tulpas_flowLayoutPanel;
        private RadioButton placeholder_radioButton;
        private StatusStrip main_statusStrip;
        private ToolStripStatusLabel main_toolStripStatusLabel;
        private ToolStripProgressBar main_toolStripProgressBar;
        private Ookii.Dialogs.WinForms.VistaFolderBrowserDialog base_directory_vistaFolderBrowserDialog;
        internal ToolTip main_toolTip;
        private ToolStripMenuItem tokenCounter_ToolStripMenuItem;
        private ToolStripMenuItem sysmsghack_ToolStripMenuItem;
        private ToolStripMenuItem models_ToolStripMenuItem;
        private TabControl preview_tabControl;
        private TabPage markf278down_tabPage;
        private TabPage webview2_tabPage;
        private Microsoft.Web.WebView2.WinForms.WebView2 webView21;
        private Panel prompt_buttons_panel;
        private Button upload_button;
        private ComboBox uploaded_files_comboBox;
        private Ookii.Dialogs.WinForms.VistaOpenFileDialog upload_vistaOpenFileDialog;
        internal Button clear_button;
        internal CheckBox autoclear_checkBox;
        private ToolStripMenuItem settings_ToolStripMenuItem;
        private Button submit_edits_button;
        internal Button remove_file_button;
        private ToolStripMenuItem openConfigDirectory_ToolStripMenuItem;
        private Button toggle_RIGHT_button;
        private Button toggle_LEFT_button;
    }
}