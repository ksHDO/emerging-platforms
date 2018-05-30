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
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Spritist.Commands;
using Spritist.Tools;


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

        private CommandHistory commandHistory;
        private DrawPathCommand drawPathCommand;

        private Tool[] tools;
        private Tool currentTool;

        private DrawerLayout drawerLayout;
        int[] buttonLocs = new int[3];
        
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

            //SetContentView(Resource.Layout.make_sprite);

            SetContentView(Resource.Layout.activity_make_sprite); // Drawer layout

            FindViewById<Button>(Resource.Id.drawButton).GetLocationOnScreen(buttonLocs);
            //// Setup Drawer
            drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_make_sprite);
            ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(this, drawerLayout, null,
                Resource.String.navigation_drawer_open,
                Resource.String.navigation_drawer_close);
            drawerLayout.AddDrawerListener(toggle);

            imageView = FindViewById<ImageView>(Resource.Id.imageView);
            cursorView = FindViewById<ImageView>(Resource.Id.cursorView);

            SetUpImage(ref this.canvas, ref this.mainSpriteDisplay, ref this.sourceBitmap, imageView, w, h);

            commandHistory = new CommandHistory();
            //Seems redundant for now
            tools = new Tool[1];
            tools[0] = new PencilTool(commandHistory, sourceBitmap);
            currentTool = tools[0];

            FrameLayout spriteMainLayout = FindViewById<FrameLayout>(Resource.Id.make_sprite_main_layout);
                spriteMainLayout.Touch += OnSpriteCanvasTouched;

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



            SetupButtons();
        }

        private void OnSpriteCanvasTouched(object sender, View.TouchEventArgs e)
        {
            OnCanvasTouchEvent(e.Event);
        }

        private void SetupButtons()
        {
            ImageButton menuButton =
                FindViewById<ImageButton>(Resource.Id.make_sprite_menu_button);
            ImageButton pencilButton =
                FindViewById<ImageButton>(Resource.Id.make_sprite_pencil_button);
            ImageButton pencilSettingsButton =
                FindViewById<ImageButton>(Resource.Id.make_sprite_pencil_settings_button);
            ImageButton eraserButton =
                FindViewById<ImageButton>(Resource.Id.make_sprite_eraser_button);
            ImageButton eraserSettingsButton =
                FindViewById<ImageButton>(Resource.Id.make_sprite_eraser_settings_button);
            ImageButton undoButton =
                FindViewById<ImageButton>(Resource.Id.make_sprite_undo_button);
            ImageButton redoButton =
                FindViewById<ImageButton>(Resource.Id.make_sprite_redo_button);

            menuButton.Click += (sender, args) =>
                drawerLayout.OpenDrawer((int)GravityFlags.Left);

            pencilSettingsButton.Click += (sender, args) =>
            {
                ToolSettingsDialogFragment dialog =
                    new ToolSettingsDialogFragment(OnPencilSettingsChanged);
                var dialogArguments = new Bundle();
                dialogArguments.PutString(GetString(Resource.String.bundle_draw_tool_name), "Pencil");
                dialog.Arguments = dialogArguments;
                dialog.Show(FragmentManager, "pencil_settings");
            };

            undoButton.Click +=
                (sender, args) =>
                {
                    commandHistory.Undo();
                    imageView.Invalidate();
                };
            redoButton.Click +=
                (sender, args) =>
                {
                    commandHistory.Redo();
                    imageView.Invalidate();
                };
        }

        private void OnPencilSettingsChanged(ToolSettingsDialogFragment.ToolSettingResult obj)
        {
            
        }

        public void MoveCursor(float dx, float dy)
        {
            cursorView.SetX(cursorView.GetX() + dx);
            cursorView.SetY(cursorView.GetY() + dy);
            cursorView.Invalidate();
        }


        bool holding = false;

        public bool OnCanvasTouchEvent(MotionEvent e)
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

            if (e.Action == MotionEventActions.Down)
            {
                //drawPathCommand = new DrawPathCommand(sourceBitmap, Color.Blue);
                currentTool.OnDown((int)canvasX, (int)canvasY);
            }
            else if (e.Action == MotionEventActions.Up)
            {
                //commandHistory.Perform(drawPathCommand);
                currentTool.OnUp((int)canvasX, (int)canvasY);
            }

            if (canvasX >= 0.0f && canvasX < (float)w && canvasY >= 0.0f && canvasY < (float)h)
            {
                //sourceBitmap.SetPixel((int)canvasX, (int)canvasY, Color.Black);
                //canvas.DrawPoint(canvasX, canvasY, new Paint()
                //{
                //    Color = Color.Blue
                //});
                if (e.Action == MotionEventActions.Move)
                {
                    //drawPathCommand.AddPixel((int) canvasX, (int) canvasY);
                    currentTool.OnMove((int)canvasX, (int)canvasY);
                }

                imageView.Invalidate();
            }
            
            
            return base.OnTouchEvent(e);
        }
    }
}