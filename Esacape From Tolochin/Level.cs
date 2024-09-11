using System;
using System.Drawing;
using System.Diagnostics;
using static SoloLeveling.MainForm;
using System.Windows.Forms;

namespace SoloLeveling
{
    public class Level
    {
        public Panel Panel { get; private set; }
        public int LevelLength { get; private set; }
        public Level(Panel panel)
        {
            Panel = panel;
        }
    }
}
