using System.Collections.Generic;
using Android.Graphics;

namespace Spritist.Commands
{
    public class FillCommand : ICommand
    {
        private Bitmap bitmap;
        private Color paintColor;
        private readonly List<Point>        points;
        private readonly List<ColoredPoint> overwrittenPoints;

        public FillCommand(Bitmap bitmap, Color color)
        {
            this.bitmap       = bitmap;
            this.paintColor   = color;

            points            = new List<Point>();
            overwrittenPoints = new List<ColoredPoint>(bitmap.Width);
        }

        public void Fill(int x, int y)
        {
            Point point = new Point(x, y);
            Fill(point, GetPixel(point));
        }

        private void Fill(Point node, int targetArgb)
        {
            int paintArgb = paintColor.ToArgb();

            if (targetArgb == paintArgb) return;
            if (bitmap.GetPixel(node.X, node.Y) == paintColor.ToArgb()) return;

            Queue<Point> queue = new Queue<Point>();
            queue.Enqueue(node);
            while (queue.Count > 0)
            {
                Point item = queue.Dequeue();
                Point w = new Point(item), e = new Point(item);

                do
                {
                    w.X--;
                } while (w.X >= 0 && GetPixel(w) == targetArgb);

                do
                {
                    e.X++;
                } while (e.X < bitmap.Width && GetPixel(e) == targetArgb);

                for (int i = w.X + 1; i <= e.X - 1; ++i)
                {
                    Point thisNode = new Point(i, w.Y);
                    SetPixel(thisNode, paintColor);
                    Point n = new Point(thisNode.X, thisNode.Y - 1);
                    Point s = new Point(thisNode.X, thisNode.Y + 1);
                    if (n.Y >= 0 && GetPixel(n) == targetArgb)
                        queue.Enqueue(n);
                    if (s.Y < bitmap.Height && GetPixel(s) == targetArgb)
                        queue.Enqueue(s);
                }
            }
        }

        private int GetPixel(Point node)
        {
            return bitmap.GetPixel(node.X, node.Y);
        }

        private void SetPixel(Point node, Color color)
        {
            points.Add(node);
            overwrittenPoints.Add(new ColoredPoint(node, new Color(GetPixel(node))));
            bitmap.SetPixel(node.X, node.Y, color);
        }

        public void Redo()
        {
            foreach (Point point in points)
            {
                bitmap.SetPixel(point.X, point.Y, paintColor);
            }
        }

        public void Undo()
        {
            for (int i = overwrittenPoints.Count - 1; i >= 0; --i)
            {
                ColoredPoint p = overwrittenPoints[i];
                bitmap.SetPixel(p.X, p.Y, p.Color);
            }
        }
    }
} 