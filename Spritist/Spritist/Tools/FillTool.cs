using Android.Graphics;
using Spritist.Commands;

namespace Spritist.Tools
{
    public class FillTool : Tool
    {
        private FillCommand fillCommand;

        public FillTool(CommandHistory history, Bitmap bitmap) : base(history, bitmap)
        {
        }

        public override void OnDown(int x, int y)
        {
            fillCommand = new FillCommand(bitmap, DrawColor.CurrentColor);
            fillCommand.Fill(x, y);
            history.Perform(fillCommand);
        }

        public override void OnMove(int x, int y)
        {
            // Lol no move
        }

        public override void OnUp(int x, int y)
        {
            // Lol no up
        }
    }
}