using System;
using Android.Graphics;

namespace Spritist.Commands
{
    /// <summary>
    /// Structure to store point + color
    /// </summary>
    public class ColoredPoint
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

        public Point Point { get; set; }

        private Color? color;

        public Color Color
        {
            get
            {
                if (!color.HasValue)
                    throw new NullReferenceException($"{nameof(color)} is unset!");
                return color.Value;
            }
            set { color = value; }
        }

        public readonly float AlphaFloat;

        public ColoredPoint(Point point)
        {
            Point = point;
        }

        public ColoredPoint(Point point, Color color)
        {
            Point = point;
            Color = color;
        }
    }
}