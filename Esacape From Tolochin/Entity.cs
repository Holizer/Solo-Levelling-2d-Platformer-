using System;
using System.Drawing;
using System.Diagnostics;
using static SoloLeveling.Enemy;
using static SoloLeveling.MainForm;
using static SoloLeveling.AnimationManagaer;

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
    public class EnemyMovement
    {
        public static void UpdateEnemies()
        {
            foreach (var enemy in enemies)
            {
                // Расчет расстояния между игроком и врагом
                float distanceX = player.X - enemy.X;
                float distanceY = player.Y - enemy.Y;
                double distanceToPlayer = Math.Sqrt(distanceX * distanceX + distanceY * distanceY);

                // Логика для прыжка врага, когда игрок близко
                if (distanceToPlayer < clientSize.Width * 0.3 && enemy.IsOnGround && !player.IsDead())
                {
                    enemy.JumpDirectionX = player.X < enemy.X ? -1 : 1;
                    enemy.CurrentDirection = enemy.JumpDirectionX < 0 ? EnemyDirection.Left : EnemyDirection.Right;

                    if (!enemy.JumpTimer.IsRunning)
                    {
                        enemy.JumpTimer.Restart();
                    }

                    if (enemy.JumpTimer.ElapsedMilliseconds >= 500 && !enemy.IsJumping && enemy.IsOnGround)
                    {
                        enemy.IsJumping = true;
                        enemy.VerticalSpeed = -enemy.JumpForce(clientSize) / 4;
                        enemy.JumpTimer.Reset();
                    }
                }
                else
                {
                    // Логика для движения врага по платформам
                    enemy.JumpTimer.Stop();
                    enemy.JumpTimer.Reset();

                    if (!enemy.IsJumping && enemy.IsOnGround)
                    {
                        Rectangle enemyRect = new Rectangle(enemy.X, enemy.Y, enemy.Width, enemy.Height);

                        bool PlatformCollision = false;

                        if (enemy.X <= 0 || enemy.X + enemy.Width >= LevelLength)
                        {
                            enemy.HorizontalSpeed = -enemy.HorizontalSpeed;
                        }

                        foreach (var platformRect in platforms)
                        {
                            var rect = platformRect.ToRectangle(clientSize);

                            if (CheckCollision(enemy.X, enemy.Y, enemy.Width, enemy.Height, rect.X, rect.Y, rect.Width, rect.Height))
                            {
                                PlatformCollision = true;

                                if (enemyRect.Left < rect.Left)
                                {
                                    enemy.X = (int)rect.Left - enemy.Width;
                                    enemy.HorizontalSpeed = -enemy.HorizontalSpeed;
                                    enemy.CurrentDirection = EnemyDirection.Left;
                                }
                                else
                                {
                                    enemy.X = (int)rect.Right;
                                    enemy.HorizontalSpeed = -enemy.HorizontalSpeed;
                                    enemy.CurrentDirection = EnemyDirection.Right;
                                }
                            }
                        }

                        // Логика для определения направления движения врага
                        if (enemy.HorizontalSpeed < 0)
                        {
                            enemy.CurrentDirection = EnemyDirection.Left;
                        }
                        else if (enemy.HorizontalSpeed > 0)
                        {
                            enemy.CurrentDirection = EnemyDirection.Right;
                        }

                        enemy.X += enemy.HorizontalSpeed;

                        if (!PlatformCollision)
                        {
                            enemy.IsOnGround = false;
                            enemy.IsJumping = true;
                        }
                    }
                }

                if (enemy.IsJumping || !enemy.IsOnGround)
                {

                    if (enemy.X <= 0 || enemy.X - enemy.Width >= LevelLength)
                    {
                        enemy.HorizontalSpeed = -enemy.HorizontalSpeed;
                    }

                    int proposedX = enemy.X;
                    bool collisionWithPlatform = false;
                    int proposedY = enemy.Y + (int)enemy.VerticalSpeed;

                    enemy.VerticalSpeed += (int)Gravity;
                    enemy.IsOnGround = enemy.IsJumping ? false : true;

                    if (distanceToPlayer < clientSize.Width * 0.3 && player.IsOnGround && !player.IsDead())
                    {
                        proposedX = enemy.X + enemy.JumpSpeed * enemy.JumpDirectionX;
                        enemy.X = proposedX;
                    }

                    RectangleF enemyRect = new RectangleF(proposedX, proposedY, enemy.Width, enemy.Height);

                    foreach (var groundRect in ground)
                    {
                        var rect = groundRect.ToRectangle(clientSize);
                        if (enemyRect.IntersectsWith(rect))
                        {
                            enemy.IsOnGround = true;
                            enemy.IsJumping = false;
                            enemy.VerticalSpeed = 0;
                            enemy.Y = (int)rect.Y - enemy.Height;
                            collisionWithPlatform = true;
                            break;
                        }
                    }

                    foreach (var platformRect in platforms)
                    {
                        var rect = platformRect.ToRectangle(clientSize);
                        if (enemyRect.IntersectsWith(rect))
                        {
                            bool fromTop = enemyRect.Bottom >= rect.Top && enemyRect.Bottom <= rect.Top + (int)(enemy.VerticalSpeed + Gravity);

                            if (enemyRect.Left < rect.Left && enemyRect.Bottom <= rect.Bottom && !fromTop)
                            {
                                enemy.X = (int)rect.Left - enemy.Width;
                                enemy.HorizontalSpeed = -enemy.HorizontalSpeed;
                                enemy.CurrentDirection = EnemyDirection.Left;
                            }
                            else if (enemyRect.Right > rect.Right && !fromTop)
                            {
                                enemy.X = (int)rect.Right;
                                enemy.HorizontalSpeed = -enemy.HorizontalSpeed;
                                enemy.CurrentDirection = EnemyDirection.Right;
                            }
                            else
                            {
                                if (enemy.VerticalSpeed < 0 && enemyRect.Bottom >= rect.Bottom)
                                {
                                    enemy.Y = (int)rect.Bottom;
                                    collisionWithPlatform = true;
                                    enemy.IsJumping = true;
                                    enemy.IsOnGround = false;
                                    enemy.VerticalSpeed = 0;
                                }
                                else if (fromTop)
                                {
                                    collisionWithPlatform = true;
                                    enemy.IsJumping = false;
                                    enemy.IsOnGround = true;
                                }
                                if (enemyRect.Bottom > rect.Top && enemyRect.Bottom < rect.Bottom)
                                {
                                    collisionWithPlatform = true;
                                    enemy.Y = (int)rect.Top - enemy.Height;
                                    enemy.VerticalSpeed = 0;
                                    enemy.IsJumping = false;
                                    enemy.IsOnGround = true;
                                }
                            }
                        }
                    }
                    if (!collisionWithPlatform)
                    {
                        enemy.Y = proposedY;
                    }
                }
            }
        }
    }
}
