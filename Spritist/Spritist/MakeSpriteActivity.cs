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
        int w = 16;
        int h = 16;
        Canvas canvas;
        BitmapDrawable mainSpriteDisplay;
        Bitmap sourceBitmap;
        ImageView imageView;

        Canvas cursorCanvas;
        BitmapDrawable cursorSpriteDisplay;
        Bitmap cursorBitmap;
        ImageView cursorView;

        float curX = 0;
        float curY = 0;


        protected void SetUpImage(ref Canvas canvas, ref BitmapDrawable drawable, ref Bitmap bitmap, ImageView view, int w, int h)
        {
            bitmap = Bitmap.CreateBitmap(w, h, Bitmap.Config.Argb8888);
            drawable = new BitmapDrawable(bitmap);
            canvas = new Canvas(bitmap);
            DrawableWrapper wr = new AliasDrawableWrapper(drawable);
            view.SetImageDrawable(wr);

        }


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            int[] dimensions = Intent.Extras.GetIntArray(
                GetString(Resource.String.bundle_sprite_dimensions));

            w = dimensions[0];
            h = dimensions[1];

            SetContentView(Resource.Layout.make_sprite);
            imageView = FindViewById<ImageView>(Resource.Id.imageView);
            cursorView = FindViewById<ImageView>(Resource.Id.cursorView);
            SetUpImage(ref this.canvas, ref this.mainSpriteDisplay, ref this.sourceBitmap, imageView, w, h);
            //SetUpImage(ref this.cursorCanvas, ref this.cursorSpriteDisplay, ref this.cursorBitmap, cursorView, 16, 16);

            //sourceBitmap = Bitmap.CreateBitmap(w, h, Bitmap.Config.Argb8888); //Temporary width height
            //mainSpriteDisplay = new BitmapDrawable(sourceBitmap);
            //canvas = new Canvas(sourceBitmap);

            //DrawableWrapper wr = new AliasDrawableWrapper(mainSpriteDisplay);
            //imageView.SetImageDrawable(wr);

            canvas.DrawARGB(255, 255, 0, 255);
            //cursorCanvas.DrawARGB(30, 200, 30, 255);

            canvas.DrawLine(0, 0, 25, 25, new Paint()
            {
                Color = Color.Yellow
            });
            canvas.DrawPoint(2, 2, new Paint()
            {
                Color = Color.Red
            });

            curX = cursorView.GetX();
            curY = cursorView.GetY();

        }

        public void MoveCursor(float dx, float dy)
        {
            cursorView.SetX(cursorView.GetX() + dx);
            cursorView.SetY(cursorView.GetY() + dy);
            cursorView.Invalidate();
        }


        bool holding = false;

        public override bool OnTouchEvent(MotionEvent e)
        {

            float x = e.GetX();
            float y = e.GetY();

            if (!holding && e.Action == MotionEventActions.Down)
            {
                holding = true;
                curX = x;
                curY = y;
            }
            else if (e.Action == MotionEventActions.Up)
            {
                holding = false;
            }
            float cursorX = 0;
            float cursorY = 0;
            if (holding)
            {
                int[] locs = new int[3];
                cursorView.GetLocationOnScreen(locs);

                float dx = x - curX;
                float dy = y - curY;
                cursorX = locs[0] + dx;
                cursorY = locs[1] + dy;
                curX = x;
                curY = y;
                MoveCursor(dx, dy);
            }

            // imageView.GetX
            int[] position = new int[2];
            imageView.GetLocationOnScreen(position);
            float canvasX = cursorX - position[0];
            float canvasY = cursorY - position[1];

            canvasX = (canvasX / imageView.Width) * (float)w;
            canvasY = (canvasY / imageView.Height) * (float)h;

            if (canvasX >= 0.0f && canvasX < (float)w && canvasY >= 0.0f && canvasY < (float)h)
            {
                //sourceBitmap.SetPixel((int)canvasX, (int)canvasY, Color.Black);
                canvas.DrawPoint(canvasX, canvasY, new Paint()
                {
                    Color = Color.Blue
                });
                imageView.Invalidate();
            }
            
            
            return base.OnTouchEvent(e);
        }
    }
}