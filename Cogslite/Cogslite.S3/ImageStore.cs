using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using CogsLite.Core;

namespace CogsLite.AwsStore
{
    public class ImageStore : IImageStore
    {
        private const string DistributionUri = @"https://d7knsuvwz5w4z.cloudfront.net/";
        private const string BucketName = "cogs-images";
        private readonly IAmazonS3 _s3Service;

        public ImageStore(IAmazonS3 s3Service)
        {
            _s3Service = s3Service ?? throw new ArgumentNullException(nameof(s3Service));
        }

        public async Task<string> Add(string associatedObjectType, Guid associatedObjectId, int version, string imageType, byte[] data)
        {
            var putRequest = new PutObjectRequest
            {
                BucketName = BucketName,
                Key =  GetKey(associatedObjectType, associatedObjectId),
                InputStream = new MemoryStream(data),
                CannedACL = S3CannedACL.PublicRead
            };

            putRequest.Metadata.Add("ImageType", imageType);
            putRequest.Metadata.Add("Version", version.ToString());

            var response = await _s3Service.PutObjectAsync(putRequest);

            if(response.HttpStatusCode != HttpStatusCode.OK)
            {
                throw new InvalidOperationException("Failed to store image");
            }

            return DistributionUri + putRequest.Key;
        }
        
        public async Task<byte[]> Get(string associatedObjectType, Guid associatedObjectId, int version)
        {
            var imageKey = GetKey(associatedObjectType, associatedObjectId);

            var getRequest = new GetObjectRequest
            {
                BucketName = BucketName,
                Key = imageKey,
            };

            try
            {
                var response = await _s3Service.GetObjectAsync(getRequest);
                if (response.Metadata["x-amz-meta-version"] == version.ToString())
                {                
                    using (var binaryReader = new BinaryReader(response.ResponseStream))
                    {
                        return binaryReader.ReadBytes((int)response.ContentLength);                            
                    }
                }
            }
            catch(AmazonS3Exception ex)
            {
                if(ex.StatusCode != HttpStatusCode.NotFound)
                    throw;
            }

            return null;
        }        

        private string GetKey(string type, Guid id)
        {
            return $"{type}/{id}";
        }
    }
}