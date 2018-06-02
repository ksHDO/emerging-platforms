using System.Collections.Generic;
using System.Linq;
using Android.Graphics;
using Spritist.Utilities;

namespace Spritist.Commands
{
    public class DrawPathCommand : ICommand
    {

        private readonly Bitmap bitmap;
        private readonly Color paintColor;
        private readonly List<Point> points;
        private readonly List<ColoredPoint> overwrittenPoints;
        //private bool addedPixels;
        private readonly int diameterSize;

        public DrawPathCommand(Bitmap bitmap, Color color, int diameterSize)
        {
            this.bitmap = bitmap;
            this.paintColor = color;
            this.diameterSize = diameterSize;

            points = new List<Point>();
            overwrittenPoints = new List<ColoredPoint>(bitmap.Width);

        }

        private void DrawPixel(int x, int y)
        {
            if (!MathUtils.InRange(x, 0, bitmap.Width - 1) ||
                !MathUtils.InRange(y, 0, bitmap.Height - 1))
                return;

            //addedPixels = true;

            Point point = new Point(x, y);
            //Could be avoided with a 2D array of Coloredpoints, skip checking everything in the list
            if (points.FirstOrDefault(p => (p.X == x && p.Y == y)) == null)
            {
                points.Add(point);
                Color oldColor = new Color(bitmap.GetPixel(x, y));

                Color colorToPaint = MathUtils.BlendColors(oldColor, paintColor);
                overwrittenPoints.Add(new ColoredPoint(point, oldColor));
                bitmap.SetPixel(x, y, colorToPaint);
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
                bitmap.SetPixel(point.X, point.Y, paintColor);
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