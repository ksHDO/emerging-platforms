using System.IO;
using Android.Graphics;
using Android.Util;
using Java.IO;
using Newtonsoft.Json;

namespace Spritist.Utilities
{
    /// <summary>
    /// Used for uploading to AWS.
    /// </summary
    [JsonObject]
    public class SpritistData
    {
        public string SpriteName;
        public string ImageData;

        public SpritistData()
        {

        }

        public SpritistData(string name, Bitmap bitmap)
        {
            SpriteName = name;
            SetImageData(bitmap);
        }

        public void SetImageData(Bitmap bitmap)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Compress(Bitmap.CompressFormat.Png, 100, stream);
                byte[] array = stream.ToArray();
                ImageData = Base64.EncodeToString(array, Base64Flags.Default);
            }
        }
    }
}