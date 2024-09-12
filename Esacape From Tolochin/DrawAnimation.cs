using System;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Collections.Generic;
using static SoloLeveling.Player;
using static SoloLeveling.MainForm;
using static SoloLeveling.AnimationManagaer;
using System.Windows.Forms;
using SoloLeveling;

namespace SoloLeveling
{
    public class DrawAnimation
    {
        public static Animation playerAFKAnimation;
        public static Animation playerAFKAnimation_Left;

        public static Animation playerMovingRightAnimation;
        public static Animation playerMovingLeftAnimation;

        public static Animation playerChargedAttackAnimation;
        public static Animation playerChargedAttackAnimation_Left;

        public static Animation playerDeathAnimation;
        public static Animation playerAttackAnimation;
        public static Animation playerAttackAnimation_Left;


        public static Animation enemyMovingAnimation;

        public static void SetupInGameAnimation()
        {
            string afkAnimFolderPath = Path.Combine(resourcesPath, "AFKAnim");
            string[] afkFrames = { "Frame1.png", "Frame2.png", "Frame3.png", "Frame4.png", "Frame5.png" };
            playerAFKAnimation = LoadAnimation(afkAnimFolderPath, afkFrames, new RectangleF(0, 60, 90, 110), new PointF(0f, 0.5f), 200);

            string movingRightAinmFolderPath = Path.Combine(resourcesPath, "MovingRigthAnim");
            string[] movingRightFrames = { "frame1.png", "frame2.png", "frame3.png", "frame4.png", "frame5.png", "frame6.png", "frame7.png", "frame8.png" };
            playerMovingRightAnimation = LoadAnimation(movingRightAinmFolderPath, movingRightFrames, new RectangleF(0, 60, 95, 110), new PointF(0f, 0.5f), 90);

            string movingLeftAnimFolderPath = Path.Combine(resourcesPath, "MovingLeftAnim");
            string[] movingLeftFrames = { "frame1.png", "frame2.png", "frame3.png", "frame4.png", "frame5.png", "frame6.png", "frame7.png", "frame8.png" };
            playerMovingLeftAnimation = LoadAnimation(movingLeftAnimFolderPath, movingLeftFrames, new RectangleF(0, 60, 85, 110), new PointF(0f, 0.5f), 90);

            string attackAnimFolderPath = Path.Combine(resourcesPath, "AttackAnim");
            string[] attackAnimFrames = { "frame1.png", "frame2.png" };
            playerAttackAnimation = LoadAnimation(attackAnimFolderPath, attackAnimFrames, new RectangleF(-20, 38, 175, 153), new PointF(0f, 0.5f), 120);

            string chargedRightAttackAnimFolderPath = Path.Combine(resourcesPath, "ChargedAttackAnim");
            string[] chargedRightAttackAnimFrames = {
                "Frame1.png", "Frame2.png", "Frame3.png", "Frame4.png",
                "Frame5.png", "Frame6.png", "Frame7.png", "Frame8.png", "Frame9.png",
                "Frame10.png", "Frame11.png", "Frame12.png", "Frame13.png", "Frame14.png",
                "Frame15.png", "Frame16.png", "Frame17.png", "Frame18.png", "Frame19.png",
                "Frame20.png", "Frame21.png", "Frame22.png"
            };
            playerChargedAttackAnimation = LoadAnimation(
                chargedRightAttackAnimFolderPath,
                chargedRightAttackAnimFrames,
                //new RectangleF(30, -49, 652, 390),
                new RectangleF(-10, -40, 619, 370),
                new PointF(0.3f, 0.5f),
                100
            );

            string deathAnimFolderPath = Path.Combine(resourcesPath, "DeathAnim");
            string[] deathAnimFrames = {
                "Frame1.png", "Frame2.png", "Frame3.png", "Frame4.png",
                "Frame5.png", "Frame6.png", "Frame7.png", "Frame8.png", "Frame9.png",
                "Frame10.png", "Frame11.png", "Frame12.png", "Frame13.png"
            };
            playerDeathAnimation = LoadAnimation(
                deathAnimFolderPath,
                deathAnimFrames,
                new RectangleF(0, -49, 234, 260),
                new PointF(0.5f, 0.5f),
                150
            );

            playerChargedAttackAnimation_Left = AnimationManagaer.CreateInvertedAnimation(playerChargedAttackAnimation, -110);
            playerAFKAnimation_Left = AnimationManagaer.CreateInvertedAnimation(playerAFKAnimation, 0);
            playerAttackAnimation_Left = AnimationManagaer.CreateInvertedAnimation(playerAttackAnimation, -30);

        }

