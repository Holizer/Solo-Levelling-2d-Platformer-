namespace SoloLeveling
{
    partial class WildForest
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WildForest));
            this.Level_WildForset = new System.Windows.Forms.Panel();
            this.groundPicture = new System.Windows.Forms.PictureBox();
            this.Level_WildForset.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groundPicture)).BeginInit();
            this.SuspendLayout();
            // 
            // Level_WildForset
            // 
            this.Level_WildForset.AutoScroll = true;
            this.Level_WildForset.BackgroundImage = global::Esacape_From_Tolochin.Properties.Resources.Component_11;
            this.Level_WildForset.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Level_WildForset.Controls.Add(this.groundPicture);
            this.Level_WildForset.Dock = System.Windows.Forms.DockStyle.Left;
            this.Level_WildForset.Location = new System.Drawing.Point(0, 0);
            this.Level_WildForset.Name = "Level_WildForset";
            this.Level_WildForset.Size = new System.Drawing.Size(1900, 673);
            this.Level_WildForset.TabIndex = 1;
            // 
            // groundPicture
            // 
            this.groundPicture.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("groundPicture.BackgroundImage")));
            this.groundPicture.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groundPicture.Location = new System.Drawing.Point(0, 609);
            this.groundPicture.Name = "groundPicture";
            this.groundPicture.Size = new System.Drawing.Size(1900, 64);
            this.groundPicture.TabIndex = 0;
            this.groundPicture.TabStop = false;
            this.groundPicture.Tag = "ground";
            // 
            // WildForest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1900, 673);
            this.Controls.Add(this.Level_WildForset);
            this.DoubleBuffered = true;
            this.MinimumSize = new System.Drawing.Size(1918, 720);
            this.Name = "WildForest";
            this.Text = "Form1";
            this.Level_WildForset.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groundPicture)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox groundPicture;
        private System.Windows.Forms.Panel Level_WildForset;
    }
}