using System.Drawing;

namespace SoloLeveling
{
    public class Pickup
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Bitmap Texture { get; set; }
        public Pickup(int x, int y, int width, int height, Bitmap texture)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Texture = texture;
        }
        public Rectangle GetRectangle()
        {
            return new Rectangle(X, Y, Width, Height);
        }
    }
    public class FallingPickup : Pickup
    {
        public int Speed { get; set; }
        public bool IsFalling { get; set; }
        public bool IsCollected { get; set; }

        public FallingPickup(int x, int y, int width, int height, int speed, Bitmap texture) : base(x, y, width, height, texture)
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