        //enemyMovingAnimation = new Animation(new List<AnimationFrame>
        //            {
        //                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "enemy_moving\\frame1.png")), new RectangleF(0, 2, 35, 40)),
        //                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "enemy_moving\\frame2.png")), new RectangleF(0, 2, 35, 40)),
        //                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "enemy_moving\\frame3.png")), new RectangleF(0, 2, 35, 40)),
        //                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "enemy_moving\\frame4.png")), new RectangleF(0, 2, 35, 40)),
        //                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "enemy_moving\\frame5.png")), new RectangleF(0, 2, 35, 40)),
        //                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "enemy_moving\\frame6.png")), new RectangleF(0, 2, 35, 40)),
        //                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "enemy_moving\\frame7.png")), new RectangleF(0, 2, 35, 40)),
        //                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "enemy_moving\\frame8.png")), new RectangleF(0, 2, 35, 40)),
        //            }, 275);
        //        }

        public static void DrawPlayerAnimation(Graphics g)
        {
            Animation currentAnimation = GetCurrentAnimation();
            player.CurrentAnimation = currentAnimation;
            playerAnimationTimer.Interval = currentAnimation.Interval;

            if (player.currentSpriteIndex < 0 || player.currentSpriteIndex >= currentAnimation.Frames.Count)
            {
                player.currentSpriteIndex = 0;
            }

            var frame = currentAnimation.Frames[player.currentSpriteIndex];
            DrawFrame(g, frame);
        }

        private static Animation GetCurrentAnimation()
        {
            if (player.IsDead())
            {
                return playerDeathAnimation;
            }

            if (player.IsChargingAttack)
            {
                HandleChargedAttack();
                return player.CurrentDirection == Direction.Right ? playerChargedAttackAnimation : playerChargedAttackAnimation_Left;
            }

            if (player.IsAttack)
            {
                HandleAttack();
                return player.CurrentDirection == Direction.Right ? playerAttackAnimation : playerAttackAnimation_Left;
            }

            if (player.IsAFK())
            {
                return player.CurrentDirection == Direction.Right ? playerAFKAnimation : playerAFKAnimation_Left; 
            }

            return player.CurrentDirection == Direction.Left ? playerMovingLeftAnimation : playerMovingRightAnimation;
        }

        private static void HandleChargedAttack()
        {
            TimeSpan elapsed = DateTime.Now - SwordAttack.chargedAttackStartTime;
            if (elapsed <= playerChargedAttackAnimation.TotalDuration)
            {
                SetPlayerMovementAndAttack(0, 0);
                SetSpriteIndexByElapsedTime(playerChargedAttackAnimation, elapsed);

                if (player.currentSpriteIndex == 2)
                {
                    TryMovePlayer((int)(clientSize.Width * 0.0025) * GetMoveDirection());
                }
                if (player.currentSpriteIndex == 9)
                {
                    TryMovePlayer((int)(clientSize.Width * 0.017) * GetMoveDirection());
                }
                if (player.currentSpriteIndex == 12)
                {
                    SwordAttack.EnhancedAttack();
                }
            }
            else
            {
                ResetPlayerAfterAttack();
                player.IsChargingAttack = false;
            }
        }

        private static void HandleAttack()
        {
            TimeSpan elapsed = DateTime.Now - SwordAttack.AttackStartTime;
            if (elapsed <= playerAttackAnimation.TotalDuration)
            {
                SetPlayerMovementAndAttack(0, 0);
                SetSpriteIndexByElapsedTime(playerAttackAnimation, elapsed);

                if (player.currentSpriteIndex == 1)
                {
                    TryMovePlayer((int)(clientSize.Width * 0.005) * GetMoveDirection());
                    sword.X = player.CurrentDirection == Direction.Left ? player.X - player.Width : player.X + player.Width / 4;
                }
            }
            else
            {
                SwordAttack.Attack();
                ResetPlayerAfterAttack();
                player.IsAttack = false;
            }
        }

        private static void SetSpriteIndexByElapsedTime(Animation animation, TimeSpan elapsed)
        {
            int framesCount = animation.Frames.Count;
            player.currentSpriteIndex = (int)(framesCount * elapsed.TotalMilliseconds / animation.TotalDuration.TotalMilliseconds) % framesCount;
        }

        private static void SetPlayerMovementAndAttack(float jumpForcePercent, float speedPercent)
        {
            player.JumpForcePercent = jumpForcePercent;
            player.SpeedPrecent = speedPercent;
        }

        private static void ResetPlayerAfterAttack()
        {
            player.JumpForcePercent = prevJumpForcePercent;
            player.SpeedPrecent = prevSpeedPrecent;
            player.currentSpriteIndex = 0;
        }

        private static int GetMoveDirection()
        {
            return player.CurrentDirection == Direction.Right ? 1 : -1;
        }

        private static void DrawFrame(Graphics g, AnimationFrame frame)
        {
            // Применение якоря для позиционирования изображения
            float drawX = player.X + frame.DisplayRectangle.X - frame.DisplayRectangle.Width * frame.Anchor.X;
            float drawY = player.Y + frame.DisplayRectangle.Y - frame.DisplayRectangle.Height * frame.Anchor.Y;

            g.DrawImage(
                frame.Frame,
                drawX,
                drawY,
                frame.DisplayRectangle.Width,
                frame.DisplayRectangle.Height
            );
        }

