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
        public int Damage { get; set; }

        public bool IsAttacking;
        public Bitmap Texture { get; set; }
        public Sword(int x, int y, int width, int height, int damage)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Damage = damage;
        }
        public Rectangle GetRectangle()
        {
            return new Rectangle(X,Y, Width, Height);
        }
    }
    public class SwordAttack
    {
        public static DateTime chargedAttackStartTime;

        public static DateTime AttackStartTime;

        public static float savedJumpForce;

        public static float savedSpeed;

        public const int knockbackDistance = 20;

        public const int knockbackCooldown = 2;

        public static void AttackWithSword()
        {
            if (player.IsAttack)
            {
                return;
            }

            savedJumpForce = player.JumpForce; 
            savedSpeed = player.Speed;

            AttackStartTime = DateTime.Now;

            player.IsAttack = true;

            player.lastAttackTime = DateTime.Now;
        }
        public static void Attack()
        {
            Rectangle swordRect = new Rectangle(sword.X, sword.Y, sword.Width, sword.Height);

            foreach (var enemy in enemies)
            {
                Rectangle enemyRect = enemy.GetRectangle();

                if (swordRect.IntersectsWith(enemyRect))
                {
                    SoundManager.PlayHitSound();
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

                            enemy.VerticalSpeed = 20;
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

            savedJumpForce = player.JumpForce;
            savedSpeed = player.Speed;

            chargedAttackStartTime = DateTime.Now;

            player.IsChargingAttack = true;

            player.lastAttackTime = DateTime.Now;
        }
        public static void EnhancedAttack()
        {
            RectangleF swordRect = new RectangleF(sword.X - sword.Width / 3, sword.Y - sword.Height / 2, sword.Width * 3f, sword.Height * 2);
            foreach (var enemy in enemies)
            {
                Rectangle enemyRect = enemy.GetRectangle();

                if (swordRect.IntersectsWith(enemyRect))
                {
                    SoundManager.PlayHitSound();
                    enemy.TakeDamage(2000);

                    if (enemy.IsDead())
                    {
                        toBeExploded.Add(enemy);
                    }

                    if (enemy.KnockbackTimer.ElapsedMilliseconds == 0)
                    {
                        enemy.KnockbackTimer.Start();
                    }

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

                    enemy.VerticalSpeed = 20;
                    enemy.IsOnGround = false;
                    enemy.IsJumping = true;
                }
            }
        }
    }
}
