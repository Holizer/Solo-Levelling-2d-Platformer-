namespace SoloLeveling
{
    partial class Ending
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
            this.EndingPanel = new System.Windows.Forms.Panel();
            this.LeaveGameBTN = new System.Windows.Forms.Button();
            this.Logo = new System.Windows.Forms.PictureBox();
            this.CongratulationLabel = new System.Windows.Forms.Label();
            this.EndingGIF = new System.Windows.Forms.PictureBox();
            this.EndingPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Logo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.EndingGIF)).BeginInit();
            this.SuspendLayout();
            // 
            // EndingPanel
            // 
            this.EndingPanel.BackgroundImage = global::Esacape_From_Tolochin.Properties.Resources.dfetuzx_f10a8a84_5b6e_4581_8279_e7fbc6729eaa1;
            this.EndingPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.EndingPanel.Controls.Add(this.LeaveGameBTN);
            this.EndingPanel.Controls.Add(this.Logo);
            this.EndingPanel.Controls.Add(this.CongratulationLabel);
            this.EndingPanel.Controls.Add(this.EndingGIF);
            this.EndingPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EndingPanel.Location = new System.Drawing.Point(0, 0);
            this.EndingPanel.Name = "EndingPanel";
            this.EndingPanel.Size = new System.Drawing.Size(1262, 673);
            this.EndingPanel.TabIndex = 0;
            // 
            // LeaveGameBTN
            // 
            this.LeaveGameBTN.BackColor = System.Drawing.Color.Transparent;
            this.LeaveGameBTN.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.LeaveGameBTN.Cursor = System.Windows.Forms.Cursors.Hand;
            this.LeaveGameBTN.FlatAppearance.BorderSize = 0;
            this.LeaveGameBTN.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LeaveGameBTN.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.LeaveGameBTN.ForeColor = System.Drawing.Color.Transparent;
            this.LeaveGameBTN.Location = new System.Drawing.Point(64, 548);
            this.LeaveGameBTN.Name = "LeaveGameBTN";
            this.LeaveGameBTN.Size = new System.Drawing.Size(269, 45);
            this.LeaveGameBTN.TabIndex = 4;
            this.LeaveGameBTN.Text = "Выйти из игры";
            this.LeaveGameBTN.UseVisualStyleBackColor = false;
            this.LeaveGameBTN.Click += new System.EventHandler(this.LeaveGameBTN_Click);
            // 
            // Logo
            // 
            this.Logo.BackColor = System.Drawing.Color.Transparent;
            this.Logo.Image = global::Esacape_From_Tolochin.Properties.Resources.Logo2;
            this.Logo.Location = new System.Drawing.Point(34, 258);
            this.Logo.Name = "Logo";
            this.Logo.Size = new System.Drawing.Size(435, 72);
            this.Logo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Logo.TabIndex = 3;
            this.Logo.TabStop = false;
            // 
            // CongratulationLabel
            // 
            this.CongratulationLabel.BackColor = System.Drawing.Color.Transparent;
            this.CongratulationLabel.CausesValidation = false;
            this.CongratulationLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.CongratulationLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.CongratulationLabel.ForeColor = System.Drawing.Color.White;
            this.CongratulationLabel.Location = new System.Drawing.Point(0, 0);
            this.CongratulationLabel.Name = "CongratulationLabel";
            this.CongratulationLabel.Size = new System.Drawing.Size(1262, 111);
            this.CongratulationLabel.TabIndex = 1;
            this.CongratulationLabel.Text = "Спасибо за прохождение техно-альфа-бетта-гамма-штрих-демо версии v0.1";
            this.CongratulationLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // EndingGIF
            // 
            this.EndingGIF.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.EndingGIF.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EndingGIF.Image = global::Esacape_From_Tolochin.Properties.Resources.dfetuzx_f10a8a84_5b6e_4581_8279_e7fbc6729eaa;
            this.EndingGIF.Location = new System.Drawing.Point(0, 0);
            this.EndingGIF.Name = "EndingGIF";
            this.EndingGIF.Size = new System.Drawing.Size(1262, 673);
            this.EndingGIF.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.EndingGIF.TabIndex = 0;
            this.EndingGIF.TabStop = false;
            // 
            // Ending
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1262, 673);
            this.Controls.Add(this.EndingPanel);
            this.Name = "Ending";
            this.Text = "Form1";
            this.EndingPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Logo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.EndingGIF)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel EndingPanel;
        private System.Windows.Forms.Button LeaveGameBTN;
        private System.Windows.Forms.PictureBox Logo;
        private System.Windows.Forms.Label CongratulationLabel;
        private System.Windows.Forms.PictureBox EndingGIF;
    }
}