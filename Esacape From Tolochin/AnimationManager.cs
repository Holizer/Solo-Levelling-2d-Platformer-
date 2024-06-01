using System;
using System.Drawing;
using System.Collections.Generic;
using static SoloLeveling.MainForm;

namespace SoloLeveling
{
    public class AnimationManagaer
    {
        // Состояния игрока
        public static Animation playerAFKAnimation;
        public static Animation playerMovingRightAnimation;
        public static Animation playerMovingLeftAnimation;
        public static Animation playerChargedAttackAnimation;
        public static Animation playerChargedLeftAttackAnimation;
        public static Animation playerDeathAnimation;
        public static Animation playerAttackAnimation;

        // Состояния противника
        public static Animation enemyMovingAnimation;

        // Фрейм анимации
        public struct AnimationFrame
        {
            public Bitmap Frame { get; }
            public RectangleF DisplayRectangle { get; }

            public AnimationFrame(Bitmap frame, RectangleF displayRect)
            {
                Frame = frame;
                DisplayRectangle = displayRect;
            }
        }
        
        // Анимация фреймов
        public struct Animation
        {
            public List<AnimationFrame> Frames { get; }
            public int Interval { get; }

            public Animation(List<AnimationFrame> frames, int interval)
            {
                Frames = frames;
                Interval = interval;
            }
            public TimeSpan TotalDuration
            {
                get
                {
                    return TimeSpan.FromMilliseconds(Frames.Count * Interval);
                }
            }
        }
        
        // Инверсия анимации
        public static Animation CreateInvertedAnimation(Animation originalAnimation)
        {
            List<AnimationFrame> invertedFrames = new List<AnimationFrame>();

            foreach (var frame in originalAnimation.Frames)
            {
                Bitmap invertedBitmap = new Bitmap(frame.Frame);
                invertedBitmap.RotateFlip(RotateFlipType.RotateNoneFlipX);

                // Инвертируем координаты X правильно для отраженного изображения
                float invertedX = clientSize.Width - frame.DisplayRectangle.Right - frame.DisplayRectangle.Width;

                // Создаем новый прямоугольник с инвертированными координатами X
                RectangleF invertedRect = new RectangleF(
                    frame.DisplayRectangle.X * -1,
                    frame.DisplayRectangle.Y,
                    frame.DisplayRectangle.Width,
                    frame.DisplayRectangle.Height
                );

                AnimationFrame invertedFrame = new AnimationFrame(invertedBitmap, invertedRect);
                invertedFrames.Add(invertedFrame);
            }

            Animation invertedAnimation = new Animation(invertedFrames, originalAnimation.Interval);

            return invertedAnimation;
        }
    }
}
