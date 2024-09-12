using System;
using static SoloLeveling.MainForm;
using System.Windows.Forms;

namespace SoloLeveling
{
    public partial class MainMenu : Form
    {
        public MainMenu()
        {
            InitializeComponent();

            // Загрузка шрифта
            CastomizeManger.LoadCustomFont();

            // Применяем кастомизацию стиля для кнопок
            CastomizeManger.CastomizeButton(StartGameBTN);
            CastomizeManger.CastomizeButton(SettingsBTN);
            CastomizeManger.CastomizeButton(AboutGameBTN);
            CastomizeManger.CastomizeButton(LeaveGameBTN);
        }
        public Panel GetPanel()
        {
            return MainMenuPanel;
        }


        // Кнопка "START"

        private void StartGameBTN_Click(object sender, EventArgs e)
        {
            gameStarted = true;
        }
        // Кнопка "Настройки"
        private void SettingsBTN_Click(object sender, EventArgs e)
        {
        }

        // Кнопка "Об игре"
        private void AboutGameBTN_Click(object sender, EventArgs e)
        {

        }

        // Кнопка "Выйти"
        private void LeaveGameBTN_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
