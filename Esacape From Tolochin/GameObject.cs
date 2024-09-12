using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoloLeveling
{
    public abstract class GameObject
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public Rectangle Hitbox => new Rectangle(X, Y, Width, Height);

        public GameObject(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }
        public void DrawGameObject(Graphics g)
        {
            g.DrawRectangle(Pens.Blue, Hitbox);
        }
    }

    public class Ground : GameObject
    {
        public int groundLevel { get; set; }
        public Ground(int x, int y, int width, int height)
            : base(x, y, width, height)
        {
            groundLevel = y;
        }
    }
}
