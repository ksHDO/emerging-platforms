using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using Spritist.Amazon;
using Spritist.Utilities;
using Object = Java.Lang.Object;

namespace Spritist
{
    public class SpritistListAdapter : ArrayAdapter<string>
    {


        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            string key = GetItem(position);

            string file;
            var client = AmazonServices.Instance.Client;
            using (var response = client.GetObjectAsync(AmazonServices.BucketName, key).Result)
            using ( var reader = new StreamReader(response.ResponseStream))
            {
                file = reader.ReadToEnd();
            }

            var data = JsonConvert.DeserializeObject<SpritistData>(file);

            if (convertView == null)
            {
                LayoutInflater inflater = LayoutInflater.From(Context);
                convertView = inflater.Inflate(Resource.Layout.main_list_object, parent, false);

            }

            ImageView imageView = convertView.FindViewById<ImageView>(Resource.Id.main_list_object_image);
            TextView textView = convertView.FindViewById<TextView>(Resource.Id.main_list_object_text);

            byte[] decoded = Base64.Decode(data.ImageData, Base64Flags.Default);
            Bitmap bitmap = BitmapFactory.DecodeByteArray(decoded, 0, decoded.Length);
            imageView.SetImageBitmap(bitmap);
            textView.Text = data.SpriteName;
            
            return convertView;
            // return base.GetView(position, convertView, parent);
        }

        public SpritistListAdapter(Context context, IEnumerable<string> objects) : base(context, 0, objects.ToArray())
        {

        }

        public SpritistListAdapter(IntPtr handle, JniHandleOwnership transfer) : base(handle, transfer)
        {
        }

        public SpritistListAdapter(Context context, int textViewResourceId) : base(context, textViewResourceId)
        {
        }

        public SpritistListAdapter(Context context, int resource, int textViewResourceId) : base(context, resource, textViewResourceId)
        {
        }

        public SpritistListAdapter(Context context, int textViewResourceId, string[] objects) : base(context, textViewResourceId, objects)
        {
        }

        public SpritistListAdapter(Context context, int resource, int textViewResourceId, string[] objects) : base(context, resource, textViewResourceId, objects)
        {
        }

        public SpritistListAdapter(Context context, int textViewResourceId, IList<string> objects) : base(context, textViewResourceId, objects)
        {
        }

        public SpritistListAdapter(Context context, int resource, int textViewResourceId, IList<string> objects) : base(context, resource, textViewResourceId, objects)
        {
        }
    }
}