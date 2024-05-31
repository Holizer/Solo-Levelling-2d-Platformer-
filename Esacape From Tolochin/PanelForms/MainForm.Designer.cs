namespace SoloLeveling
{
    partial class MainForm
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

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.MainMenuPanel = new System.Windows.Forms.Panel();
            this.StartGameBTN = new System.Windows.Forms.Button();
            this.logo = new System.Windows.Forms.PictureBox();
            this.LeaveGameBTN = new System.Windows.Forms.Button();
            this.MainMenuGIF = new System.Windows.Forms.PictureBox();
            this.MainMenuPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.logo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MainMenuGIF)).BeginInit();
            this.SuspendLayout();
            // 
            // MainMenuPanel
            // 
            this.MainMenuPanel.BackgroundImage = global::Esacape_From_Tolochin.Properties.Resources.Main_menu_screen;
            this.MainMenuPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.MainMenuPanel.Controls.Add(this.StartGameBTN);
            this.MainMenuPanel.Controls.Add(this.logo);
            this.MainMenuPanel.Controls.Add(this.LeaveGameBTN);
            this.MainMenuPanel.Controls.Add(this.MainMenuGIF);
            this.MainMenuPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainMenuPanel.Location = new System.Drawing.Point(0, 0);
            this.MainMenuPanel.Name = "MainMenuPanel";
            this.MainMenuPanel.Size = new System.Drawing.Size(1262, 673);
            this.MainMenuPanel.TabIndex = 0;
            // 
            // StartGameBTN
            // 
            this.StartGameBTN.BackColor = System.Drawing.Color.Transparent;
            this.StartGameBTN.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.StartGameBTN.Cursor = System.Windows.Forms.Cursors.Hand;
            this.StartGameBTN.FlatAppearance.BorderSize = 0;
            this.StartGameBTN.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.StartGameBTN.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.StartGameBTN.ForeColor = System.Drawing.SystemColors.Control;
            this.StartGameBTN.Location = new System.Drawing.Point(70, 196);
            this.StartGameBTN.Name = "StartGameBTN";
            this.StartGameBTN.Size = new System.Drawing.Size(238, 45);
            this.StartGameBTN.TabIndex = 1;
            this.StartGameBTN.Text = "Новая игра";
            this.StartGameBTN.UseVisualStyleBackColor = false;
            this.StartGameBTN.Click += new System.EventHandler(this.StartGameBTN_Click);
            // 
            // logo
            // 
            this.logo.BackColor = System.Drawing.Color.Transparent;
            this.logo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.logo.Image = global::Esacape_From_Tolochin.Properties.Resources.Logo1;
            this.logo.Location = new System.Drawing.Point(28, 113);
            this.logo.Name = "logo";
            this.logo.Size = new System.Drawing.Size(340, 60);
            this.logo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.logo.TabIndex = 4;
            this.logo.TabStop = false;
            // 
            // LeaveGameBTN
            // 
            this.LeaveGameBTN.BackColor = System.Drawing.Color.Transparent;
            this.LeaveGameBTN.Cursor = System.Windows.Forms.Cursors.Hand;
            this.LeaveGameBTN.FlatAppearance.BorderSize = 0;
            this.LeaveGameBTN.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LeaveGameBTN.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.LeaveGameBTN.ForeColor = System.Drawing.SystemColors.Control;
            this.LeaveGameBTN.Location = new System.Drawing.Point(70, 266);
            this.LeaveGameBTN.Name = "LeaveGameBTN";
            this.LeaveGameBTN.Size = new System.Drawing.Size(238, 45);
            this.LeaveGameBTN.TabIndex = 2;
            this.LeaveGameBTN.Text = "Выйти из игры";
            this.LeaveGameBTN.UseVisualStyleBackColor = false;
            this.LeaveGameBTN.Click += new System.EventHandler(this.LeaveGameBTN_Click);
            // 
            // MainMenuGIF
            // 
            this.MainMenuGIF.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainMenuGIF.Image = global::Esacape_From_Tolochin.Properties.Resources.Main_menu_screen2;
            this.MainMenuGIF.Location = new System.Drawing.Point(0, 0);
            this.MainMenuGIF.Name = "MainMenuGIF";
            this.MainMenuGIF.Size = new System.Drawing.Size(1262, 673);
            this.MainMenuGIF.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.MainMenuGIF.TabIndex = 5;
            this.MainMenuGIF.TabStop = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1262, 673);
            this.Controls.Add(this.MainMenuPanel);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(1280, 720);
            this.Name = "MainForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Solo Levelling";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.MainMenuPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.logo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MainMenuGIF)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel MainMenuPanel;
        private System.Windows.Forms.Button LeaveGameBTN;
        private System.Windows.Forms.Button StartGameBTN;
        private System.Windows.Forms.PictureBox logo;
        private System.Windows.Forms.PictureBox MainMenuGIF;
    }
}

