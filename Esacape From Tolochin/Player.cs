using System;
using System.Drawing;
using static SoloLeveling.MainForm;
using static SoloLeveling.AnimationManagaer;
using System.Windows.Forms;

namespace SoloLeveling
{
    public class Player
    {
        public int X { get; set; }
        public float XPercent { get; set; }
        public int Y { get; set; }
        public float YPercent { get; set; }
        public float SpeedPrecent { get; set; }
        public int Height { get; set; }
        public float HeightPercent { get; set; }
        public int Width { get; set; }
        public float WidthPercent { get; set; }
        public float VerticalSpeed { get; set; }
        public bool IsJumping { get; set; }
        public bool IsOnGround { get; set; }
        public int MaxHealth { get; set; }
        public int CurrentHealth { get; set; }
        public float JumpForcePercent { get; set; }
        public Bitmap Texture { get; set; }
        public Animation CurrentAnimation { get; set; }
        public AnimationFrame CurrentFrame { get; set; }
        public Rectangle Hitbox { get; set; }
        //adsda

        public int currentSpriteIndex = 0;
        public int Experience { get; private set; }
        public int Level { get; private set; }
        public int ExperienceToNextLevel { get; private set; }

        public DateTime lastAttackTime;

        public DateTime lastChargedAttackTime;

        public TimeSpan attackCooldown = TimeSpan.FromSeconds(0);

        public TimeSpan chargedAttackCooldown = TimeSpan.FromSeconds(0);

        public bool IsChargingAttack = false;

        public bool IsAttack = false;
        public Player(int x, int y, float speedPrecent, int health, float heightPercent, float widthPercent, float xPrecent, float yPrecent, float jumpForcePercent, Bitmap texture)
        {
            X = x;
            Y = y;
            HeightPercent = heightPercent;
            WidthPercent = widthPercent;
            SpeedPrecent = speedPrecent;
            XPercent = xPrecent;
            YPercent = yPrecent;
            MaxHealth = health;
            CurrentHealth = health;
            Texture = texture;
            Experience = 0;
            Level = 1;
            ExperienceToNextLevel = 100;
            JumpForcePercent = jumpForcePercent;
            Hitbox = new Rectangle((int)X, (int)Y, Width, Height);
        }
        public float JumpForce(Size clientSize)
        {
            return JumpForcePercent * clientSize.Height;
        }
        public float Speed(Size clientSize)
        {
            return SpeedPrecent * clientSize.Height;
        }
        public bool IsAFK()
        {
            return !(Keyboard.IsKeyDown(Keys.A) || Keyboard.IsKeyDown(Keys.D));
        }
        public Rectangle GetRectangle(Size clientSize)
        {
            Height = (int)(HeightPercent * clientSize.Height);
            Width = (int)(WidthPercent * clientSize.Width);

            int x = (int)X;
            int y = (int)Y;

            return new Rectangle(x, y, Width, Height);
        }
        public void TakeDamage(int damage)
        {
            CurrentHealth -= damage;
        }

        public void GainExperience(int amount)
        {
            Experience += amount;
            if (Experience >= ExperienceToNextLevel)
            {
                LevelUp();
            }
        }
        private void LevelUp()
        {
            LevelUpSound.Play();
            Level++;
            MaxHealth += 20;
            Experience = 0;
            ExperienceToNextLevel = 100 * Level;
            healthBar.UpdateMaxHealth(MaxHealth);
        }

        public bool IsDead()
        {
            return CurrentHealth <= 0;
        }
        public void DrawPlayer_HitBox(Graphics g, Size clientsize)
        {
            if (CurrentFrame.Frame != null)
            {
                int x = (int)CurrentFrame.DisplayRectangle.X + X;
                int y = (int)CurrentFrame.DisplayRectangle.Y + Y;

                g.DrawRectangle(Pens.Red, GetRectangle(clientsize));
            }
        }
        public Direction CurrentDirection = Direction.Right;
        public enum Direction
        {
            Left,
            Right
        }
        public void RestoreHealth(int amount)
        {
            CurrentHealth = Math.Min(MaxHealth, CurrentHealth + amount);
        }
    }
}
