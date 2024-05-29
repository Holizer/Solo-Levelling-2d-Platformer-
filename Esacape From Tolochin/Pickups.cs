using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Esacape_From_Tolochin.MainForm;
using System.Windows.Forms;
using System.Diagnostics;

namespace Esacape_From_Tolochin
{
    public class Pickup
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public Bitmap Texture { get; set; }
        public Pickup(float x, float y, float width, float height, Bitmap texture)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Texture = texture;
        }
        public RectangleF GetRectangle()
        {
            return new RectangleF(X, Y, Width, Height);
        }
    }
    public class FallingPickup : Pickup
    {
        public float Speed { get; set; }
        public bool IsFalling { get; set; }
        public bool IsCollected { get; set; }

        public FallingPickup(float x, float y, float width, float height, float speed, Bitmap texture)
        : base(x, y, width, height, texture)
        {
            Speed = speed;
            IsFalling = true;
            IsCollected = false;
            Texture = texture;
        }
        public void UpdatePosition()
        {
            if (IsFalling)
            {
                Y += Speed;
            }
        }
    }
}
