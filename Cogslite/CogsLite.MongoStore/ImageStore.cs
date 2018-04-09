using CogsLite.Core;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver.GridFS;
using MongoDB.Driver;
using System;
using System.IO;

namespace CogsLite.MongoStore
{
    public class ImageStore : BaseMongoStore<ImageData>, IImageStore
    {
        public ImageStore(IConfiguration configuration) : base("Images", configuration)
        {
        }

        public void Add(ImageData imageData)
        {
            Insert(imageData);            
        }

        public ImageData Get(Guid id)
        {
            return FindById(id);
        }
    }
}
