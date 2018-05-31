using System.Collections.Generic;
using System.Linq;
using Android.Graphics;
using Spritist.Commands;
using Spritist;


namespace Spritist.Tools
{
    public class EraserTool : Tool
    {
        private DrawPathCommand command;

        public EraserTool(CommandHistory history, Bitmap bitmap) : base(history, bitmap)
        {
        }

        public override void OnDown(int x, int y)
        {
            // Needs erase path command instead
            command = new DrawPathCommand(bitmap, Color.White, DrawColor.CurrentSize);
            command.DrawOnPoint(x, y);
        }

        public override void OnMove(int x, int y)
        {
            command.DrawOnPoint(x, y);
        }

        public override void OnUp(int x, int y)
        {
            this.history.Perform(command);
        }

    }
}