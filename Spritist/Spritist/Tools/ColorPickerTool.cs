using Android.Graphics;
using Spritist.Commands;

namespace Spritist.Tools
{
    public class ColorPickerTool : Tool
    {
        public ColorPickerTool(CommandHistory history, Bitmap bitmap) : base(history, bitmap)
        {
        }

        public override void OnDown(int x, int y)
        {
            
        }

        public override void OnMove(int x, int y)
        {
            
        }

        public override void OnUp(int x, int y)
        {
            DrawColor.CurrentColor = new Color(bitmap.GetPixel(x, y));
        }
    }
}