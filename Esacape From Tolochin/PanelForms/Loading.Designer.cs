namespace SoloLeveling
{
    partial class Loading
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
            this.LoadingPanel = new System.Windows.Forms.Panel();
            this.LoadingLabel = new System.Windows.Forms.Label();
            this.LoadingGIF = new System.Windows.Forms.PictureBox();
            this.LoadingPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LoadingGIF)).BeginInit();
            this.SuspendLayout();
            // 
            // LoadingPanel
            // 
            this.LoadingPanel.BackColor = System.Drawing.SystemColors.Control;
            this.LoadingPanel.BackgroundImage = global::Esacape_From_Tolochin.Properties.Resources.loading_screen1;
            this.LoadingPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.LoadingPanel.Controls.Add(this.LoadingLabel);
            this.LoadingPanel.Controls.Add(this.LoadingGIF);
            this.LoadingPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LoadingPanel.Location = new System.Drawing.Point(0, 0);
            this.LoadingPanel.Name = "LoadingPanel";
            this.LoadingPanel.Size = new System.Drawing.Size(1262, 673);
            this.LoadingPanel.TabIndex = 0;
            // 
            // LoadingLabel
            // 
            this.LoadingLabel.BackColor = System.Drawing.Color.Transparent;
            this.LoadingLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.LoadingLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.LoadingLabel.ForeColor = System.Drawing.SystemColors.Control;
            this.LoadingLabel.Location = new System.Drawing.Point(0, 592);
            this.LoadingLabel.Name = "LoadingLabel";
            this.LoadingLabel.Size = new System.Drawing.Size(1262, 81);
            this.LoadingLabel.TabIndex = 2;
            this.LoadingLabel.Text = "Загрузка";
            this.LoadingLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LoadingGIF
            // 
            this.LoadingGIF.BackColor = System.Drawing.Color.Transparent;
            this.LoadingGIF.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.LoadingGIF.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LoadingGIF.Image = global::Esacape_From_Tolochin.Properties.Resources.loading_screen1;
            this.LoadingGIF.Location = new System.Drawing.Point(0, 0);
            this.LoadingGIF.Name = "LoadingGIF";
            this.LoadingGIF.Size = new System.Drawing.Size(1262, 673);
            this.LoadingGIF.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.LoadingGIF.TabIndex = 0;
            this.LoadingGIF.TabStop = false;
            // 
            // Loading
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1262, 673);
            this.Controls.Add(this.LoadingPanel);
            this.Name = "Loading";
            this.Text = "Form1";
            this.LoadingPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.LoadingGIF)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel LoadingPanel;
        private System.Windows.Forms.PictureBox LoadingGIF;
        private System.Windows.Forms.Label LoadingLabel;
    }
}