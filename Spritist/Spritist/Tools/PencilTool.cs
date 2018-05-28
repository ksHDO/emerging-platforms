using System.Collections.Generic;
using System.Linq;
using Android.Graphics;
using Spritist.Commands;
using Spritist;


namespace Spritist.Tools
{
    public class PencilTool : Tool
    {
        private DrawPathCommand command;
        
        public PencilTool(CommandHistory history, Bitmap bitmap) : base(history, bitmap)
        {
        }

        public override void OnDown(int x, int y)
        {
            command = new DrawPathCommand(bitmap, DrawColor.CurrentColor);
            command.AddPixel(x, y);
        }

        public override void OnMove(int x, int y)
        {
            command.AddPixel(x, y);
        }

        public override void OnUp(int x, int y)
        {
            this.history.Perform(command);
        }

    }
}