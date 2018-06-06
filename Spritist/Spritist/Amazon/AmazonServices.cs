using System;
using System.IO;
using System.Threading.Tasks;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Android.Content.Res;
using Android.Util;
using Newtonsoft.Json;

namespace Spritist.Amazon
{
    public class AmazonServices
    {
        public static string BucketName;
        private readonly string AccessKey;
        private readonly string SecretKey;
        private const string File = "keys.json";
        private readonly RegionEndpoint BucketRegion = RegionEndpoint.USEast1;
        
        // TODO Shouldn't really be public but just trying to get things done
        public readonly AmazonS3Client Client;

        private AmazonServices(AssetManager assets)
        {
            using (StreamReader sr = new StreamReader(assets.Open(File)))
            {
                string contents = sr.ReadToEnd();
                var keys = JsonConvert.DeserializeAnonymousType(contents, new {BucketName, AccessKey, SecretKey});
                BucketName = keys.BucketName;
                AccessKey = keys.AccessKey;
                SecretKey = keys.SecretKey;
            }
            AWSCredentials credentials = new BasicAWSCredentials(AccessKey, SecretKey);
            Client = new AmazonS3Client(credentials, BucketRegion);
        }

        public async Task PutObject(string key, string content)
        {
            try
            {
                PutObjectRequest request = new PutObjectRequest()
                {
                    BucketName  = BucketName,
                    Key         = key,
                    ContentBody = content
                };
                await Client.PutObjectAsync(request);
                // Successfully put an object
            }
            catch (AmazonS3Exception e)
            {
                Log.Error("Spritist.AmazonServices", "Error in AmazonServices.PutObject({0}, {1}). Message: [{2}]", key, content, e.Message);
            }
            catch (Exception e)
            {
                Log.Error("Spritist.AmazonServices", "Unknown Error in AmazonServices.PutObject({0}, {1}). Message: [{2}]", key, content, e.Message);
            }
        }


        private static AmazonServices _instance;

        public static AmazonServices Instance(AssetManager assets)
        {
            if (_instance == null)
                _instance = new AmazonServices(assets);
            return _instance;
        }
    }
}