        public static void DrawEnemyAnimation(Graphics g)
        {
            Animation currentAnimation = enemyMovingAnimation;

            foreach (var enemy in enemies)
            {
                if (enemy.CurrentDirection == Enemy.EnemyDirection.Right)
                {
                    currentAnimation = enemyMovingAnimation;
                    enemy.CurrentAnimation = currentAnimation;
                    enemyAnimationTimer.Interval = enemyMovingAnimation.Interval;
                }
                else if (enemy.CurrentDirection == Enemy.EnemyDirection.Left)
                {
                    currentAnimation = AnimationManagaer.CreateInvertedAnimation(enemyMovingAnimation, 0);
                    enemy.CurrentAnimation = currentAnimation;
                    enemyAnimationTimer.Interval = enemyMovingAnimation.Interval;
                }
                g.DrawImage(
                    currentAnimation.Frames[enemy.currentSpriteIndex].Frame,
                    enemy.X + currentAnimation.Frames[enemy.currentSpriteIndex].DisplayRectangle.X,
                    enemy.Y + currentAnimation.Frames[enemy.currentSpriteIndex].DisplayRectangle.Y,
                    currentAnimation.Frames[enemy.currentSpriteIndex].DisplayRectangle.Width,
                    currentAnimation.Frames[enemy.currentSpriteIndex].DisplayRectangle.Height
                );
            }
        }
        public static void DrawLevelUp(Graphics g)
        {

            string levelText = $"Level: {player.Level}";
            string experienceText = $"Experience: {player.Experience}/{player.ExperienceToNextLevel}";

            levelFont = new Font(privateFontCollection.Families.First(f => f.Name == "Planes_ValMore"), (int)(clientSize.Width * 0.018), FontStyle.Bold);

            SizeF levelTextSize = g.MeasureString(levelText, levelFont);
            SizeF experienceTextSize = g.MeasureString(experienceText, levelFont);

            float levelTextX = cameraX + clientSize.Width / 2 - levelTextSize.Width / 2;
            float levelTextY = clientSize.Width * 0.012f;
            PointF levelTextLocation = new PointF(levelTextX, levelTextY);

            float experienceTextX = cameraX + clientSize.Width / 2 - experienceTextSize.Width / 2;
            float experienceTextY = levelTextY + levelTextSize.Height;
            PointF experienceTextLocation = new PointF(experienceTextX, experienceTextY);

            g.DrawString(levelText, levelFont, Brushes.White, levelTextLocation);
            g.DrawString(experienceText, levelFont, Brushes.White, experienceTextLocation);
        }
        private static void TryMovePlayer(int deltaX)
        {
            int proposedX = player.X + deltaX;
            bool willCollide = WillCollideWithObstacles(proposedX, player.Y, player.Width, player.Height);

            if (!willCollide)
            {
                player.X = proposedX;
            }
        }
        public static void DrawDeath(Graphics g) {
            if (isGameOver && YouAreDeadTimer.ElapsedMilliseconds > 2500 && player.IsDead() && !GameIsEnd)
            {
                DrawPlayerDeath(g);
            }
            else if (isGameOver && YouAreDeadTimer.ElapsedMilliseconds > 1500 && player.IsDead() && GameIsEnd)
            {
                Color overlayColor = Color.FromArgb(overlayAlpha, 0, 0, 0);
                Brush overlayBrush = new SolidBrush(overlayColor);

                g.FillRectangle(overlayBrush, cameraX, 0, clientSize.Width, clientSize.Height);
            }
        }
        private static void DrawPlayerDeath(Graphics g)
        {
            string deathText = "ВЫ ПОГИБЛИ";
            DeathFont = new Font(privateFontCollection.Families.First(f => f.Name == "Planes_ValMore"), (int)(clientSize.Width * 0.05), FontStyle.Bold);

            SizeF TextSize = g.MeasureString(deathText, DeathFont);

            float TextX = cameraX + clientSize.Width / 2 - TextSize.Width / 2;
            float TextY = clientSize.Height * 0.35f;

            PointF DeathTextLocation = new PointF(TextX, TextY);

            Color overlayColor = Color.FromArgb(overlayAlpha, 0, 0, 0);
            Brush overlayBrush = new SolidBrush(overlayColor);

            g.FillRectangle(overlayBrush, cameraX, 0, clientSize.Width, clientSize.Height);

            g.DrawString(deathText, DeathFont, Brushes.Red, DeathTextLocation);
        }
        public static void DrawPauseMenu_BG(Graphics g)
        {
            Color overlayColor = Color.FromArgb(160, 0, 0, 0);
            Brush overlayBrush = new SolidBrush(overlayColor);
            g.FillRectangle(overlayBrush, cameraX, 0, clientSize.Width, clientSize.Height);
        }
    }
}