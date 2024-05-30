using System;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Collections.Generic;
using static SoloLeveling.Player;
using static SoloLeveling.MainForm;
using static SoloLeveling.AnimationManagaer;

namespace SoloLeveling
{
    public class DrawAnimation
    {
        public static void SetupInGameAnimation()
        {
            playerAFKAnimation = new Animation(new List<AnimationFrame>
            {
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "Idle1.png")), new RectangleF(8, 2, 60, 80)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "Idle2.png")), new RectangleF(8, 2, 60, 80)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "Idle3.png")), new RectangleF(8, 2, 60, 80)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "Idle4.png")), new RectangleF(8, 2, 60, 80)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "Idle5.png")), new RectangleF(8, 2, 60, 80)),
            }, 200);
            playerMovingRightAnimation = new Animation(new List<AnimationFrame>
            {
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "moving-rigth_frames\\frame1.png")), new RectangleF(0, 2, 65, 80)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "moving-rigth_frames\\frame2.png")), new RectangleF(0, 2, 65, 80)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "moving-rigth_frames\\frame3.png")), new RectangleF(0, 2, 65, 80)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "moving-rigth_frames\\frame4.png")), new RectangleF(0, 2, 65, 80)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "moving-rigth_frames\\frame5.png")), new RectangleF(0, 2, 65, 80)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "moving-rigth_frames\\frame6.png")), new RectangleF(0, 2, 65, 80)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "moving-rigth_frames\\frame7.png")), new RectangleF(0, 2, 65, 80)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "moving-rigth_frames\\frame8.png")), new RectangleF(0, 2, 65, 80)),
            }, 90);
            playerMovingLeftAnimation = new Animation(new List<AnimationFrame>
            {
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "moving-left_frames\\frame1.png")), new Rectangle(0, 2, 65, 80)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "moving-left_frames\\frame2.png")), new Rectangle(0, 2, 65, 80)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "moving-left_frames\\frame3.png")), new Rectangle(0, 2, 65, 80)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "moving-left_frames\\frame4.png")), new Rectangle(0, 2, 65, 80)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "moving-left_frames\\frame5.png")), new Rectangle(0, 2, 65, 80)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "moving-left_frames\\frame6.png")), new Rectangle(0, 2, 65, 80)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "moving-left_frames\\frame7.png")), new Rectangle(0, 2, 65, 80)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "moving-left_frames\\frame8.png")), new Rectangle(0, 2, 65, 80)),
            }, 90);
            playerDeathAnimation = new Animation(new List<AnimationFrame>
            {
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "Death_Anim\\frame1.png")), new RectangleF(0, 2, 60, 80)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "Death_Anim\\frame2.png")), new RectangleF(0, 2, 60, 85)),
                //new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "Death_Anim\\frame3.png")), new Rectangle(0, -4, 70, 85)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "Death_Anim\\frame4.png")), new RectangleF(0, 20, 60, 60)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "Death_Anim\\frame5.png")), new RectangleF(0, 20, 60, 60)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "Death_Anim\\frame6.png")), new RectangleF(0, 21, 60, 60)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "Death_Anim\\frame7.png")), new RectangleF(0, 22, 60, 60)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "Death_Anim\\frame8.png")), new RectangleF(0, 22, 60, 60)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "Death_Anim\\frame9.png")), new RectangleF(0, 23, 70, 60)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "Death_Anim\\frame10.png")), new RectangleF(0, 24, 75, 60)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "Death_Anim\\frame11.png")), new RectangleF(0, 24, 75, 60)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "Death_Anim\\frame12.png")), new RectangleF(0, 24, 75, 60)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "Death_Anim\\frame13.png")), new RectangleF(0, 24, 90, 60)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "Death_Anim\\frame14.png")), new RectangleF(0, 24, 90, 60)),
            }, 275);
            playerChargedLeftAttackAnimation = new Animation(new List<AnimationFrame>
            {
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "ChargedAttak_Left\\Frame1.png")), new RectangleF(-40, 2, 100, 80)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "ChargedAttak_Left\\Frame2.png")), new RectangleF(0, -8, 80, 90)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "ChargedAttak_Left\\Frame3.png")), new RectangleF(-20, -11, 100, 115)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "ChargedAttak_Left\\Frame4.png")), new RectangleF(-20, -13, 100, 115)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "ChargedAttak_Left\\Frame5.png")), new RectangleF(-15, -48, 100, 145)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "ChargedAttak_Left\\Frame6.png")), new RectangleF(-15, -53, 100, 145)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "ChargedAttak_Left\\Frame7.png")), new RectangleF(-15, -60, 100, 155)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "ChargedAttak_Left\\Frame8.png")), new RectangleF(-15, -64, 100, 155)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "ChargedAttak_Left\\Frame9.png")), new RectangleF(-15, -99, 110, 183)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "ChargedAttak_Left\\Frame10.png")), new RectangleF(-35, -78, 95, 160)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "ChargedAttak_Left\\Frame11.png")), new RectangleF(-10, -60, 200, 145)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "ChargedAttak_Left\\Frame12.png")), new RectangleF(-10, -52, 200, 145)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "ChargedAttak_Left\\Frame13.png")), new RectangleF(-10, 3, 200, 87)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "ChargedAttak_Left\\Frame14.png")), new RectangleF(-215, -7, 450, 90)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "ChargedAttak_Left\\Frame15.png")), new RectangleF(-160, -128, 300, 210)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "ChargedAttak_Left\\Frame16.png")), new RectangleF(-160, -138, 330, 220)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "ChargedAttak_Left\\Frame17.png")), new RectangleF(-175, -118, 370, 200)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "ChargedAttak_Left\\Frame18.png")), new RectangleF(-120, -98, 310, 180)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "ChargedAttak_Left\\Frame19.png")), new RectangleF(-2, -98, 210, 180)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "ChargedAttak_Left\\Frame20.png")), new RectangleF(0, -53, 170, 135)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "ChargedAttak_Left\\Frame21.png")), new RectangleF(0, -43, 145, 125)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "ChargedAttak_Left\\Frame22.png")), new RectangleF(0, -3, 100, 85)),
            }, 120);
            playerChargedAttackAnimation = new Animation(new List<AnimationFrame>
            {
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "Charged_AttackAnimation\\Frame1.png")), new RectangleF(0, 2, 100, 80)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "Charged_AttackAnimation\\Frame2.png")), new RectangleF(0, -8, 80, 90)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "Charged_AttackAnimation\\Frame3.png")), new RectangleF(0, -11, 100, 115)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "Charged_AttackAnimation\\Frame4.png")), new RectangleF(-5, -13, 100, 115)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "Charged_AttackAnimation\\Frame5.png")), new RectangleF(-5, -48, 100, 145)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "Charged_AttackAnimation\\Frame6.png")), new RectangleF(-5, -53, 100, 145)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "Charged_AttackAnimation\\Frame7.png")), new RectangleF(-5, -60, 100, 155)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "Charged_AttackAnimation\\Frame8.png")), new RectangleF(-5, -64, 100, 155)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "Charged_AttackAnimation\\Frame9.png")), new RectangleF(-5, -99, 110, 183)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "Charged_AttackAnimation\\Frame10.png")), new RectangleF(-35, -78, 95, 160)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "Charged_AttackAnimation\\Frame11.png")), new RectangleF(-135, -60, 200, 145)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "Charged_AttackAnimation\\Frame12.png")), new RectangleF(-135, -52, 200, 145)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "Charged_AttackAnimation\\Frame13.png")), new RectangleF(-135, 3, 200, 87)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "Charged_AttackAnimation\\Frame14.png")), new RectangleF(-155, -7, 450, 90)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "Charged_AttackAnimation\\Frame15.png")), new RectangleF(-70, -128, 300, 210)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "Charged_AttackAnimation\\Frame16.png")), new RectangleF(-100, -138, 330, 220)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "Charged_AttackAnimation\\Frame17.png")), new RectangleF(-115, -118, 370, 200)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "Charged_AttackAnimation\\Frame18.png")), new RectangleF(-110, -98, 310, 180)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "Charged_AttackAnimation\\Frame19.png")), new RectangleF(-135, -98, 210, 180)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "Charged_AttackAnimation\\Frame20.png")), new RectangleF(-98, -53, 170, 135)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "Charged_AttackAnimation\\Frame21.png")), new RectangleF(-75, -33, 145, 115)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "Charged_AttackAnimation\\Frame22.png")), new RectangleF(-30, -3, 100, 85)),
            }, 120);
            playerAttackAnimation = new Animation(new List<AnimationFrame>
            {
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "AttackAnimLeft\\frame1.png")), new RectangleF(0, 2, 95, 80)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "AttackAnimLeft\\frame3.png")), new RectangleF(-30, 2, 95, 80))
            }, 100);
            enemyMovingAnimation = new Animation(new List<AnimationFrame>
            {
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "enemy_moving\\frame1.png")), new RectangleF(0, 2, 35, 40)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "enemy_moving\\frame2.png")), new RectangleF(0, 2, 35, 40)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "enemy_moving\\frame3.png")), new RectangleF(0, 2, 35, 40)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "enemy_moving\\frame4.png")), new RectangleF(0, 2, 35, 40)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "enemy_moving\\frame5.png")), new RectangleF(0, 2, 35, 40)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "enemy_moving\\frame6.png")), new RectangleF(0, 2, 35, 40)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "enemy_moving\\frame7.png")), new RectangleF(0, 2, 35, 40)),
                new AnimationFrame(new Bitmap(Path.Combine(resourcesPath, "enemy_moving\\frame8.png")), new RectangleF(0, 2, 35, 40)),
            }, 275);
        }
        public static void DrawPlayerAnimation(Graphics g)
        {
            Animation currentAnimation;

            if (player.IsDead())
            {
                currentAnimation = playerDeathAnimation;
                player.CurrentAnimation = playerDeathAnimation;
                playerAnimationTimer.Interval = playerDeathAnimation.Interval;
            }
            else if (player.IsChargingAttack && !player.IsDead())
            {
                int moveDirection = player.CurrentDirection == Direction.Right ? 1 : -1;
                currentAnimation = player.CurrentDirection == Direction.Right ? playerChargedAttackAnimation : playerChargedLeftAttackAnimation;
                TimeSpan elapsed = DateTime.Now - SwordAttack.chargedAttackStartTime;

                if (elapsed <= playerChargedAttackAnimation.TotalDuration)
                {
                    player.JumpForcePercent = 0;
                    player.SpeedPrecent = 0;

                    int framesCount = currentAnimation.Frames.Count;
                    player.currentSpriteIndex = (int)(framesCount * elapsed.TotalMilliseconds / playerChargedAttackAnimation.TotalDuration.TotalMilliseconds) % framesCount;

                    if (player.currentSpriteIndex == 2)
                    {
                        TryMovePlayer((int)(clientWidth * 0.0025) * moveDirection);
                    }
                    if (player.currentSpriteIndex == 12)
                    {
                        SwordAttack.EnhancedAttack();
                    }
                    if (player.currentSpriteIndex == 9)
                    {
                        TryMovePlayer((int)(clientWidth * 0.017) * moveDirection);
                    }
                }
                else
                {
                    player.IsChargingAttack = false;

                    player.JumpForcePercent = prevJumpForcePercent;
                    player.SpeedPrecent = prevSpeedPrecent;
                    player.currentSpriteIndex = 0;
                }

                player.CurrentAnimation = playerChargedAttackAnimation;
                playerAnimationTimer.Interval = playerChargedAttackAnimation.Interval;
            }
            else if (!player.IsChargingAttack && player.IsAttack && !player.IsDead())
            {
                currentAnimation = player.CurrentDirection == Player.Direction.Right ? AnimationManagaer.CreateInvertedAnimation(playerAttackAnimation) : playerAttackAnimation;
                TimeSpan elapsed = DateTime.Now - SwordAttack.AttackStartTime;
                int directionX = player.CurrentDirection == Player.Direction.Right ? 1 : -1;

                if (elapsed <= currentAnimation.TotalDuration)
                {
                    player.JumpForcePercent = 0;
                    player.SpeedPrecent = 0;

                    int framesCount = currentAnimation.Frames.Count;
                    player.currentSpriteIndex = (int)(framesCount * elapsed.TotalMilliseconds / currentAnimation.TotalDuration.TotalMilliseconds) % framesCount;
                    if (player.currentSpriteIndex == 1)
                    {
                        TryMovePlayer((int)(clientWidth * 0.005) * directionX);
                        sword.X = directionX < 0 ? player.X - player.Width : player.X + player.Width / 4;
                    }
                }
                else
                {
                    SwordAttack.Attack();
                    player.IsAttack = false;
                    player.JumpForcePercent = prevJumpForcePercent;
                    player.SpeedPrecent = prevSpeedPrecent;
                    player.currentSpriteIndex = 0;
                }

                player.CurrentAnimation = currentAnimation;
                playerAnimationTimer.Interval = currentAnimation.Interval;
            }

            else if (player.IsAFK())
            {
                currentAnimation = player.CurrentDirection == Player.Direction.Left ? AnimationManagaer.CreateInvertedAnimation(playerAFKAnimation) : playerAFKAnimation;
                player.CurrentAnimation = playerAFKAnimation;
                playerAnimationTimer.Interval = playerAFKAnimation.Interval;
            }
            else
            {
                if (player.CurrentDirection == Player.Direction.Left)
                {
                    currentAnimation = playerMovingLeftAnimation;
                    player.CurrentAnimation = playerMovingLeftAnimation;
                    playerAnimationTimer.Interval = playerMovingLeftAnimation.Interval;
                }
                else
                {
                    currentAnimation = playerMovingRightAnimation;
                    player.CurrentAnimation = playerMovingRightAnimation;
                    playerAnimationTimer.Interval = playerMovingRightAnimation.Interval;
                }
            }

            if (player.currentSpriteIndex < 0 || player.currentSpriteIndex >= currentAnimation.Frames.Count)
            {
                player.currentSpriteIndex = 0;
            }

            g.DrawImage(
                currentAnimation.Frames[player.currentSpriteIndex].Frame,
                player.X + currentAnimation.Frames[player.currentSpriteIndex].DisplayRectangle.X,
                player.Y + currentAnimation.Frames[player.currentSpriteIndex].DisplayRectangle.Y,
                currentAnimation.Frames[player.currentSpriteIndex].DisplayRectangle.Width,
                currentAnimation.Frames[player.currentSpriteIndex].DisplayRectangle.Height
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
                    currentAnimation = AnimationManagaer.CreateInvertedAnimation(enemyMovingAnimation);
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

            levelFont = new Font(privateFontCollection.Families.First(f => f.Name == "Planes_ValMore"), (int)(clientWidth * 0.018), FontStyle.Bold);

            SizeF levelTextSize = g.MeasureString(levelText, levelFont);
            SizeF experienceTextSize = g.MeasureString(experienceText, levelFont);

            float levelTextX = cameraX + clientWidth / 2 - levelTextSize.Width / 2;
            float levelTextY = clientWidth * 0.012f;
            PointF levelTextLocation = new PointF(levelTextX, levelTextY);

            float experienceTextX = cameraX + clientWidth / 2 - experienceTextSize.Width / 2;
            float experienceTextY = levelTextY + levelTextSize.Height;
            PointF experienceTextLocation = new PointF(experienceTextX, experienceTextY);

            g.DrawString(levelText, levelFont, Brushes.White, levelTextLocation);
            g.DrawString(experienceText, levelFont, Brushes.White, experienceTextLocation);
        }
        public static void DrawPlayerDeath(Graphics g)
        {
            string deathText = "ВЫ ПОГИБЛИ";
            DeathFont = new Font(privateFontCollection.Families.First(f => f.Name == "Planes_ValMore"), (int)(clientWidth * 0.05), FontStyle.Bold);

            SizeF TextSize = g.MeasureString(deathText, DeathFont);

            float TextX = cameraX + clientWidth / 2 - TextSize.Width / 2;
            float TextY = clientHeight * 0.35f;

            PointF DeathTextLocation = new PointF(TextX, TextY);

            Color overlayColor = Color.FromArgb(overlayAlpha, 0, 0, 0);
            Brush overlayBrush = new SolidBrush(overlayColor);

            g.FillRectangle(overlayBrush, cameraX, 0, clientWidth, clientHeight);

            g.DrawString(deathText, DeathFont, Brushes.Red, DeathTextLocation);
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
        public static void DrawPauseMenu(Graphics g)
        {
            Color overlayColor = Color.FromArgb(160, 0, 0, 0);
            Brush overlayBrush = new SolidBrush(overlayColor);
            g.FillRectangle(overlayBrush, cameraX, 0, clientWidth, clientHeight);
        }
    }
}