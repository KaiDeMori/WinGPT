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
            TreeNode treeNode1 = new TreeNode("Node2");
            TreeNode treeNode2 = new TreeNode("Chat1", new TreeNode[] { treeNode1 });
            TreeNode treeNode3 = new TreeNode("Node3");
            TreeNode treeNode4 = new TreeNode("Node4");
            TreeNode treeNode5 = new TreeNode("Conversation History Root", new TreeNode[] { treeNode2, treeNode3, treeNode4 });
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WinGPT_Form));
            main_toolTip = new ToolTip(components);
            text_splitContainer = new SplitContainer();
            prompt_textBox = new TextBox();
            send_prompt_button = new Button();
            character_textBox = new TextBox();
            response_textBox = new TextBox();
            new_conversation_button = new Button();
            conversation_name_textBox = new TextBox();
            conversation_history_treeView = new TreeView();
            splitter1 = new Splitter();
            main_menuStrip = new MenuStrip();
            base_directory_toolStripMenuItem = new ToolStripMenuItem();
            toolsToolStripMenuItem = new ToolStripMenuItem();
            openai_api_key_toolStripMenuItem = new ToolStripMenuItem();
            tokenCounterToolStripMenuItem = new ToolStripMenuItem();
            sysmsghack_ToolStripMenuItem = new ToolStripMenuItem();
            models_ToolStripMenuItem = new ToolStripMenuItem();
            helpToolStripMenuItem = new ToolStripMenuItem();
            aboutToolStripMenuItem1 = new ToolStripMenuItem();
            panel1 = new Panel();
            characters_tableLayoutPanel = new TableLayoutPanel();
            characters_flowLayoutPanel = new FlowLayoutPanel();
            placeholder_radioButton = new RadioButton();
            contentsToolStripMenuItem = new ToolStripMenuItem();
            indexToolStripMenuItem = new ToolStripMenuItem();
            searchToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator5 = new ToolStripSeparator();
            aboutToolStripMenuItem = new ToolStripMenuItem();
            customizeToolStripMenuItem = new ToolStripMenuItem();
            optionsToolStripMenuItem = new ToolStripMenuItem();
            undoToolStripMenuItem = new ToolStripMenuItem();
            redoToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator3 = new ToolStripSeparator();
            cutToolStripMenuItem = new ToolStripMenuItem();
            copyToolStripMenuItem = new ToolStripMenuItem();
            pasteToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator4 = new ToolStripSeparator();
            selectAllToolStripMenuItem = new ToolStripMenuItem();
            newToolStripMenuItem = new ToolStripMenuItem();
            openToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator = new ToolStripSeparator();
            saveToolStripMenuItem = new ToolStripMenuItem();
            saveAsToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            printToolStripMenuItem = new ToolStripMenuItem();
            printPreviewToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator2 = new ToolStripSeparator();
            exitToolStripMenuItem = new ToolStripMenuItem();
            main_statusStrip = new StatusStrip();
            main_toolStripStatusLabel = new ToolStripStatusLabel();
            main_toolStripProgressBar = new ToolStripProgressBar();
            base_directory_vistaFolderBrowserDialog = new Ookii.Dialogs.WinForms.VistaFolderBrowserDialog();
            ((System.ComponentModel.ISupportInitialize)text_splitContainer).BeginInit();
            text_splitContainer.Panel1.SuspendLayout();
            text_splitContainer.Panel2.SuspendLayout();
            text_splitContainer.SuspendLayout();
            main_menuStrip.SuspendLayout();
            panel1.SuspendLayout();
            characters_tableLayoutPanel.SuspendLayout();
            characters_flowLayoutPanel.SuspendLayout();
            main_statusStrip.SuspendLayout();
            SuspendLayout();
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
            text_splitContainer.Panel1.Controls.Add(prompt_textBox);
            text_splitContainer.Panel1.Controls.Add(send_prompt_button);
            text_splitContainer.Panel1.Controls.Add(character_textBox);
            text_splitContainer.Panel1.Padding = new Padding(12);
            // 
            // text_splitContainer.Panel2
            // 
            text_splitContainer.Panel2.BackColor = SystemColors.Control;
            text_splitContainer.Panel2.Controls.Add(response_textBox);
            text_splitContainer.Panel2.Controls.Add(new_conversation_button);
            text_splitContainer.Panel2.Controls.Add(conversation_name_textBox);
            text_splitContainer.Panel2.Padding = new Padding(12);
            text_splitContainer.Size = new Size(475, 283);
            text_splitContainer.SplitterDistance = 219;
            text_splitContainer.SplitterWidth = 7;
            text_splitContainer.TabIndex = 0;
            // 
            // prompt_textBox
            // 
            prompt_textBox.Dock = DockStyle.Fill;
            prompt_textBox.Location = new Point(12, 35);
            prompt_textBox.Multiline = true;
            prompt_textBox.Name = "prompt_textBox";
            prompt_textBox.PlaceholderText = "Prompt";
            prompt_textBox.ScrollBars = ScrollBars.Both;
            prompt_textBox.Size = new Size(195, 212);
            prompt_textBox.TabIndex = 0;
            prompt_textBox.KeyDown += prompt_textBox_KeyDown;
            // 
            // send_prompt_button
            // 
            send_prompt_button.AutoSize = true;
            send_prompt_button.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            send_prompt_button.Dock = DockStyle.Bottom;
            send_prompt_button.FlatStyle = FlatStyle.System;
            send_prompt_button.Location = new Point(12, 247);
            send_prompt_button.Name = "send_prompt_button";
            send_prompt_button.Size = new Size(195, 24);
            send_prompt_button.TabIndex = 1;
            send_prompt_button.Text = "Send Prompt ->";
            send_prompt_button.UseVisualStyleBackColor = true;
            send_prompt_button.Click += send_prompt_button_Click;
            // 
            // character_textBox
            // 
            character_textBox.Dock = DockStyle.Top;
            character_textBox.Enabled = false;
            character_textBox.Location = new Point(12, 12);
            character_textBox.Name = "character_textBox";
            character_textBox.PlaceholderText = "Current Character";
            character_textBox.Size = new Size(195, 23);
            character_textBox.TabIndex = 2;
            // 
            // response_textBox
            // 
            response_textBox.Dock = DockStyle.Fill;
            response_textBox.Location = new Point(12, 35);
            response_textBox.Multiline = true;
            response_textBox.Name = "response_textBox";
            response_textBox.PlaceholderText = "Conversation";
            response_textBox.ScrollBars = ScrollBars.Both;
            response_textBox.Size = new Size(225, 211);
            response_textBox.TabIndex = 1;
            // 
            // new_conversation_button
            // 
            new_conversation_button.AutoSize = true;
            new_conversation_button.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            new_conversation_button.Dock = DockStyle.Bottom;
            new_conversation_button.Location = new Point(12, 246);
            new_conversation_button.Name = "new_conversation_button";
            new_conversation_button.Size = new Size(225, 25);
            new_conversation_button.TabIndex = 2;
            new_conversation_button.Text = "New Conversation";
            new_conversation_button.UseVisualStyleBackColor = true;
            new_conversation_button.Click += new_conversation_button_Click;
            // 
            // conversation_name_textBox
            // 
            conversation_name_textBox.Dock = DockStyle.Top;
            conversation_name_textBox.Location = new Point(12, 12);
            conversation_name_textBox.Name = "conversation_name_textBox";
            conversation_name_textBox.PlaceholderText = "Name of Conversation";
            conversation_name_textBox.Size = new Size(225, 23);
            conversation_name_textBox.TabIndex = 3;
            conversation_name_textBox.KeyDown += conversation_name_textBox_KeyDown;
            conversation_name_textBox.MouseDoubleClick += conversation_name_textBox_MouseDoubleClick;
            // 
            // conversation_history_treeView
            // 
            conversation_history_treeView.Dock = DockStyle.Left;
            conversation_history_treeView.Location = new Point(0, 37);
            conversation_history_treeView.Name = "conversation_history_treeView";
            treeNode1.Name = "Node2";
            treeNode1.Text = "Node2";
            treeNode2.Name = "Node1";
            treeNode2.Text = "Chat1";
            treeNode3.Name = "Node3";
            treeNode3.Text = "Node3";
            treeNode4.Name = "Node4";
            treeNode4.Text = "Node4";
            treeNode5.Name = "RootNode";
            treeNode5.Text = "Conversation History Root";
            conversation_history_treeView.Nodes.AddRange(new TreeNode[] { treeNode5 });
            conversation_history_treeView.PathSeparator = "/";
            conversation_history_treeView.Size = new Size(161, 283);
            conversation_history_treeView.TabIndex = 0;
            conversation_history_treeView.AfterSelect += conversation_history_treeView_AfterSelect;
            // 
            // splitter1
            // 
            splitter1.Location = new Point(161, 37);
            splitter1.Name = "splitter1";
            splitter1.Size = new Size(3, 283);
            splitter1.TabIndex = 1;
            splitter1.TabStop = false;
            // 
            // main_menuStrip
            // 
            main_menuStrip.Items.AddRange(new ToolStripItem[] { base_directory_toolStripMenuItem, toolsToolStripMenuItem, helpToolStripMenuItem });
            main_menuStrip.Location = new Point(0, 0);
            main_menuStrip.Name = "main_menuStrip";
            main_menuStrip.ShowItemToolTips = true;
            main_menuStrip.Size = new Size(636, 24);
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
            // toolsToolStripMenuItem
            // 
            toolsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { openai_api_key_toolStripMenuItem, tokenCounterToolStripMenuItem, sysmsghack_ToolStripMenuItem, models_ToolStripMenuItem });
            toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            toolsToolStripMenuItem.Size = new Size(46, 20);
            toolsToolStripMenuItem.Text = "&Tools";
            // 
            // openai_api_key_toolStripMenuItem
            // 
            openai_api_key_toolStripMenuItem.Image = (Image)resources.GetObject("openai_api_key_toolStripMenuItem.Image");
            openai_api_key_toolStripMenuItem.Name = "openai_api_key_toolStripMenuItem";
            openai_api_key_toolStripMenuItem.Size = new Size(168, 22);
            openai_api_key_toolStripMenuItem.Text = "OpenAI API Key";
            openai_api_key_toolStripMenuItem.Click += openai_api_key_toolStripMenuItem_Click;
            // 
            // tokenCounterToolStripMenuItem
            // 
            tokenCounterToolStripMenuItem.Image = Properties.Resources.TokenCounter;
            tokenCounterToolStripMenuItem.Name = "tokenCounterToolStripMenuItem";
            tokenCounterToolStripMenuItem.Size = new Size(168, 22);
            tokenCounterToolStripMenuItem.Text = "Token Counter";
            tokenCounterToolStripMenuItem.Click += tokenCounterToolStripMenuItem_Click;
            // 
            // sysmsghack_ToolStripMenuItem
            // 
            sysmsghack_ToolStripMenuItem.CheckOnClick = true;
            sysmsghack_ToolStripMenuItem.Image = Properties.Resources.AddField;
            sysmsghack_ToolStripMenuItem.Name = "sysmsghack_ToolStripMenuItem";
            sysmsghack_ToolStripMenuItem.Size = new Size(168, 22);
            sysmsghack_ToolStripMenuItem.Text = "GPT4 sysmsghack";
            sysmsghack_ToolStripMenuItem.Click += sysmsghack_ToolStripMenuItem_Click;
            // 
            // models_ToolStripMenuItem
            // 
            models_ToolStripMenuItem.Image = Properties.Resources.ColumnGroup;
            models_ToolStripMenuItem.Name = "models_ToolStripMenuItem";
            models_ToolStripMenuItem.Size = new Size(168, 22);
            models_ToolStripMenuItem.Text = "Models";
            // 
            // helpToolStripMenuItem
            // 
            helpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { aboutToolStripMenuItem1 });
            helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            helpToolStripMenuItem.Size = new Size(44, 20);
            helpToolStripMenuItem.Text = "&Help";
            // 
            // aboutToolStripMenuItem1
            // 
            aboutToolStripMenuItem1.Image = (Image)resources.GetObject("aboutToolStripMenuItem1.Image");
            aboutToolStripMenuItem1.Name = "aboutToolStripMenuItem1";
            aboutToolStripMenuItem1.Size = new Size(116, 22);
            aboutToolStripMenuItem1.Text = "&About...";
            aboutToolStripMenuItem1.Click += about_ToolStripMenuItem_Click;
            // 
            // panel1
            // 
            panel1.Controls.Add(splitter1);
            panel1.Controls.Add(text_splitContainer);
            panel1.Controls.Add(conversation_history_treeView);
            panel1.Controls.Add(characters_tableLayoutPanel);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 24);
            panel1.Name = "panel1";
            panel1.Size = new Size(636, 320);
            panel1.TabIndex = 2;
            // 
            // characters_tableLayoutPanel
            // 
            characters_tableLayoutPanel.AutoSize = true;
            characters_tableLayoutPanel.ColumnCount = 1;
            characters_tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            characters_tableLayoutPanel.Controls.Add(characters_flowLayoutPanel, 0, 0);
            characters_tableLayoutPanel.Dock = DockStyle.Top;
            characters_tableLayoutPanel.Location = new Point(0, 0);
            characters_tableLayoutPanel.Name = "characters_tableLayoutPanel";
            characters_tableLayoutPanel.RowCount = 1;
            characters_tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            characters_tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            characters_tableLayoutPanel.Size = new Size(636, 37);
            characters_tableLayoutPanel.TabIndex = 3;
            // 
            // characters_flowLayoutPanel
            // 
            characters_flowLayoutPanel.Anchor = AnchorStyles.Top;
            characters_flowLayoutPanel.AutoSize = true;
            characters_flowLayoutPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            characters_flowLayoutPanel.Controls.Add(placeholder_radioButton);
            characters_flowLayoutPanel.Location = new Point(261, 3);
            characters_flowLayoutPanel.Name = "characters_flowLayoutPanel";
            characters_flowLayoutPanel.Size = new Size(113, 31);
            characters_flowLayoutPanel.TabIndex = 2;
            // 
            // placeholder_radioButton
            // 
            placeholder_radioButton.Appearance = Appearance.Button;
            placeholder_radioButton.AutoSize = true;
            placeholder_radioButton.Location = new Point(3, 3);
            placeholder_radioButton.Name = "placeholder_radioButton";
            placeholder_radioButton.Size = new Size(107, 25);
            placeholder_radioButton.TabIndex = 2;
            placeholder_radioButton.TabStop = true;
            placeholder_radioButton.Text = "_PLACEHOLDER_";
            placeholder_radioButton.UseVisualStyleBackColor = true;
            // 
            // contentsToolStripMenuItem
            // 
            contentsToolStripMenuItem.Name = "contentsToolStripMenuItem";
            contentsToolStripMenuItem.Size = new Size(122, 22);
            contentsToolStripMenuItem.Text = "&Contents";
            // 
            // indexToolStripMenuItem
            // 
            indexToolStripMenuItem.Name = "indexToolStripMenuItem";
            indexToolStripMenuItem.Size = new Size(122, 22);
            indexToolStripMenuItem.Text = "&Index";
            // 
            // searchToolStripMenuItem
            // 
            searchToolStripMenuItem.Name = "searchToolStripMenuItem";
            searchToolStripMenuItem.Size = new Size(122, 22);
            searchToolStripMenuItem.Text = "&Search";
            // 
            // toolStripSeparator5
            // 
            toolStripSeparator5.Name = "toolStripSeparator5";
            toolStripSeparator5.Size = new Size(119, 6);
            // 
            // aboutToolStripMenuItem
            // 
            aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            aboutToolStripMenuItem.Size = new Size(122, 22);
            aboutToolStripMenuItem.Text = "&About...";
            // 
            // customizeToolStripMenuItem
            // 
            customizeToolStripMenuItem.Name = "customizeToolStripMenuItem";
            customizeToolStripMenuItem.Size = new Size(130, 22);
            customizeToolStripMenuItem.Text = "&Customize";
            // 
            // optionsToolStripMenuItem
            // 
            optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            optionsToolStripMenuItem.Size = new Size(130, 22);
            optionsToolStripMenuItem.Text = "&Options";
            // 
            // undoToolStripMenuItem
            // 
            undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            undoToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Z;
            undoToolStripMenuItem.Size = new Size(144, 22);
            undoToolStripMenuItem.Text = "&Undo";
            // 
            // redoToolStripMenuItem
            // 
            redoToolStripMenuItem.Name = "redoToolStripMenuItem";
            redoToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Y;
            redoToolStripMenuItem.Size = new Size(144, 22);
            redoToolStripMenuItem.Text = "&Redo";
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new Size(141, 6);
            // 
            // cutToolStripMenuItem
            // 
            cutToolStripMenuItem.Image = (Image)resources.GetObject("cutToolStripMenuItem.Image");
            cutToolStripMenuItem.ImageTransparentColor = Color.Magenta;
            cutToolStripMenuItem.Name = "cutToolStripMenuItem";
            cutToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.X;
            cutToolStripMenuItem.Size = new Size(144, 22);
            cutToolStripMenuItem.Text = "Cu&t";
            // 
            // copyToolStripMenuItem
            // 
            copyToolStripMenuItem.Image = (Image)resources.GetObject("copyToolStripMenuItem.Image");
            copyToolStripMenuItem.ImageTransparentColor = Color.Magenta;
            copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            copyToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.C;
            copyToolStripMenuItem.Size = new Size(144, 22);
            copyToolStripMenuItem.Text = "&Copy";
            // 
            // pasteToolStripMenuItem
            // 
            pasteToolStripMenuItem.Image = (Image)resources.GetObject("pasteToolStripMenuItem.Image");
            pasteToolStripMenuItem.ImageTransparentColor = Color.Magenta;
            pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            pasteToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.V;
            pasteToolStripMenuItem.Size = new Size(144, 22);
            pasteToolStripMenuItem.Text = "&Paste";
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new Size(141, 6);
            // 
            // selectAllToolStripMenuItem
            // 
            selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
            selectAllToolStripMenuItem.Size = new Size(144, 22);
            selectAllToolStripMenuItem.Text = "Select &All";
            // 
            // newToolStripMenuItem
            // 
            newToolStripMenuItem.Image = (Image)resources.GetObject("newToolStripMenuItem.Image");
            newToolStripMenuItem.ImageTransparentColor = Color.Magenta;
            newToolStripMenuItem.Name = "newToolStripMenuItem";
            newToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.N;
            newToolStripMenuItem.Size = new Size(146, 22);
            newToolStripMenuItem.Text = "&New";
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.Image = (Image)resources.GetObject("openToolStripMenuItem.Image");
            openToolStripMenuItem.ImageTransparentColor = Color.Magenta;
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.O;
            openToolStripMenuItem.Size = new Size(146, 22);
            openToolStripMenuItem.Text = "&Open";
            // 
            // toolStripSeparator
            // 
            toolStripSeparator.Name = "toolStripSeparator";
            toolStripSeparator.Size = new Size(143, 6);
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Image = (Image)resources.GetObject("saveToolStripMenuItem.Image");
            saveToolStripMenuItem.ImageTransparentColor = Color.Magenta;
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.S;
            saveToolStripMenuItem.Size = new Size(146, 22);
            saveToolStripMenuItem.Text = "&Save";
            // 
            // saveAsToolStripMenuItem
            // 
            saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            saveAsToolStripMenuItem.Size = new Size(146, 22);
            saveAsToolStripMenuItem.Text = "Save &As";
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(143, 6);
            // 
            // printToolStripMenuItem
            // 
            printToolStripMenuItem.Image = (Image)resources.GetObject("printToolStripMenuItem.Image");
            printToolStripMenuItem.ImageTransparentColor = Color.Magenta;
            printToolStripMenuItem.Name = "printToolStripMenuItem";
            printToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.P;
            printToolStripMenuItem.Size = new Size(146, 22);
            printToolStripMenuItem.Text = "&Print";
            // 
            // printPreviewToolStripMenuItem
            // 
            printPreviewToolStripMenuItem.Image = (Image)resources.GetObject("printPreviewToolStripMenuItem.Image");
            printPreviewToolStripMenuItem.ImageTransparentColor = Color.Magenta;
            printPreviewToolStripMenuItem.Name = "printPreviewToolStripMenuItem";
            printPreviewToolStripMenuItem.Size = new Size(146, 22);
            printPreviewToolStripMenuItem.Text = "Print Pre&view";
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(143, 6);
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(146, 22);
            exitToolStripMenuItem.Text = "E&xit";
            // 
            // main_statusStrip
            // 
            main_statusStrip.Items.AddRange(new ToolStripItem[] { main_toolStripStatusLabel, main_toolStripProgressBar });
            main_statusStrip.Location = new Point(0, 344);
            main_statusStrip.Name = "main_statusStrip";
            main_statusStrip.Size = new Size(636, 22);
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
            // WinGPT_Form
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(636, 366);
            Controls.Add(panel1);
            Controls.Add(main_menuStrip);
            Controls.Add(main_statusStrip);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = main_menuStrip;
            Name = "WinGPT_Form";
            Text = "WinGPT";
            text_splitContainer.Panel1.ResumeLayout(false);
            text_splitContainer.Panel1.PerformLayout();
            text_splitContainer.Panel2.ResumeLayout(false);
            text_splitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)text_splitContainer).EndInit();
            text_splitContainer.ResumeLayout(false);
            main_menuStrip.ResumeLayout(false);
            main_menuStrip.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            characters_tableLayoutPanel.ResumeLayout(false);
            characters_tableLayoutPanel.PerformLayout();
            characters_flowLayoutPanel.ResumeLayout(false);
            characters_flowLayoutPanel.PerformLayout();
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
        private Splitter splitter1;
        private MenuStrip main_menuStrip;
        private Button send_prompt_button;
        private Panel panel1;
        private Button new_conversation_button;
        private ToolStripMenuItem contentsToolStripMenuItem;
        private ToolStripMenuItem indexToolStripMenuItem;
        private ToolStripMenuItem searchToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator5;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private ToolStripMenuItem customizeToolStripMenuItem;
        private ToolStripMenuItem optionsToolStripMenuItem;
        private ToolStripMenuItem undoToolStripMenuItem;
        private ToolStripMenuItem redoToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripMenuItem cutToolStripMenuItem;
        private ToolStripMenuItem copyToolStripMenuItem;
        private ToolStripMenuItem pasteToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripMenuItem selectAllToolStripMenuItem;
        private ToolStripMenuItem newToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem saveAsToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem printToolStripMenuItem;
        private ToolStripMenuItem printPreviewToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem base_directory_toolStripMenuItem;
        private ToolStripMenuItem toolsToolStripMenuItem;
        private ToolStripMenuItem openai_api_key_toolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem1;
        private TextBox conversation_name_textBox;
        private TextBox character_textBox;
        private TableLayoutPanel characters_tableLayoutPanel;
        private FlowLayoutPanel characters_flowLayoutPanel;
        private RadioButton radioButton1;
        private RadioButton radioButton2;
        private RadioButton radioButton4;
        private RadioButton placeholder_radioButton;
        private RadioButton radioButton5;
        private StatusStrip main_statusStrip;
        private ToolStripStatusLabel main_toolStripStatusLabel;
        private ToolStripProgressBar main_toolStripProgressBar;
        private Ookii.Dialogs.WinForms.VistaFolderBrowserDialog base_directory_vistaFolderBrowserDialog;
        private ToolTip main_toolTip;
        private ToolStripMenuItem tokenCounterToolStripMenuItem;
        private ToolStripMenuItem sysmsghack_ToolStripMenuItem;
        private ToolStripMenuItem models_ToolStripMenuItem;
    }
}