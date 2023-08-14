namespace WinGPT
{
    partial class API_KEY_Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(API_KEY_Form));
            groupBox1 = new GroupBox();
            api_key_textBox = new TextBox();
            api_key_ok_button = new Button();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Anchor = AnchorStyles.Top;
            groupBox1.AutoSize = true;
            groupBox1.Controls.Add(api_key_textBox);
            groupBox1.Location = new Point(12, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(486, 67);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "OpenAI API Key";
            // 
            // api_key_textBox
            // 
            api_key_textBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            api_key_textBox.Location = new Point(6, 22);
            api_key_textBox.MaxLength = 52;
            api_key_textBox.Name = "api_key_textBox";
            api_key_textBox.PlaceholderText = "sk-ccccccccccccccccccaaaaaaaaaaafffffffffeeeeeeeeee";
            api_key_textBox.Size = new Size(474, 23);
            api_key_textBox.TabIndex = 0;
            api_key_textBox.TextAlign = HorizontalAlignment.Center;
            // 
            // api_key_ok_button
            // 
            api_key_ok_button.Anchor = AnchorStyles.Top;
            api_key_ok_button.Location = new Point(216, 71);
            api_key_ok_button.Name = "api_key_ok_button";
            api_key_ok_button.Size = new Size(75, 23);
            api_key_ok_button.TabIndex = 1;
            api_key_ok_button.Text = "OK";
            api_key_ok_button.UseVisualStyleBackColor = true;
            api_key_ok_button.Click += api_key_ok_button_Click;
            // 
            // API_KEY_Form
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(507, 106);
            Controls.Add(api_key_ok_button);
            Controls.Add(groupBox1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "API_KEY_Form";
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "API_KEY_Form";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private GroupBox groupBox1;
        private TextBox api_key_textBox;
        private Button api_key_ok_button;
    }
}