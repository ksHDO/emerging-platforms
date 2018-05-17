using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Spritist
{
    [Activity(Label = "MakeSpriteActivity")]
    public class MakeSpriteActivity : Activity
    {

        Canvas canvas;
        BitmapDrawable mainSpriteDisplay;
        Bitmap sourceBitmap;
        Bitmap scaledBmp;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.make_sprite);
            sourceBitmap = Bitmap.CreateBitmap(16, 16, Bitmap.Config.Argb8888); //Temporary width height
            mainSpriteDisplay = new BitmapDrawable(sourceBitmap);
            canvas = new Canvas(sourceBitmap);

            ImageView imageView = FindViewById<ImageView>(Resource.Id.imageView);
            // Bitmap.CreateScaledBitmap(mainSpriteDisplay, 256, 256, false);
            //imageView.
            DrawableWrapper wr = new AliasDrawableWrapper(mainSpriteDisplay);
            
            imageView.SetImageDrawable(wr);
            
            canvas.DrawARGB(255, 255, 0, 255);

            canvas.DrawLine(0, 0, 25, 25, new Paint()
            {
                Color = Color.Yellow
            });
            canvas.DrawPoint(2, 2, new Paint()
            {
                Color = Color.Red
            });
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            
            return base.OnTouchEvent(e);
        }
    }
}