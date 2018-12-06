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

            throw new NotImplementedException("Need to figure out how to return url for the image");
        }
        
        public async Task<byte[]> Get(string associatedObjectType, Guid associatedObjectId, int version)
        {
            var imageKey = GetKey(associatedObjectType, associatedObjectId);

            var getMetaDataRequest = new GetObjectMetadataRequest
            {
                BucketName = BucketName,
                Key = imageKey 
            };

            var metaDataResponse = await _s3Service.GetObjectMetadataAsync(getMetaDataRequest);

            if(metaDataResponse.HttpStatusCode == HttpStatusCode.OK)
            {
                var storedVersion = Int32.Parse(metaDataResponse.Metadata["Version"]);
                if(storedVersion == version)
                {
                    var getRequest = new GetObjectRequest
                    {
                        BucketName = BucketName,
                        Key = imageKey,
                    };

                    var response = await _s3Service.GetObjectAsync(getRequest);
                    if(response.HttpStatusCode == HttpStatusCode.OK)
                    {
                        using (var binaryReader = new BinaryReader(response.ResponseStream))
                        {
                            return binaryReader.ReadBytes((int)response.ContentLength);                            
                        }
                    }
                }    
            }
            
            return null;
        }        

        private string GetKey(string type, Guid id)
        {
            return $"{type}/{id}";
        }
    }
}