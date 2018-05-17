using System;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Runtime;

namespace Spritist
{
    public class AliasDrawableWrapper : DrawableWrapper
    {
        private readonly DrawFilter DrawFilter = new PaintFlagsDrawFilter(PaintFlags.FilterBitmap, 0);

        public AliasDrawableWrapper(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public AliasDrawableWrapper(Drawable dr) : base(dr)
        {
        }

        public override void Draw(Canvas canvas)
        {
            var oldFilter = canvas.DrawFilter;
            canvas.DrawFilter = DrawFilter;
            base.Draw(canvas);
            canvas.DrawFilter = oldFilter;
        }
    }
}