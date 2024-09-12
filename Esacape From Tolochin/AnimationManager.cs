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

        public class AnimationFrame
        {
            public Bitmap Frame { get; }
            public RectangleF DisplayRectangle { get; }
            public PointF Anchor { get; }

            public AnimationFrame(Bitmap frame, RectangleF displayRectangle, PointF anchor)
            {
                Frame = frame;
                DisplayRectangle = displayRectangle;
                Anchor = anchor;
            }
        }
        public static AnimationFrame CreateFrameWithAnchor(Bitmap frame, float originX, float originY, PointF anchor)
        {
            float offsetX = originX - (frame.Width * anchor.X);
            float offsetY = originY - (frame.Height * anchor.Y);

            RectangleF displayRect = new RectangleF(offsetX, offsetY, frame.Width, frame.Height);

            return new AnimationFrame(frame, displayRect, anchor);
        }

        // Инверсия анимации
        public static Animation CreateInvertedAnimation(Animation originalAnimation, int xOffset)
        {
            List<AnimationFrame> invertedFrames = new List<AnimationFrame>();

            foreach (var frame in originalAnimation.Frames)
            {
                Bitmap invertedBitmap = new Bitmap(frame.Frame);
                invertedBitmap.RotateFlip(RotateFlipType.RotateNoneFlipX);

                RectangleF newDisplayRectangle = frame.DisplayRectangle;
                newDisplayRectangle.X += xOffset;

                AnimationFrame invertedFrame = new AnimationFrame(invertedBitmap, newDisplayRectangle, frame.Anchor);
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
