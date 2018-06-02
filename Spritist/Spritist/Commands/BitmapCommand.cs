using Android.Graphics;

namespace Spritist.Commands
{
    public abstract class CanvasCommand : ICommand
    {
        protected class ColoredPoint
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

            public readonly float AlphaFloat;

            public ColoredPoint(Point point, Color color)
            {
                Point = point;
                Color = color;
            }
        }

        protected Bitmap bitmap;
        

        protected CanvasCommand(Bitmap bitmap)
        {
            this.bitmap = bitmap;
        }

        public abstract void Undo();
        public abstract void Redo();
    }
}