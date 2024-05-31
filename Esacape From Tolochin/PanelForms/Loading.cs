using System;
using System.Windows.Forms;
using static SoloLeveling.MainForm;

namespace SoloLeveling
{
    public partial class Loading : Form
    {
        private Timer loadingAnimationTimer;
        private int loadingAnimationTick = 0;
        public Loading()
        {
            InitializeComponent();

            loadingAnimationTimer = new Timer();
            loadingAnimationTimer.Interval = 350;
            loadingAnimationTimer.Tick += LoadingAnimationTimer_Tick;
            loadingAnimationTimer.Start();

            LoadCustomFont();
            ApplyCustomFont(LoadingLabel, "Planes_ValMore", 18);
            loadingAnimationTick++;
        }

        private string[] loadingPhrases = { "Загрузка", "Загрузка.", "Загрузка. .", "Загрузка. . ." };
        private int loadingPhraseIndex = 0;
        private void LoadingAnimationTimer_Tick(object sender, EventArgs e)
        {
            LoadingLabel.Text = loadingPhrases[loadingPhraseIndex];
            loadingPhraseIndex = (loadingPhraseIndex + 1) % loadingPhrases.Length;
        }
        public Panel GetPanel()
        {
            return LoadingPanel;
        }
    }
}
