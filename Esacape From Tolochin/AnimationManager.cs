using System;
using System.Drawing;
using System.Collections.Generic;
using static SoloLeveling.MainForm;
using System.IO;
using System.Linq;

namespace SoloLeveling
{
    public class AnimationManagaer
    {
        // Состояния игрока
        public static Animation playerAFKAnimation;
        public static Animation playerMovingRightAnimation;
        public static Animation playerMovingLeftAnimation;
        public static Animation playerChargedAttackAnimation;
        public static Animation playerDeathAnimation;
        public static Animation playerAttackAnimation;

        // Состояния противника
        public static Animation enemyMovingAnimation;
        // Фрейм анимации
        public class AnimationFrame
        {
            public Bitmap Frame { get; }
            public RectangleF DisplayRectangle { get; }
            public PointF Anchor { get; } // Добавлен якорь

            public AnimationFrame(Bitmap frame, RectangleF displayRectangle, PointF anchor)
            {
                Frame = frame;
                DisplayRectangle = displayRectangle;
                Anchor = anchor;
            }
        }
        public static AnimationFrame CreateFrameWithAnchor(Bitmap frame, float originX, float originY, PointF anchor)
        {
            // Вычисляем смещение по X и Y относительно якоря
            float offsetX = originX - (frame.Width * anchor.X);
            float offsetY = originY - (frame.Height * anchor.Y);

            // Создаём новый прямоугольник отображения с учётом смещения
            RectangleF displayRect = new RectangleF(offsetX, offsetY, frame.Width, frame.Height);

            return new AnimationFrame(frame, displayRect, anchor);
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

                // Создаем новый прямоугольник с инвертированными координатами X
                RectangleF invertedRect = new RectangleF(
                    frame.DisplayRectangle.X * -1,
                    frame.DisplayRectangle.Y,
                    frame.DisplayRectangle.Width,
                    frame.DisplayRectangle.Height
                );

                // Используем оригинальный anchor, или создаем новый, если нужно
                AnimationFrame invertedFrame = new AnimationFrame(invertedBitmap, invertedRect, frame.Anchor);
                invertedFrames.Add(invertedFrame);
            }

            Animation invertedAnimation = new Animation(invertedFrames, originalAnimation.Interval);

            return invertedAnimation;
        }


        public static Animation LoadAnimation(string folderPath, string[] frameFiles, RectangleF defaultRectangle, PointF anchor, int frameInterval)
        {
            var frames = frameFiles.Select(fileName =>
            {
                string path = Path.Combine(folderPath, fileName);
                var bitmap = new Bitmap(path);
                return new AnimationFrame(bitmap, defaultRectangle, anchor);
            }).ToList();

            return new Animation(frames, frameInterval);
        }
    }
}
