using System.Collections.Generic;
using System.Linq;
using Android.Graphics;
using Spritist.Commands;


namespace Spritist.Tools
{
    public abstract class Tool
    {
        protected CommandHistory history;
        protected Bitmap bitmap;

        public Tool(CommandHistory history, Bitmap bitmap)
        {
            this.history = history;
            this.bitmap = bitmap;
        }

        public abstract void OnDown(int x, int y);
        public abstract void OnMove(int x, int y);
        public abstract void OnUp(int x, int y);

    }
}