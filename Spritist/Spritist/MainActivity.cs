using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Amazon.S3;
using Amazon.S3.Model;
using Android;
using Android.App;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using Spritist.Amazon;
using Spritist.Fragments;
using Spritist.Utilities;

namespace Spritist
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener
    {
        private ListView listView;

        protected override void OnCreate(Bundle savedInstanceState)
        {     
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            
            FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += FabOnClick;

            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(this, drawer, toolbar, Resource.String.navigation_drawer_open, Resource.String.navigation_drawer_close);
            drawer.AddDrawerListener(toggle);
            toggle.SyncState();

            NavigationView navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navigationView.SetNavigationItemSelectedListener(this);

            listView = FindViewById<ListView>(Resource.Id.content_main_list);

            RetrieveListOfObjects();
        }

        public override void OnBackPressed()
        {
            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            if(drawer.IsDrawerOpen(GravityCompat.Start))
            {
                drawer.CloseDrawer(GravityCompat.Start);
            }
            else
            {
                base.OnBackPressed();
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                RetrieveListOfObjects();
                Toast.MakeText(this, "Refreshed List.", ToastLength.Short);
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        private void FabOnClick(object sender, EventArgs eventArgs)
        {
            // View view = (View) sender;
            // Snackbar.Make(view, "Add button has been pressed", Snackbar.LengthLong)
            //     .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
            
            CreateSpriteDialogFragment dialog = new CreateSpriteDialogFragment();
            dialog.Show(FragmentManager, "create_sprite");
            // StartActivity(typeof(MakeSpriteActivity));
        }

        public bool OnNavigationItemSelected(IMenuItem item)
        {
            int id = item.ItemId;

            if (id == Resource.Id.nav_camera)
            {
                // Handle the camera action
            }
            else if (id == Resource.Id.nav_gallery)
            {

            }
            else if (id == Resource.Id.nav_slideshow)
            {

            }
            else if (id == Resource.Id.nav_manage)
            {

            }
            else if (id == Resource.Id.nav_share)
            {

            }
            else if (id == Resource.Id.nav_send)
            {

            }

            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            drawer.CloseDrawer(GravityCompat.Start);
            return true;
        }

        public async void RetrieveListOfObjects()
        {
            AmazonS3Client client = AmazonServices.Instance(Assets).Client;
            ListObjectsRequest listRequest = new ListObjectsRequest()
            {
                BucketName = AmazonServices.BucketName
            };
            try
            {
                ListObjectsResponse listResponse = await client.ListObjectsAsync(listRequest);

                var objects = listResponse.S3Objects.Select(o =>
                {
                    string file;
                    using (var response = client.GetObjectAsync(AmazonServices.BucketName, o.Key).Result)
                    using (var reader = new StreamReader(response.ResponseStream))
                    {
                        file = reader.ReadToEnd();
                    }

                    SpritistData data = JsonConvert.DeserializeObject<SpritistData>(file);
                    return data;
                });
                listView.Adapter = new SpritistListAdapter(this, objects);
            }
            catch (Exception e)
            {
                Toast.MakeText(this, "Could not load images", ToastLength.Long);
            }

        }
    }
}

