using System.Drawing;
using System.Windows.Forms;
using static SoloLeveling.MainForm;

namespace SoloLeveling
{
    public partial class PauseMenu : Form
    {
        public static bool Active;
        public PauseMenu()
        {
            InitializeComponent();

            LoadCustomFont();
            ContinueGameBTN.FlatAppearance.MouseOverBackColor = Color.Transparent;
            LeaveToMainMenuBTN.FlatAppearance.MouseOverBackColor = Color.Transparent;
            SettingsBTN.FlatAppearance.MouseOverBackColor = Color.Transparent;

            // Устанавливаем прозрачный цвет фона при нажатии
            ContinueGameBTN.FlatAppearance.MouseDownBackColor = Color.Transparent;
            SettingsBTN.FlatAppearance.MouseDownBackColor = Color.Transparent;
            LeaveToMainMenuBTN.FlatAppearance.MouseDownBackColor = Color.Transparent;

            ApplyCustomFont(ContinueGameBTN, "Planes_ValMore", 13);
            ApplyCustomFont(SettingsBTN, "Planes_ValMore", 13);
            ApplyCustomFont(LeaveToMainMenuBTN, "Planes_ValMore", 13);
        }

        public Panel GetPanel()
        {
            return ButtonsPauseMenuPanel;
        }


        private void SettingsBTN_Click(object sender, System.EventArgs e)
        {
            SoundManager.PlayClickSound();
        }

        private void ContinueGameBTN_Click_1(object sender, System.EventArgs e)
        {
            Active = false;
            SoundManager.PlayClickSound();
        }

        private void LeaveToMainMenuBTN_Click_1(object sender, System.EventArgs e)
        {
            Application.Exit();
        }
    }
}
