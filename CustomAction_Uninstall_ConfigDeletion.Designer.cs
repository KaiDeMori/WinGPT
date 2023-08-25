namespace WinGPT
{
    partial class CustomAction_Uninstall_ConfigDeletion
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomAction_Uninstall_ConfigDeletion));
            label1 = new Label();
            delete_button = new Button();
            keep_button = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top;
            label1.AutoSize = true;
            label1.Location = new Point(63, 36);
            label1.Name = "label1";
            label1.Size = new Size(242, 15);
            label1.TabIndex = 0;
            label1.Text = "Do you want to delete the configuration file?";
            // 
            // delete_button
            // 
            delete_button.Anchor = AnchorStyles.Top;
            delete_button.Location = new Point(40, 87);
            delete_button.Name = "delete_button";
            delete_button.Size = new Size(104, 26);
            delete_button.TabIndex = 2;
            delete_button.Text = "Yes, delete it!";
            delete_button.UseVisualStyleBackColor = true;
            // 
            // keep_button
            // 
            keep_button.Anchor = AnchorStyles.Top;
            keep_button.Location = new Point(225, 87);
            keep_button.Name = "keep_button";
            keep_button.Size = new Size(104, 26);
            keep_button.TabIndex = 1;
            keep_button.Text = "OMG, no!";
            keep_button.UseVisualStyleBackColor = true;
            // 
            // CustomAction_Uninstall_ConfigDeletion
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(369, 134);
            Controls.Add(keep_button);
            Controls.Add(delete_button);
            Controls.Add(label1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "CustomAction_Uninstall_ConfigDeletion";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Configuration Deletion";
            TopMost = true;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Button delete_button;
        private Button keep_button;
    }
}