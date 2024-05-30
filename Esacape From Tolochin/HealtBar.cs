using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using static SoloLeveling.MainForm;

namespace SoloLeveling
{
    internal class HealthBar
    {
        private int maxHealth;
        private int currentHealth;
        public float barWidth;
        public float barHeight;
        private Pen borderColor;

        private float animationSpeed = 0.1f;
        private float currentAnimatedWidth;

        public HealthBar(int MaxHealth, int BarWidth, int BarHeight, Brush BarColor, Pen BorderColor)
        {
            maxHealth = MaxHealth;
            currentHealth = maxHealth;
            barWidth = BarWidth;
            barHeight = BarHeight;
            borderColor = BorderColor;
            currentAnimatedWidth = barWidth;
        }
        public void UpdateMaxHealth(int newMaxHealth)
        {
            maxHealth = newMaxHealth;
            currentHealth = Math.Min(maxHealth, currentHealth);
        }
        public void UpdateHealth(int currentHealth)
        {
            this.currentHealth = Math.Max(0, currentHealth);
            this.currentHealth = Math.Min(maxHealth, this.currentHealth);

            float targetWidth = (float)this.currentHealth / maxHealth * barWidth;
            currentAnimatedWidth += (targetWidth - currentAnimatedWidth) * animationSpeed;
        }
        public void Draw(Graphics g, Size clientSize, int offsetX)
        {
            float healthPercent = (float)currentHealth / maxHealth;

            int x = (int)(20f + offsetX);
            int y = (int)(20f);

            // Закругленный прямоугольник для закрашенной части полосы HP
            DrawRoundedRectangle(g, borderColor, x, y, barWidth, barHeight, 5);

            if (currentHealth > 0)
            {
                GraphicsPath fillPath = CreateRoundedRectanglePath(x, y, currentAnimatedWidth, barHeight, 5);

                Color startColor = Color.FromArgb(0, 210, 0);
                Color endColor = Color.FromArgb(0, 100, 0);

                LinearGradientBrush fillBrush = new LinearGradientBrush(
                    new Rectangle(x, y, (int)currentAnimatedWidth, (int)barHeight),
                    startColor,
                    endColor,
                    LinearGradientMode.Vertical);

                g.FillPath(fillBrush, fillPath);

                if (!(currentAnimatedWidth == 0))
                {
                    DrawRoundedRectangle(g, borderColor, x, y, currentAnimatedWidth, barHeight, 5);
                }
            }

            string healthText = $"{currentHealth}/{maxHealth}";
            Font baseFont = LoadFont(Path.Combine(resourcesPath, "Font\\Planes_ValMore.ttf"), 7);

            float fontSize = FindAdaptiveFontSize(g, healthText, barWidth, barHeight, clientSize, baseFont);
            Font font = new Font(baseFont.FontFamily, fontSize);
            SizeF textSize = g.MeasureString(healthText, font);

            float textX = x + (barWidth - textSize.Width) / 2;
            float textY = y + (int)((barHeight - textSize.Height) / 1.5);

            g.DrawString(healthText, font, Brushes.White, textX, textY);
        }
        private GraphicsPath CreateRoundedRectanglePath(float x, float y, float width, float height, float radius)
        {
            GraphicsPath path = new GraphicsPath();
            float diameter = radius * 2;

            RectangleF arc = new RectangleF(x, y, diameter, diameter);
            path.AddArc(arc, 180, 90);
            arc.X = x + width - diameter;
            path.AddArc(arc, 270, 90);
            arc.Y = y + height - diameter;
            path.AddArc(arc, 0, 90);
            arc.X = x;
            path.AddArc(arc, 90, 90);

            path.CloseFigure();

            return path;
        }
        private void DrawRoundedRectangle(Graphics g, Pen pen, float x, float y, float width, float height, float radius)
        {
            GraphicsPath path = CreateRoundedRectanglePath(x, y, width, height, radius);
            g.DrawPath(pen, path);
        }
        private static Font LoadFont(string fullPathToFont, float fontSize)
        {
            PrivateFontCollection pfc = new PrivateFontCollection();
            pfc.AddFontFile(fullPathToFont);
            return new Font(pfc.Families[0], fontSize, FontStyle.Regular);
        }
    }
}