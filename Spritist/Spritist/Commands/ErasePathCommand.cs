using System;
using System.Collections.Generic;
using System.Linq;
using Android.Graphics;
using Spritist.Utilities;

namespace Spritist.Commands
{
    public class ErasePathCommand : ICommand
    {
        private readonly Bitmap bitmap;
        private readonly byte alpha;
        private readonly List<ColoredPoint> points;
        private readonly List<ColoredPoint> overwrittenPoints;
        private readonly int diameterSize;

        public ErasePathCommand(Bitmap bitmap, byte alpha, int diameterSize)
        {
            this.bitmap = bitmap;
            this.alpha = alpha;
            this.diameterSize = diameterSize;

            points = new List<ColoredPoint>();
            overwrittenPoints = new List<ColoredPoint>(bitmap.Width);
        }

        private void DrawPixel(int x, int y)
        {
            if (!MathUtils.InRange(x, 0, bitmap.Width - 1) ||
                !MathUtils.InRange(y, 0, bitmap.Height - 1))
                return;

            //addedPixels = true;

            Point point = new Point(x, y);
            if (points.FirstOrDefault(p => (p.X == x && p.Y == y)) == null)
            {
                Color oldColor = new Color(bitmap.GetPixel(x, y));

                overwrittenPoints.Add(new ColoredPoint(point, oldColor));

                Color newColor = new Color(oldColor);
                newColor.A = (byte) Math.Max(0, oldColor.A - alpha); 
                points.Add(new ColoredPoint(point, newColor));

                bitmap.SetPixel(x, y, newColor);
            }
        }

        /// <summary>
        /// Draws a pixel onto the bitmap, storing the previous pixel value of the bitmap position.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="color">The color.</param>
        public void DrawOnPoint(int x, int y)
        {
            // Figure out what pixels will be affected

            int radius = diameterSize / 2;
            // Draw a square
            for (int i = 0; i < diameterSize; i++)
            {
                int drawPosX = i - radius;
                for (int j = 0; j < diameterSize; j++)
                {
                    int drawPosY = j - radius;
                    DrawPixel(x + drawPosX, y + drawPosY);
                }
            }
        }

        public void Redo()
        {
            foreach (var point in points)
            {
                bitmap.SetPixel(point.X, point.Y, point.Color);
            }
        }

        public void Undo()
        {
            for (var i = overwrittenPoints.Count - 1; i >= 0; i--)
            {
                var point = overwrittenPoints[i];
                bitmap.SetPixel(point.X, point.Y, point.Color);
            }
        }
    }
}