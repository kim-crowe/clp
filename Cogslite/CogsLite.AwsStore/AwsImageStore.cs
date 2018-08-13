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
    public class AwsImageStore : IImageStore
    {
        private const string BucketName = "cogs-images";
        private readonly IAmazonS3 _s3Service;

        public AwsImageStore(IAmazonS3 s3Service)
        {
            _s3Service = s3Service ?? throw new ArgumentNullException(nameof(s3Service));
        }

        public async Task Add(ImageData imageData)
        {
            var putRequest = new PutObjectRequest
            {
                BucketName = BucketName,
                Key = imageData.Id.ToString(),
                InputStream = new MemoryStream(imageData.Data),
                StorageClass = new S3StorageClass("REDUCED_REDUNDANCY"),
                CannedACL = S3CannedACL.PublicRead
            };

            putRequest.Metadata.Add("FileName", imageData.OriginalFileName);

            var response = await _s3Service.PutObjectAsync(putRequest);

            Console.WriteLine(response.HttpStatusCode);
            Console.WriteLine(response.ToString());
        }

        public async Task<ImageData> Get(string id)
        {
            var getRequest = new GetObjectRequest
            {
                BucketName = BucketName,
                Key = id,
            };

            var response = await _s3Service.GetObjectAsync(getRequest);

            if(response.HttpStatusCode != HttpStatusCode.OK)
                return null;

            using (var binaryReader = new BinaryReader(response.ResponseStream))
            {
                return new ImageData
                {
                    Id = id,
                    OriginalFileName = response.Metadata["FileName"],
                    Data = binaryReader.ReadBytes((int)response.ContentLength),
                    Version = response.Metadata.GetInt32("Version")
                };
            }
        }
    }
}
