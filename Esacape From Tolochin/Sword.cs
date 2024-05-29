using System;
using System.Drawing;

namespace Esacape_From_Tolochin
{
    internal class Sword
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public float PrecentWidth { get; set; }
        public float PrecentHeight { get; set; }
        public int Damage { get; set; }

        public bool IsAttacking;
        public Bitmap Texture { get; set; }
        public Sword(int x, int y, float precentWidth, float precentHeigth, int damage)
        {
            X = x;
            Y = y;
            PrecentWidth = precentWidth;
            PrecentHeight = precentHeigth;
            Damage = damage;
        }
        public Rectangle GetRectangle(Size clientSize)
        {
            Height = (int)(PrecentHeight * clientSize.Height);
            Width = (int)(PrecentWidth * clientSize.Width);

            return new Rectangle((int)X, (int)Y, Width, Height);
        }
    }
}
