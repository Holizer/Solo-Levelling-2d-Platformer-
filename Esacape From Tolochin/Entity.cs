using System.Drawing;
using static SoloLeveling.AnimationManagaer;
using System.Diagnostics;

namespace SoloLeveling
{
    public class Enemy
    {
        public int X { get; set; }
        public float XPercent { get; set; }
        public int Y { get; set; }
        public float YPercent { get; set; }
        public int HorizontalSpeed { get; set; }
        public int JumpSpeed { get; set; }
        public int Height { get; set; }
        public float HeightPercent { get; set; }
        public int Width { get; set; }
        public float WidthPercent { get; set; }
        public int JumpDirectionX { get; set; }
        public int VerticalSpeed { get; set; }
        public bool IsJumping { get; set; }
        public bool IsOnGround { get; set; }
        public float JumpForcePercent { get; set; }
        public int MaxHealth { get; set; }
        public int CurrentHealth { get; set; }
        public Stopwatch JumpTimer { get; set; }
        public Stopwatch LastTimeSpottedPlayer { get; set; }
        public Stopwatch AttackCooldown { get; set; }
        public Stopwatch KnockbackTimer { get; set; }
        public int KnockbackDuration { get; set; }
        public Animation CurrentAnimation { get; set; }
        public AnimationFrame CurrentFrame { get; set; }
        public Rectangle Hitbox { get; set; }

        public int currentSpriteIndex = 0;
        public Enemy(int x, int y, int horizontalSpeed, int jumpSpeed, int health, float heightPercent, float widthPercent, float xPrecent, float yPrecent, float jumpForcePercent)
        {
            X = x;
            XPercent = xPrecent;
            Y = y;
            YPercent = yPrecent;

            WidthPercent = widthPercent;
            HeightPercent = heightPercent;

            HorizontalSpeed = horizontalSpeed;
            JumpSpeed = jumpSpeed;

            JumpForcePercent = jumpForcePercent;
            JumpTimer = new Stopwatch();
            JumpTimer.Start();

            AttackCooldown = new Stopwatch();
            AttackCooldown.Start();

            KnockbackTimer = new Stopwatch();
            KnockbackDuration = 500;

            MaxHealth = health;
            CurrentHealth = health;

            Hitbox = new Rectangle((int)X, (int)Y, Width, Height);
        }
        public Rectangle GetRectangle(Size clientSize)
        {
            Height = (int)(HeightPercent * clientSize.Height);
            Width = (int)(WidthPercent * clientSize.Width);

            int x = (int)X;
            int y = (int)Y;

            return new Rectangle(x, y, Width, Height);
        }
        public void DrawEnemy_HitBox(Graphics g, Size clientsize)
        {
            if (CurrentFrame.Frame != null)
            {
                int x = (int)CurrentFrame.DisplayRectangle.X + X;
                int y = (int)CurrentFrame.DisplayRectangle.Y + Y;

                g.DrawRectangle(Pens.Red, GetRectangle(clientsize));
            }
        }
        public EnemyDirection CurrentDirection { get; set; }
        public enum EnemyDirection
        {
            Left,
            Right
        }
        public int JumpForce(Size clientSize)
        {
            return (int)(JumpForcePercent * clientSize.Height);
        }
        public void TakeDamage(int damage)
        {
            CurrentHealth -= damage;
        }
        public bool IsDead()
        {
            return CurrentHealth <= 0;
        }
    }
}
