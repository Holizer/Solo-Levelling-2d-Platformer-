namespace SoloLeveling
{
    partial class MainMenu
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
            this.MainMenuPanel = new System.Windows.Forms.Panel();
            this.MainMenuBG = new System.Windows.Forms.PictureBox();
            this.MenuLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.LeaveGameBTN = new System.Windows.Forms.Button();
            this.AboutGameBTN = new System.Windows.Forms.Button();
            this.SettingsBTN = new System.Windows.Forms.Button();
            this.StartGameBTN = new System.Windows.Forms.Button();
            this.Logo = new System.Windows.Forms.PictureBox();
            this.MainMenuPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MainMenuBG)).BeginInit();
            this.MenuLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Logo)).BeginInit();
            this.SuspendLayout();
            // 
            // MainMenuPanel
            // 
            this.MainMenuPanel.BackgroundImage = global::Esacape_From_Tolochin.Properties.Resources.Main_menu_screen;
            this.MainMenuPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.MainMenuPanel.Controls.Add(this.MenuLayoutPanel);
            this.MainMenuPanel.Controls.Add(this.MainMenuBG);
            this.MainMenuPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainMenuPanel.Location = new System.Drawing.Point(0, 0);
            this.MainMenuPanel.Name = "MainMenuPanel";
            this.MainMenuPanel.Size = new System.Drawing.Size(1262, 673);
            this.MainMenuPanel.TabIndex = 0;
            // 
            // MainMenuBG
            // 
            this.MainMenuBG.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainMenuBG.Image = global::Esacape_From_Tolochin.Properties.Resources.Main_menu_screen;
            this.MainMenuBG.Location = new System.Drawing.Point(0, 0);
            this.MainMenuBG.Name = "MainMenuBG";
            this.MainMenuBG.Size = new System.Drawing.Size(1262, 673);
            this.MainMenuBG.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.MainMenuBG.TabIndex = 0;
            this.MainMenuBG.TabStop = false;
            // 
            // MenuLayoutPanel
            // 
            this.MenuLayoutPanel.BackColor = System.Drawing.Color.Transparent;
            this.MenuLayoutPanel.ColumnCount = 1;
            this.MenuLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.MenuLayoutPanel.Controls.Add(this.Logo, 0, 0);
            this.MenuLayoutPanel.Controls.Add(this.AboutGameBTN, 0, 3);
            this.MenuLayoutPanel.Controls.Add(this.LeaveGameBTN, 0, 4);
            this.MenuLayoutPanel.Controls.Add(this.SettingsBTN, 0, 2);
            this.MenuLayoutPanel.Controls.Add(this.StartGameBTN, 0, 1);
            this.MenuLayoutPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.MenuLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.MenuLayoutPanel.Name = "MenuLayoutPanel";
            this.MenuLayoutPanel.RowCount = 6;
            this.MenuLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.MenuLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.MenuLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.MenuLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.MenuLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.MenuLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.MenuLayoutPanel.Size = new System.Drawing.Size(533, 673);
            this.MenuLayoutPanel.TabIndex = 1;
            // 
            // LeaveGameBTN
            // 
            this.LeaveGameBTN.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.LeaveGameBTN.BackColor = System.Drawing.Color.Transparent;
            this.LeaveGameBTN.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.LeaveGameBTN.Cursor = System.Windows.Forms.Cursors.Hand;
            this.LeaveGameBTN.FlatAppearance.BorderSize = 0;
            this.LeaveGameBTN.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LeaveGameBTN.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.LeaveGameBTN.ForeColor = System.Drawing.Color.Transparent;
            this.LeaveGameBTN.Location = new System.Drawing.Point(132, 481);
            this.LeaveGameBTN.Name = "LeaveGameBTN";
            this.LeaveGameBTN.Size = new System.Drawing.Size(269, 45);
            this.LeaveGameBTN.TabIndex = 5;
            this.LeaveGameBTN.Text = "Выйти из игры";
            this.LeaveGameBTN.UseVisualStyleBackColor = false;
            this.LeaveGameBTN.Click += new System.EventHandler(this.LeaveGameBTN_Click);
            // 
            // AboutGameBTN
            // 
            this.AboutGameBTN.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.AboutGameBTN.BackColor = System.Drawing.Color.Transparent;
            this.AboutGameBTN.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.AboutGameBTN.Cursor = System.Windows.Forms.Cursors.Hand;
            this.AboutGameBTN.FlatAppearance.BorderSize = 0;
            this.AboutGameBTN.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AboutGameBTN.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.AboutGameBTN.ForeColor = System.Drawing.Color.Transparent;
            this.AboutGameBTN.Location = new System.Drawing.Point(132, 414);
            this.AboutGameBTN.Name = "AboutGameBTN";
            this.AboutGameBTN.Size = new System.Drawing.Size(269, 45);
            this.AboutGameBTN.TabIndex = 5;
            this.AboutGameBTN.Text = "Об игре";
            this.AboutGameBTN.UseVisualStyleBackColor = false;
            this.AboutGameBTN.Click += new System.EventHandler(this.AboutGameBTN_Click);
            // 
            // SettingsBTN
            // 
            this.SettingsBTN.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.SettingsBTN.BackColor = System.Drawing.Color.Transparent;
            this.SettingsBTN.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.SettingsBTN.Cursor = System.Windows.Forms.Cursors.Hand;
            this.SettingsBTN.FlatAppearance.BorderSize = 0;
            this.SettingsBTN.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SettingsBTN.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.SettingsBTN.ForeColor = System.Drawing.Color.Transparent;
            this.SettingsBTN.Location = new System.Drawing.Point(132, 347);
            this.SettingsBTN.Name = "SettingsBTN";
            this.SettingsBTN.Size = new System.Drawing.Size(269, 45);
            this.SettingsBTN.TabIndex = 6;
            this.SettingsBTN.Text = "Настройки";
            this.SettingsBTN.UseVisualStyleBackColor = false;
            this.SettingsBTN.Click += new System.EventHandler(this.SettingsBTN_Click);
            // 
            // StartGameBTN
            // 
            this.StartGameBTN.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.StartGameBTN.BackColor = System.Drawing.Color.Transparent;
            this.StartGameBTN.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.StartGameBTN.Cursor = System.Windows.Forms.Cursors.Hand;
            this.StartGameBTN.FlatAppearance.BorderSize = 0;
            this.StartGameBTN.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.StartGameBTN.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.StartGameBTN.ForeColor = System.Drawing.Color.Transparent;
            this.StartGameBTN.Location = new System.Drawing.Point(132, 280);
            this.StartGameBTN.Name = "StartGameBTN";
            this.StartGameBTN.Size = new System.Drawing.Size(269, 45);
            this.StartGameBTN.TabIndex = 7;
            this.StartGameBTN.Text = "Начть игру";
            this.StartGameBTN.UseVisualStyleBackColor = false;
            this.StartGameBTN.Click += new System.EventHandler(this.StartGameBTN_Click);
            // 
            // Logo
            // 
            this.Logo.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Logo.BackColor = System.Drawing.Color.Transparent;
            this.Logo.Image = global::Esacape_From_Tolochin.Properties.Resources.Logo2;
            this.Logo.Location = new System.Drawing.Point(49, 98);
            this.Logo.Name = "Logo";
            this.Logo.Size = new System.Drawing.Size(435, 72);
            this.Logo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Logo.TabIndex = 4;
            this.Logo.TabStop = false;
            // 
            // MainMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1262, 673);
            this.Controls.Add(this.MainMenuPanel);
            this.Name = "MainMenu";
            this.Text = "Form1";
            this.MainMenuPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MainMenuBG)).EndInit();
            this.MenuLayoutPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Logo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel MainMenuPanel;
        private System.Windows.Forms.PictureBox MainMenuBG;
        private System.Windows.Forms.TableLayoutPanel MenuLayoutPanel;
        private System.Windows.Forms.Button AboutGameBTN;
        private System.Windows.Forms.Button LeaveGameBTN;
        private System.Windows.Forms.Button SettingsBTN;
        private System.Windows.Forms.Button StartGameBTN;
        private System.Windows.Forms.PictureBox Logo;
    }
}