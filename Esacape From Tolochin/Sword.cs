using System;
using System.Drawing;
using static SoloLeveling.MainForm;

namespace SoloLeveling
{
    public class Sword
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
    public class SwordAttack
    {
        public static DateTime chargedAttackStartTime;

        public static DateTime AttackStartTime;

        public static void AttackWithSword()
        {
            if (player.IsAttack)
            {
                return;
            }

            if (!player.IsAttack)
            {
                prevJumpForcePercent = player.JumpForcePercent;
                prevSpeedPrecent = player.SpeedPrecent;
            }

            AttackStartTime = DateTime.Now;

            player.IsAttack = true;

            player.lastAttackTime = DateTime.Now;
        }
        public static void Attack()
        {
            int knockbackDistance = (int)(clientWidth * 0.1);
            int knockbackCooldown = 10;

            Rectangle swordRect = new Rectangle(sword.X, sword.Y, sword.Width, sword.Height);

            foreach (var enemy in enemies)
            {
                Rectangle enemyRect = enemy.GetRectangle(clientSize);

                if (swordRect.IntersectsWith(enemyRect))
                {
                    HitDetectedSound.Play();
                    enemy.TakeDamage(sword.Damage);

                    if (enemy.IsDead())
                    {
                        toBeExploded.Add(enemy);
                    }

                    if (enemy.KnockbackTimer.ElapsedMilliseconds == 0 || enemy.KnockbackTimer.ElapsedMilliseconds >= knockbackCooldown)
                    {
                        if (enemy.IsOnGround && !enemy.IsJumping)
                        {
                            enemy.JumpTimer.Stop();
                            enemy.KnockbackTimer.Restart();

                            int knockbackDirectionX = player.X < enemy.X ? 1 : -1;

                            int proposedX = enemy.X + knockbackDistance * knockbackDirectionX;
                            int proposedY = enemy.Y - (int)enemy.VerticalSpeed;

                            bool collisionDetected = CheckCollisionWithPlatforms(proposedX, proposedY, enemy);
                            if (!collisionDetected)
                            {
                                enemy.JumpTimer.Restart();
                                enemy.X = proposedX;
                                enemy.Y = proposedY;
                            }

                            enemy.VerticalSpeed = -enemy.JumpForce(clientSize) / 4;
                            enemy.IsOnGround = false;
                            enemy.IsJumping = true;
                        }
                    }
                }
            }
        }
        public static void ChargingEnhancedAttack()
        {
            if (player.IsChargingAttack)
            {
                return;
            }

            if (!player.IsChargingAttack)
            {
                prevJumpForcePercent = player.JumpForcePercent;
                prevSpeedPrecent = player.SpeedPrecent;
            }

            chargedAttackStartTime = DateTime.Now;

            player.IsChargingAttack = true;

            player.lastAttackTime = DateTime.Now;
        }
        public static void EnhancedAttack()
        {
            RectangleF swordRect = new RectangleF(sword.X - sword.Width / 3, sword.Y - sword.Height / 2, sword.Width * 3f, sword.Height * 2);
            foreach (var enemy in enemies)
            {
                Rectangle enemyRect = enemy.GetRectangle(clientSize);

                if (swordRect.IntersectsWith(enemyRect))
                {
                    HitDetectedSound.Play();
                    enemy.TakeDamage(2000);

                    if (enemy.IsDead())
                    {
                        toBeExploded.Add(enemy);
                    }

                    if (enemy.KnockbackTimer.ElapsedMilliseconds == 0)
                    {
                        enemy.KnockbackTimer.Start();
                    }

                    int knockbackDistance = (int)(clientWidth * 0.03);
                    int knockbackDirection = player.X < enemy.X ? 1 : -1;
                    int proposedX = enemy.X + knockbackDistance * knockbackDirection;

                    bool collisionDetected = false;

                    foreach (var platformRect in platforms)
                    {
                        RectangleF rect = platformRect.ToRectangle(clientSize);
                        if (CheckCollision(proposedX, enemy.Y, enemy.Width, enemy.Height, rect.X, rect.Y, rect.Width, rect.Height))
                        {
                            collisionDetected = true;
                            break;
                        }
                    }

                    if (!collisionDetected)
                    {
                        enemy.X = proposedX;
                    }

                    enemy.VerticalSpeed = -enemy.JumpForce(clientSize) / 8;
                    enemy.IsOnGround = false;
                    enemy.IsJumping = true;
                }
            }
        }
    }
}
