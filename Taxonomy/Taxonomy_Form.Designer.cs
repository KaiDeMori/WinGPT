namespace WinGPT.Taxonomy
{
    partial class Taxonomy_Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Taxonomy_Form));
            summary_groupBox = new GroupBox();
            summary_textBox = new TextBox();
            filename_groupBox = new GroupBox();
            filename_textBox = new TextBox();
            new_category_groupBox = new GroupBox();
            new_category_textBox = new TextBox();
            existing_categories_groupBox = new GroupBox();
            existing_categories_comboBox = new ComboBox();
            OK_button = new Button();
            reroll_button = new Button();
            trackBar1 = new TrackBar();
            groupBox5 = new GroupBox();
            illegal_state_label = new Label();
            summary_groupBox.SuspendLayout();
            filename_groupBox.SuspendLayout();
            new_category_groupBox.SuspendLayout();
            existing_categories_groupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)trackBar1).BeginInit();
            groupBox5.SuspendLayout();
            SuspendLayout();
            // 
            // summary_groupBox
            // 
            summary_groupBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            summary_groupBox.Controls.Add(summary_textBox);
            summary_groupBox.Location = new Point(12, 136);
            summary_groupBox.Name = "summary_groupBox";
            summary_groupBox.Size = new Size(314, 84);
            summary_groupBox.TabIndex = 0;
            summary_groupBox.TabStop = false;
            summary_groupBox.Text = "Summary";
            // 
            // summary_textBox
            // 
            summary_textBox.Dock = DockStyle.Fill;
            summary_textBox.Location = new Point(3, 19);
            summary_textBox.Multiline = true;
            summary_textBox.Name = "summary_textBox";
            summary_textBox.PlaceholderText = "The summary";
            summary_textBox.ScrollBars = ScrollBars.Both;
            summary_textBox.Size = new Size(308, 62);
            summary_textBox.TabIndex = 0;
            summary_textBox.Text = resources.GetString("summary_textBox.Text");
            // 
            // filename_groupBox
            // 
            filename_groupBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            filename_groupBox.Controls.Add(filename_textBox);
            filename_groupBox.Location = new Point(12, 236);
            filename_groupBox.Name = "filename_groupBox";
            filename_groupBox.Size = new Size(314, 52);
            filename_groupBox.TabIndex = 1;
            filename_groupBox.TabStop = false;
            filename_groupBox.Text = "Filename";
            // 
            // filename_textBox
            // 
            filename_textBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            filename_textBox.Location = new Point(6, 22);
            filename_textBox.Name = "filename_textBox";
            filename_textBox.PlaceholderText = "The summary";
            filename_textBox.ScrollBars = ScrollBars.Both;
            filename_textBox.Size = new Size(302, 23);
            filename_textBox.TabIndex = 0;
            filename_textBox.Text = "Some filename.md";
            // 
            // new_category_groupBox
            // 
            new_category_groupBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            new_category_groupBox.Controls.Add(new_category_textBox);
            new_category_groupBox.Location = new Point(12, 305);
            new_category_groupBox.Name = "new_category_groupBox";
            new_category_groupBox.Size = new Size(314, 52);
            new_category_groupBox.TabIndex = 2;
            new_category_groupBox.TabStop = false;
            new_category_groupBox.Text = "New Category";
            // 
            // new_category_textBox
            // 
            new_category_textBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            new_category_textBox.Location = new Point(6, 22);
            new_category_textBox.Name = "new_category_textBox";
            new_category_textBox.PlaceholderText = "Existing category selected";
            new_category_textBox.ScrollBars = ScrollBars.Both;
            new_category_textBox.Size = new Size(302, 23);
            new_category_textBox.TabIndex = 0;
            // 
            // existing_categories_groupBox
            // 
            existing_categories_groupBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            existing_categories_groupBox.Controls.Add(existing_categories_comboBox);
            existing_categories_groupBox.Location = new Point(12, 380);
            existing_categories_groupBox.Name = "existing_categories_groupBox";
            existing_categories_groupBox.Size = new Size(314, 52);
            existing_categories_groupBox.TabIndex = 3;
            existing_categories_groupBox.TabStop = false;
            existing_categories_groupBox.Text = "Existing Categories";
            // 
            // existing_categories_comboBox
            // 
            existing_categories_comboBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            existing_categories_comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            existing_categories_comboBox.FormattingEnabled = true;
            existing_categories_comboBox.Items.AddRange(new object[] { "Category A", "Category B", "Category C", "N/A" });
            existing_categories_comboBox.Location = new Point(6, 22);
            existing_categories_comboBox.Name = "existing_categories_comboBox";
            existing_categories_comboBox.Size = new Size(302, 23);
            existing_categories_comboBox.TabIndex = 0;
            // 
            // OK_button
            // 
            OK_button.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            OK_button.AutoSize = true;
            OK_button.Location = new Point(222, 453);
            OK_button.Name = "OK_button";
            OK_button.Size = new Size(104, 34);
            OK_button.TabIndex = 4;
            OK_button.Text = "OK";
            OK_button.UseVisualStyleBackColor = true;
            OK_button.Click += OK_button_Click;
            // 
            // reroll_button
            // 
            reroll_button.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            reroll_button.Enabled = false;
            reroll_button.Location = new Point(12, 450);
            reroll_button.Name = "reroll_button";
            reroll_button.Size = new Size(104, 37);
            reroll_button.TabIndex = 5;
            reroll_button.Text = "Re-roll";
            reroll_button.UseVisualStyleBackColor = true;
            reroll_button.Click += reroll_button_Click;
            // 
            // trackBar1
            // 
            trackBar1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            trackBar1.Location = new Point(6, 22);
            trackBar1.Maximum = 200;
            trackBar1.Name = "trackBar1";
            trackBar1.Size = new Size(302, 45);
            trackBar1.TabIndex = 6;
            trackBar1.TickFrequency = 10;
            // 
            // groupBox5
            // 
            groupBox5.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBox5.Controls.Add(trackBar1);
            groupBox5.Enabled = false;
            groupBox5.Location = new Point(12, 56);
            groupBox5.Name = "groupBox5";
            groupBox5.Size = new Size(314, 74);
            groupBox5.TabIndex = 7;
            groupBox5.TabStop = false;
            groupBox5.Text = "Temperature";
            // 
            // illegal_state_label
            // 
            illegal_state_label.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            illegal_state_label.Font = new Font("Consolas", 15.75F, FontStyle.Bold, GraphicsUnit.Point);
            illegal_state_label.ForeColor = Color.Red;
            illegal_state_label.Location = new Point(12, 9);
            illegal_state_label.Name = "illegal_state_label";
            illegal_state_label.Size = new Size(314, 44);
            illegal_state_label.TabIndex = 8;
            illegal_state_label.Text = "Illegal state provided!";
            illegal_state_label.TextAlign = ContentAlignment.MiddleCenter;
            illegal_state_label.Visible = false;
            // 
            // Taxonomy_Form
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(338, 499);
            Controls.Add(illegal_state_label);
            Controls.Add(groupBox5);
            Controls.Add(reroll_button);
            Controls.Add(OK_button);
            Controls.Add(existing_categories_groupBox);
            Controls.Add(new_category_groupBox);
            Controls.Add(filename_groupBox);
            Controls.Add(summary_groupBox);
            Name = "Taxonomy_Form";
            Text = "Taxonomy";
            summary_groupBox.ResumeLayout(false);
            summary_groupBox.PerformLayout();
            filename_groupBox.ResumeLayout(false);
            filename_groupBox.PerformLayout();
            new_category_groupBox.ResumeLayout(false);
            new_category_groupBox.PerformLayout();
            existing_categories_groupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)trackBar1).EndInit();
            groupBox5.ResumeLayout(false);
            groupBox5.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private GroupBox summary_groupBox;
        private TextBox summary_textBox;
        private GroupBox filename_groupBox;
        private TextBox filename_textBox;
        private GroupBox new_category_groupBox;
        private TextBox new_category_textBox;
        private GroupBox existing_categories_groupBox;
        private ComboBox existing_categories_comboBox;
        private Button OK_button;
        private Button reroll_button;
        private TrackBar trackBar1;
        private GroupBox groupBox5;
        private Label illegal_state_label;
    }
}