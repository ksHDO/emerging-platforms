using System;
using System.Threading.Tasks;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Android.Util;

namespace Spritist.Amazon
{
    public class AmazonServices
    {
        public const string BucketName = "spritistbucket";
        private const string AccessKey = "AKIAI3B3UQEGSAZP2RMQ";
        private const string SecretKey = "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";
        private static readonly RegionEndpoint BucketRegion = RegionEndpoint.USEast1;
        
        // TODO Shouldn't really be public but just trying to get things done
        public readonly AmazonS3Client Client;

        private AmazonServices()
        {
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
        public static AmazonServices Instance
        {
            get
            {
                if (_instance != null)
                    _instance = new AmazonServices();
                return _instance;
            }
        }


    }
}