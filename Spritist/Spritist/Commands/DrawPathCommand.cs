using Android.Graphics;

namespace Spritist.Commands
{
    public class DrawPathCommand : CanvasCommand
    {
        private Path path;
        private Paint paint;

        public DrawPathCommand(Canvas canvas, Path path, Paint paint) : base(canvas)
        {
            this.path = path;
            this.paint = paint;
        }

        public override void Perform()
        {
            canvas.DrawPath(path, paint);
        }

        public override void Undo()
        {
            
        }
    }
}