using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static SoloLeveling.MainForm;

namespace SoloLeveling
{
    public class CastomizeManger
    {
        public static void CastomizeButton(Button button, int FontSize = 18)
        {
            ApplyCustomFont(button, "Planes_ValMore", FontSize);

            // Устанавливаем прозрачный цвет фона при наведении
            button.FlatAppearance.MouseOverBackColor = Color.Transparent;

            // Устанавливаем прозрачный цвет фона при нажатии
            button.FlatAppearance.MouseDownBackColor = Color.Transparent;
        }
        public static void LoadCustomFont()
        {
            privateFontCollection.AddFontFile(Path.Combine(resourcesPath, "Font\\8_bit_Limit.ttf"));
            privateFontCollection.AddFontFile(Path.Combine(resourcesPath, "Font\\Planes_ValMore.ttf"));
        }
        public static void ApplyCustomFont(Control control, string fontFileName, float fontSize)
        {
            if (privateFontCollection.Families.Any(f => f.Name == fontFileName))
            {
                Font customFont = new Font(privateFontCollection.Families.First(f => f.Name == fontFileName), fontSize);
                control.Font = customFont;
            }
        }
    }
}
