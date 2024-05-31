using System.Drawing;
using System.Windows.Forms;
using static SoloLeveling.MainForm;

namespace SoloLeveling
{
    public partial class Ending : Form
    {
        public Ending()
        {
            InitializeComponent();

            LoadCustomFont();
            ApplyCustomFont(CongratulationLabel, "Planes_ValMore", 18);
            ApplyCustomFont(LeaveGameBTN, "Planes_ValMore", 18);

            LeaveGameBTN.FlatAppearance.MouseOverBackColor = Color.Transparent;
            LeaveGameBTN.FlatAppearance.MouseDownBackColor = Color.Transparent;
        }
        public Panel GetPanel()
        {
            return EndingPanel;
        }

        private void LeaveGameBTN_Click(object sender, System.EventArgs e)
        {
            Application.Exit();
        }
    }
}
