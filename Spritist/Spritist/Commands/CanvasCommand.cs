using Android.Graphics;

namespace Spritist.Commands
{
    public abstract class CanvasCommand : ICommand
    {
        protected Canvas canvas;
        

        protected CanvasCommand(Canvas canvas)
        {
            this.canvas = canvas;
        }

        //public abstract void Perform();
        public abstract void Undo();
        public abstract void Redo();
    }
}