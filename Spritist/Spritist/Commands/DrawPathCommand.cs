using System.Collections.Generic;
using System.Linq;
using Android.Graphics;
using Spritist.Utilities;

namespace Spritist.Commands
{
    public class DrawPathCommand : ICommand
    {

        private class ColoredPoint
        {
            public int X
            {
                get => Point.X;
                set => Point.X = value;
            }
            public int Y
            {
                get => Point.Y;
                set => Point.Y = value;
            }

            public Point Point;
            public Color Color;

            public ColoredPoint(Point point, Color color)
            {
                Point = point;
                Color = color;
            }
        }

        private readonly Bitmap bitmap;
        private readonly Color paintColor;
        private readonly List<Point> points;
        private readonly List<ColoredPoint> overwrittenPoints;
        //private bool addedPixels;


        public DrawPathCommand(Bitmap bitmap, Color color)
        {
            this.bitmap = bitmap;
            this.paintColor = color;

            points = new List<Point>();
            overwrittenPoints = new List<ColoredPoint>(bitmap.Width);
        }

        /// <summary>
        /// Draws a pixel onto the bitmap, storing the previous pixel value of the bitmap position.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="color">The color.</param>
        public void AddPixel(int x, int y)
        {
            if (!MathUtils.InRange(x, 0, bitmap.Width - 1) ||
                !MathUtils.InRange(y, 0, bitmap.Height - 1))
                return;

            //addedPixels = true;

            Point point = new Point(x, y);
            //Could be avoided with a 2D array of Coloredpoints, skip checking everything in the list
            if (points.Where(p => (p.X == x && p.Y == y)).FirstOrDefault() == null)
            {
                points.Add(point);
                Color oldColor = new Color(bitmap.GetPixel(x, y));

                overwrittenPoints.Add(new ColoredPoint(point, oldColor));
                bitmap.SetPixel(x, y, paintColor);
            }
        }

        public void Redo()
        {
            //if (addedPixels)
            //{
            //    addedPixels = false;
            //}
            //else
            //{
                foreach (var point in points)
                {
                    bitmap.SetPixel(point.X, point.Y, paintColor);
                }
            //}
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