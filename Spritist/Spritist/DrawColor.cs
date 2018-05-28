using Android.Graphics;

namespace Spritist
{
    public static class DrawColor
    {
        public static Color CurrentColor { get; set; }
        static DrawColor()
        {
            CurrentColor = Color.Black;
        }
    }
}