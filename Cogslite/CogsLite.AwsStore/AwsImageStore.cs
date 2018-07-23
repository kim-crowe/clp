using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using CogsLite.Core;

namespace CogsLite.AwsStore
{
    public class AwsImageStore : IImageStore
    {
        private readonly IAmazonS3 _s3Service;

        public AwsImageStore(IAmazonS3 s3Service)
        {
            _s3Service = s3Service ?? throw new ArgumentNullException(nameof(s3Service));
        }

        public async Task Add(ImageData imageData)
        {
            var putRequest = new PutObjectRequest
            {
                BucketName = "CogsImages",
                Key = imageData.Id.ToString(),
                InputStream = new MemoryStream(imageData.Data),                
            };

            putRequest.Metadata.Add("FileName", imageData.OriginalFileName);

            var response = await _s3Service.PutObjectAsync(putRequest);
        }

        public async Task<ImageData> Get(Guid id)
        {
            var getRequest = new GetObjectRequest
            {
                BucketName = "CogsImages",
                Key = id.ToString(),
            };

            var response = await _s3Service.GetObjectAsync(getRequest);

            using (var binaryReader = new BinaryReader(response.ResponseStream))
            {
                return new ImageData
                {
                    Id = id,
                    OriginalFileName = response.Metadata["FileName"],
                    Data = binaryReader.ReadBytes((int)response.ContentLength)
                };
            }
        }
    }
}
