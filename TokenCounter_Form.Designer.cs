namespace WinGPT
{
    partial class TokenCounter_Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TokenCounter_Form));
            tableLayoutPanel1 = new TableLayoutPanel();
            label1 = new Label();
            prompt_tokens_label = new Label();
            label3 = new Label();
            label4 = new Label();
            completion_tokens_label = new Label();
            total_tokens_label = new Label();
            label7 = new Label();
            token_limit_numericUpDown = new NumericUpDown();
            ok_button = new Button();
            reset_button = new Button();
            label2 = new Label();
            show_token_limit_message_checkBox = new CheckBox();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)token_limit_numericUpDown).BeginInit();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(label1, 0, 1);
            tableLayoutPanel1.Controls.Add(prompt_tokens_label, 1, 1);
            tableLayoutPanel1.Controls.Add(label3, 0, 2);
            tableLayoutPanel1.Controls.Add(label4, 0, 3);
            tableLayoutPanel1.Controls.Add(completion_tokens_label, 1, 2);
            tableLayoutPanel1.Controls.Add(total_tokens_label, 1, 3);
            tableLayoutPanel1.Controls.Add(label7, 0, 6);
            tableLayoutPanel1.Controls.Add(token_limit_numericUpDown, 1, 6);
            tableLayoutPanel1.Controls.Add(ok_button, 1, 9);
            tableLayoutPanel1.Controls.Add(reset_button, 0, 4);
            tableLayoutPanel1.Controls.Add(label2, 0, 5);
            tableLayoutPanel1.Controls.Add(show_token_limit_message_checkBox, 1, 5);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 13;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 5F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(271, 226);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Location = new Point(47, 7);
            label1.Margin = new Padding(2);
            label1.Name = "label1";
            label1.Size = new Size(86, 15);
            label1.TabIndex = 0;
            label1.Text = "Prompt Tokens";
            // 
            // prompt_tokens_label
            // 
            prompt_tokens_label.AutoSize = true;
            prompt_tokens_label.Location = new Point(137, 7);
            prompt_tokens_label.Margin = new Padding(2);
            prompt_tokens_label.Name = "prompt_tokens_label";
            prompt_tokens_label.Size = new Size(31, 15);
            prompt_tokens_label.TabIndex = 1;
            prompt_tokens_label.Text = "1234";
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label3.AutoSize = true;
            label3.Location = new Point(24, 26);
            label3.Margin = new Padding(2);
            label3.Name = "label3";
            label3.Size = new Size(109, 15);
            label3.TabIndex = 2;
            label3.Text = "Completion Tokens";
            // 
            // label4
            // 
            label4.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label4.AutoSize = true;
            label4.Location = new Point(62, 45);
            label4.Margin = new Padding(2);
            label4.Name = "label4";
            label4.Size = new Size(71, 15);
            label4.TabIndex = 3;
            label4.Text = "Total Tokens";
            // 
            // completion_tokens_label
            // 
            completion_tokens_label.AutoSize = true;
            completion_tokens_label.Location = new Point(137, 26);
            completion_tokens_label.Margin = new Padding(2);
            completion_tokens_label.Name = "completion_tokens_label";
            completion_tokens_label.Size = new Size(31, 15);
            completion_tokens_label.TabIndex = 4;
            completion_tokens_label.Text = "1234";
            // 
            // total_tokens_label
            // 
            total_tokens_label.AutoSize = true;
            total_tokens_label.Location = new Point(137, 45);
            total_tokens_label.Margin = new Padding(2);
            total_tokens_label.Name = "total_tokens_label";
            total_tokens_label.Size = new Size(31, 15);
            total_tokens_label.TabIndex = 5;
            total_tokens_label.Text = "1234";
            // 
            // label7
            // 
            label7.Anchor = AnchorStyles.Right;
            label7.AutoSize = true;
            label7.Location = new Point(65, 120);
            label7.Margin = new Padding(2);
            label7.Name = "label7";
            label7.Size = new Size(68, 15);
            label7.TabIndex = 6;
            label7.Text = "Token Limit";
            // 
            // token_limit_numericUpDown
            // 
            token_limit_numericUpDown.Anchor = AnchorStyles.Left;
            token_limit_numericUpDown.Increment = new decimal(new int[] { 1000, 0, 0, 0 });
            token_limit_numericUpDown.Location = new Point(138, 116);
            token_limit_numericUpDown.Maximum = new decimal(new int[] { 999999, 0, 0, 0 });
            token_limit_numericUpDown.Minimum = new decimal(new int[] { 10, 0, 0, 0 });
            token_limit_numericUpDown.Name = "token_limit_numericUpDown";
            token_limit_numericUpDown.Size = new Size(82, 23);
            token_limit_numericUpDown.TabIndex = 7;
            token_limit_numericUpDown.ThousandsSeparator = true;
            token_limit_numericUpDown.Value = new decimal(new int[] { 10000, 0, 0, 0 });
            token_limit_numericUpDown.ValueChanged += token_limit_numericUpDown_ValueChanged;
            // 
            // ok_button
            // 
            ok_button.Anchor = AnchorStyles.None;
            ok_button.AutoSize = true;
            tableLayoutPanel1.SetColumnSpan(ok_button, 2);
            ok_button.Location = new Point(103, 165);
            ok_button.Name = "ok_button";
            ok_button.Size = new Size(65, 25);
            ok_button.TabIndex = 9;
            ok_button.Text = "OK";
            ok_button.UseVisualStyleBackColor = true;
            ok_button.Click += ok_button_Click;
            // 
            // reset_button
            // 
            reset_button.Anchor = AnchorStyles.None;
            reset_button.AutoSize = true;
            reset_button.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tableLayoutPanel1.SetColumnSpan(reset_button, 2);
            reset_button.Location = new Point(113, 65);
            reset_button.Name = "reset_button";
            reset_button.Size = new Size(45, 25);
            reset_button.TabIndex = 8;
            reset_button.Text = "Reset";
            reset_button.UseVisualStyleBackColor = true;
            reset_button.Click += reset_button_Click;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Right;
            label2.AutoSize = true;
            label2.Location = new Point(48, 95);
            label2.Margin = new Padding(2);
            label2.Name = "label2";
            label2.Size = new Size(85, 15);
            label2.TabIndex = 10;
            label2.Text = "Show Message";
            // 
            // show_token_limit_message_checkBox
            // 
            show_token_limit_message_checkBox.AutoSize = true;
            show_token_limit_message_checkBox.Location = new Point(138, 96);
            show_token_limit_message_checkBox.Name = "show_token_limit_message_checkBox";
            show_token_limit_message_checkBox.Size = new Size(15, 14);
            show_token_limit_message_checkBox.TabIndex = 11;
            show_token_limit_message_checkBox.UseVisualStyleBackColor = true;
            show_token_limit_message_checkBox.CheckedChanged += show_token_limit_message_checkBox_CheckedChanged;
            // 
            // TokenCounter_Form
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(271, 226);
            Controls.Add(tableLayoutPanel1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "TokenCounter_Form";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Token Counter";
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)token_limit_numericUpDown).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private Label label1;
        private Label prompt_tokens_label;
        private Label label3;
        private Label label4;
        private Label completion_tokens_label;
        private Label total_tokens_label;
        private Label label7;
        private NumericUpDown token_limit_numericUpDown;
        private Button reset_button;
        private Button ok_button;
        private Label label2;
        private CheckBox show_token_limit_message_checkBox;
    }
}