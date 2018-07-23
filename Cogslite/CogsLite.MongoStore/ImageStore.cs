using CogsLite.Core;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver.GridFS;
using MongoDB.Driver;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CogsLite.MongoStore
{
    public class ImageStore : BaseMongoStore<ImageData>, IImageStore
    {
        public ImageStore(IConfiguration configuration) : base("Images", configuration)
        {
        }

        public async Task Add(ImageData imageData)
        {
            await Insert(imageData);
        }

        public async Task<ImageData> Get(Guid id)
        {
            return FindById(id);
        }
    }
}
