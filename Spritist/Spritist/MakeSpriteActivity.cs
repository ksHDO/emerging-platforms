using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using Spritist.Amazon;
using Spritist.Commands;
using Spritist.Tools;
using Spritist.Utilities;
using static Android.Views.View;

namespace Spritist
{
    [Activity(Label = "MakeSpriteActivity")]
    public class MakeSpriteActivity : Activity, NavigationView.IOnNavigationItemSelectedListener
    {
        private string spriteName;
        int w = 16;
        int h = 16;
        Canvas canvas;
        BitmapDrawable mainSpriteDisplay;
        Bitmap sourceBitmap;
        ImageView canvasView;
        List<ImageView> tiledViews = new List<ImageView>(4);

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
        Button drawButton;

        float canvasX, canvasY = 0;
        bool down = false;

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

            Bundle extras = Intent.Extras;
            spriteName = extras.GetString(GetString(Resource.String.bundle_sprite_name));
            int[] dimensions = extras.GetIntArray(
                GetString(Resource.String.bundle_sprite_dimensions));

            w = dimensions[0];
            h = dimensions[1];


            //SetContentView(Resource.Layout.make_sprite);

            SetContentView(Resource.Layout.activity_make_sprite); // Drawer layout

            drawButton = FindViewById<Button>(Resource.Id.drawButton);
            drawButton.GetLocationOnScreen(buttonLocs);
            drawButton.Touch += OnbuttonPush;
            //// Setup Drawer
            drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_make_sprite);
            ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(this, drawerLayout, null,
                Resource.String.navigation_drawer_open,
                Resource.String.navigation_drawer_close);
            drawerLayout.AddDrawerListener(toggle);
            NavigationView navigationView = FindViewById<NavigationView>(Resource.Id.nav_view_create_sprite);
            navigationView.SetNavigationItemSelectedListener(this);


            canvasView = FindViewById<ImageView>(Resource.Id.imageView);
            cursorView = FindViewById<ImageView>(Resource.Id.cursorView);

            SetUpImage(ref this.canvas, ref this.mainSpriteDisplay, ref this.sourceBitmap, canvasView, w, h);

            commandHistory = new CommandHistory();
            //Seems redundant for now
            tools = new Tool[4];
            tools[0] = new PencilTool(commandHistory, sourceBitmap);
            tools[1] = new EraserTool(commandHistory, sourceBitmap);
            tools[2] = new FillTool(commandHistory, sourceBitmap);
            tools[3] = new ColorPickerTool(commandHistory, sourceBitmap);
            currentTool = tools[0];

            FrameLayout spriteMainLayout = FindViewById<FrameLayout>(Resource.Id.make_sprite_main_layout);
                spriteMainLayout.Touch += OnSpriteCanvasTouched;

            SetupSideImageViews(Resource.Id.imageViewLeft, sourceBitmap);
            SetupSideImageViews(Resource.Id.imageViewTop, sourceBitmap);
            SetupSideImageViews(Resource.Id.imageViewRight, sourceBitmap);
            SetupSideImageViews(Resource.Id.imageViewBottom, sourceBitmap);


            canvas.DrawARGB(255, 255, 0, 255);

            curX = cursorView.GetX();
            curY = cursorView.GetY();

