using System;
using System.Drawing;
using static SoloLeveling.MainForm;
using static SoloLeveling.AnimationManagaer;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SoloLeveling
{
    public class ResizeAdaptatinon
    {
        public static void AdaptAnimationFrames(List<AnimationFrame> frames, float xRatio, float yRatio)
        {
            for (int i = 0; i < frames.Count; i++)
            {
                var frame = frames[i];
                var displayRect = frame.DisplayRectangle;

                float adaptedX = displayRect.X * xRatio;
                float adaptedY = displayRect.Y * yRatio;
                float adaptedWidth = displayRect.Width * xRatio;
                float adaptedHeight = displayRect.Height * yRatio;

                frames[i] = new AnimationFrame(frame.Frame, new RectangleF(adaptedX, adaptedY, adaptedWidth, adaptedHeight));
            }
        }
        public static void AdaptDynamicRectangles(List<DynamicRectangle> rectangles, float xRatio, float yRatio)
        {
            foreach (var rect in rectangles)
            {
                rect.X = rect.X * xRatio;
                rect.Y = rect.Y * yRatio;
                rect.Width = rect.Width * xRatio;
                rect.Height = rect.Height * yRatio;
            }
        }
        public static void AdaptFontSize(Control control, float xRatio)
        {
            float newSize = control.Font.Size * xRatio;
            control.Font = new Font(control.Font.FontFamily, newSize, control.Font.Style);
        }
        public static void AdaptPeekUps(List<Pickup> rectangles, float xRatio, float yRatio)
        {
            foreach (var rect in rectangles)
            {
                rect.X = rect.X * xRatio;
                rect.Y = rect.Y * yRatio;
                rect.Width = rect.Width * xRatio;
                rect.Height = rect.Height * yRatio;
            }
        }
        public static void AdaptFallingPickUps(List<FallingPickup> rectangles, float xRatio, float yRatio)
        {
            foreach (var rect in rectangles)
            {
                rect.X = rect.X * xRatio;
                rect.Y = rect.Y * yRatio;
                rect.Width = rect.Width * xRatio;
                rect.Height = rect.Height * yRatio;
            }
        }
    }
}