namespace SoloLeveling
{
    partial class PauseMenu
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PauseMenu));
            this.PausePanel = new System.Windows.Forms.Panel();
            this.PausePanelBG = new System.Windows.Forms.PictureBox();
            this.ButtonsPauseMenuPanel = new System.Windows.Forms.TableLayoutPanel();
            this.ContinueGameBTN = new System.Windows.Forms.Button();
            this.SettingsBTN = new System.Windows.Forms.Button();
            this.LeaveToMainMenuBTN = new System.Windows.Forms.Button();
            this.PausePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PausePanelBG)).BeginInit();
            this.ButtonsPauseMenuPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // PausePanel
            // 
            this.PausePanel.Controls.Add(this.ButtonsPauseMenuPanel);
            this.PausePanel.Controls.Add(this.PausePanelBG);
            this.PausePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PausePanel.Location = new System.Drawing.Point(0, 0);
            this.PausePanel.Name = "PausePanel";
            this.PausePanel.Size = new System.Drawing.Size(1262, 673);
            this.PausePanel.TabIndex = 0;
            // 
            // PausePanelBG
            // 
            this.PausePanelBG.BackColor = System.Drawing.Color.Transparent;
            this.PausePanelBG.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PausePanelBG.Location = new System.Drawing.Point(0, 0);
            this.PausePanelBG.Name = "PausePanelBG";
            this.PausePanelBG.Size = new System.Drawing.Size(1262, 673);
            this.PausePanelBG.TabIndex = 0;
            this.PausePanelBG.TabStop = false;
            // 
            // ButtonsPauseMenuPanel
            // 
            this.ButtonsPauseMenuPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonsPauseMenuPanel.BackgroundImage = global::Esacape_From_Tolochin.Properties.Resources.panelbg;
            this.ButtonsPauseMenuPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ButtonsPauseMenuPanel.ColumnCount = 1;
            this.ButtonsPauseMenuPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ButtonsPauseMenuPanel.Controls.Add(this.LeaveToMainMenuBTN, 0, 2);
            this.ButtonsPauseMenuPanel.Controls.Add(this.SettingsBTN, 0, 1);
            this.ButtonsPauseMenuPanel.Controls.Add(this.ContinueGameBTN, 0, 0);
            this.ButtonsPauseMenuPanel.Location = new System.Drawing.Point(365, 166);
            this.ButtonsPauseMenuPanel.Name = "ButtonsPauseMenuPanel";
            this.ButtonsPauseMenuPanel.Padding = new System.Windows.Forms.Padding(10);
            this.ButtonsPauseMenuPanel.RowCount = 3;
            this.ButtonsPauseMenuPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.ButtonsPauseMenuPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.ButtonsPauseMenuPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.ButtonsPauseMenuPanel.Size = new System.Drawing.Size(534, 314);
            this.ButtonsPauseMenuPanel.TabIndex = 1;
            // 
            // ContinueGameBTN
            // 
            this.ContinueGameBTN.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.ContinueGameBTN.BackColor = System.Drawing.Color.Transparent;
            this.ContinueGameBTN.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ContinueGameBTN.BackgroundImage")));
            this.ContinueGameBTN.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ContinueGameBTN.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ContinueGameBTN.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.ContinueGameBTN.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ContinueGameBTN.Location = new System.Drawing.Point(92, 84);
            this.ContinueGameBTN.Name = "ContinueGameBTN";
            this.ContinueGameBTN.Size = new System.Drawing.Size(350, 40);
            this.ContinueGameBTN.TabIndex = 7;
            this.ContinueGameBTN.Text = "Продолжить";
            this.ContinueGameBTN.UseVisualStyleBackColor = false;
            // 
            // SettingsBTN
            // 
            this.SettingsBTN.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.SettingsBTN.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("SettingsBTN.BackgroundImage")));
            this.SettingsBTN.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.SettingsBTN.Cursor = System.Windows.Forms.Cursors.Hand;
            this.SettingsBTN.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.SettingsBTN.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.SettingsBTN.Location = new System.Drawing.Point(92, 136);
            this.SettingsBTN.Name = "SettingsBTN";
            this.SettingsBTN.Size = new System.Drawing.Size(350, 40);
            this.SettingsBTN.TabIndex = 8;
            this.SettingsBTN.Text = "Настройки";
            this.SettingsBTN.UseVisualStyleBackColor = true;
            // 
            // LeaveToMainMenuBTN
            // 
            this.LeaveToMainMenuBTN.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.LeaveToMainMenuBTN.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("LeaveToMainMenuBTN.BackgroundImage")));
            this.LeaveToMainMenuBTN.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.LeaveToMainMenuBTN.Cursor = System.Windows.Forms.Cursors.Hand;
            this.LeaveToMainMenuBTN.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.LeaveToMainMenuBTN.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.LeaveToMainMenuBTN.Location = new System.Drawing.Point(92, 188);
            this.LeaveToMainMenuBTN.Name = "LeaveToMainMenuBTN";
            this.LeaveToMainMenuBTN.Size = new System.Drawing.Size(350, 40);
            this.LeaveToMainMenuBTN.TabIndex = 9;
            this.LeaveToMainMenuBTN.Text = "Выйти из игры";
            this.LeaveToMainMenuBTN.UseVisualStyleBackColor = true;
            // 
            // PauseMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1262, 673);
            this.Controls.Add(this.PausePanel);
            this.MinimumSize = new System.Drawing.Size(1280, 720);
            this.Name = "PauseMenu";
            this.Text = "Form1";
            this.PausePanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PausePanelBG)).EndInit();
            this.ButtonsPauseMenuPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel PausePanel;
        private System.Windows.Forms.PictureBox PausePanelBG;
        private System.Windows.Forms.TableLayoutPanel ButtonsPauseMenuPanel;
        private System.Windows.Forms.Button ContinueGameBTN;
        private System.Windows.Forms.Button SettingsBTN;
        private System.Windows.Forms.Button LeaveToMainMenuBTN;
    }
}