            SetupButtons();

            
        }

        private void SetupSideImageViews(int id, Bitmap srcBitmap)
        {
            ImageView       view     = FindViewById<ImageView>(id);
            BitmapDrawable  drawable = new BitmapDrawable(srcBitmap);
            DrawableWrapper wr       = new AliasDrawableWrapper(drawable);
            view.SetImageDrawable(wr);
            tiledViews.Add(view);
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
            ImageButton eraserButton =
                FindViewById<ImageButton>(Resource.Id.make_sprite_eraser_button);
            ImageButton fillButton =
                FindViewById<ImageButton>(Resource.Id.make_sprite_fill_button);
            ImageButton settingsButton =
                FindViewById<ImageButton>(Resource.Id.make_sprite_settings_button);

            ImageButton colorPickerButton =
                FindViewById<ImageButton>(Resource.Id.make_sprite_dropper_button);
            ImageButton undoButton =
                FindViewById<ImageButton>(Resource.Id.make_sprite_undo_button);
            ImageButton redoButton =
                FindViewById<ImageButton>(Resource.Id.make_sprite_redo_button);

            menuButton.Click += (sender, args) =>
                drawerLayout.OpenDrawer((int)GravityFlags.Left);

            pencilButton.Click += (sender, args) =>
            {
                drawButton.Text = "Draw!";
                currentTool = tools[0];
            };
            eraserButton.Click += (sender, args) =>
            {
                drawButton.Text = "Erase!";
                currentTool = tools[1];
            };
            fillButton.Click += (sender, args) =>
            {
                drawButton.Text = "Fill!";
                currentTool = tools[2];
            };
            colorPickerButton.Click += (sender, args) =>
            {
                drawButton.Text = "Color Pick!";
                currentTool = tools[3];
            };

            settingsButton.Click += (sender, args) =>
            {
                ToolSettingsDialogFragment dialog =
                    new ToolSettingsDialogFragment(DrawColor.CurrentSize, DrawColor.CurrentColor, OnToolSettingsChanged);
                dialog.Show(FragmentManager, "tool_settings");
            };

            undoButton.Click +=
                (sender, args) =>
                {
                    commandHistory.Undo();
                    InvalidateImageViews();
                };
            redoButton.Click +=
                (sender, args) =>
                {
                    commandHistory.Redo();
                    InvalidateImageViews();
                };
        }

        private void OnToolSettingsChanged(ToolSettingsDialogFragment.ToolSettingResult obj)
        {
            DrawColor.CurrentSize = obj.PixelSize;
            DrawColor.CurrentColor = obj.Color;
        }

        public void MoveCursor(float dx, float dy)
        {
            cursorView.SetX(cursorView.GetX() + dx);
            cursorView.SetY(cursorView.GetY() + dy);
            cursorView.Invalidate();
        }

        public void InvalidateImageViews()
        {
            canvasView.Invalidate();
            foreach (ImageView tiledView in tiledViews)
            {
                tiledView.Invalidate();
            }
        }

        bool holding = false;
        private void OnbuttonPush(object sender, TouchEventArgs e)
        {
            if (e.Event.Action == MotionEventActions.Down)
            {
                currentTool.OnDown((int)this.canvasX, (int)this.canvasY);
                down = true;
                Console.WriteLine("Button pushed");
                Console.WriteLine("Canvas X, Y: " + canvasX.ToString() + "," + canvasY.ToString());
                InvalidateImageViews();
            }
            else if (e.Event.Action == MotionEventActions.Up)
            {
                currentTool.OnUp((int)this.canvasX, (int)this.canvasY);
                down = false;
                Console.WriteLine("Button released");
                InvalidateImageViews();
            }
        }

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
                if (y > buttonLocs[1])
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

                    if (cursorView.GetX() < canvasView.GetX() + 3.0f) cursorView.SetX(canvasView.GetX()+ 3.0f);
                    else if (cursorView.GetX() > canvasView.GetX() + canvasView.Width - 3.0f) cursorView.SetX(canvasView.GetX() + (canvasView.Width- 3.0f));
                    if (cursorView.GetY() < canvasView.GetY() + 3.0f) cursorView.SetY(canvasView.GetY()+3.0f);
                    else if (cursorView.GetY() > canvasView.GetY() + canvasView.Height - 3.0f) cursorView.SetY(canvasView.GetY() + (canvasView.Height- 3.0f));
                    cursorView.Invalidate();

                    int[] position = new int[3];
                    canvasView.GetLocationOnScreen(position);
                    float canvasX = cursorX - position[0];
                    float canvasY = cursorY - position[1];

                    canvasX = (canvasX / canvasView.Width) * (float)w;
                    canvasY = (canvasY / canvasView.Height) * (float)h;
                    this.canvasX = canvasX;
                    this.canvasY = canvasY;

                    if (down && canvasX >= 0.0f && canvasX < (float)w && canvasY >= 0.0f && canvasY < (float)h)
                    {

                        if (e.Action == MotionEventActions.Move)
                        {
                            currentTool.OnMove((int)canvasX, (int)canvasY);
                        }

                        InvalidateImageViews();
                    }
                }
            }     
            return base.OnTouchEvent(e);
        }

        public bool OnNavigationItemSelected(IMenuItem menuItem)
        {
            int id = menuItem.ItemId;

            if (id == Resource.Id.nav_upload)
            {
                UploadImage();
            }
            else if (id == Resource.Id.nav_exit_sprite)
            {
                this.Finish();
            }

            drawerLayout.CloseDrawer(GravityCompat.Start);
            return true;
        }

        private static void UploadImage()
        {
            // aws upload
            var services = AmazonServices.Instance;
            var client = services.Client;

            // pack things into a stream


            PutObjectRequest request = new PutObjectRequest()
            {
                BucketName = AmazonServices.BucketName,
                ContentType = AmazonServicesContentType.Binary
            };

            client.PutObjectAsync(request);
        }
    }
}