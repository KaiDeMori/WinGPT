namespace WinGPT
{
    partial class Config_UI
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
            config_ui_propertyGrid = new PropertyGrid();
            SuspendLayout();
            // 
            // config_ui_propertyGrid
            // 
            config_ui_propertyGrid.Dock = DockStyle.Fill;
            config_ui_propertyGrid.Location = new Point(0, 0);
            config_ui_propertyGrid.Name = "config_ui_propertyGrid";
            config_ui_propertyGrid.PropertySort = PropertySort.NoSort;
            config_ui_propertyGrid.Size = new Size(397, 299);
            config_ui_propertyGrid.TabIndex = 0;
            // 
            // Config_UI
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(397, 299);
            Controls.Add(config_ui_propertyGrid);
            Name = "Config_UI";
            Text = "Config_UI";
            ResumeLayout(false);
        }

        #endregion

        private PropertyGrid config_ui_propertyGrid;
    }
}