using System;
using System.Drawing;
using System.Windows.Forms;
using static SoloLeveling.MainForm;

namespace SoloLeveling
{
    public partial class WildForest : Form
    {
        public WildForest()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            DoubleBuffered = true;
        }
        public Panel GetPanel()
        {
            return Level_WildForset;
        }
    }
}
