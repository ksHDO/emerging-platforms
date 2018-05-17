using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace Spritist
{
    /// <summary>
    /// Testing view, please ignore
    /// </summary>
    /// <seealso cref="Android.Views.View" />
    public class TestView : View
    {
        public Bitmap bitmap;
        public Canvas canvas;

        public TestView(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {
            Initialize();
        }

        public TestView(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle)
        {
            Initialize();
        }

        private void Initialize()
        {
            bitmap = Bitmap.CreateBitmap(64, 64, Bitmap.Config.Argb8888);
            canvas = new Canvas(bitmap);
        }

        
    }
}