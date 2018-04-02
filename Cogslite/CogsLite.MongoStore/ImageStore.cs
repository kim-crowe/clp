using CogsLite.Core;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver.GridFS;
using MongoDB.Driver;
using System;
using System.IO;

namespace CogsLite.MongoStore
{
    public class ImageStore : BaseMongoStore, IImageStore
    {
        public ImageStore(IConfiguration configuration) : base(configuration)
        {
        }

        public void Add(ImageData imageData)
        {
            var database = GetDatabase();
            var imagesCollection = database.GetCollection<ImageData>("Images");
            imagesCollection.InsertOne(imageData);
        }

        public ImageData Get(Guid id)
        {
            var database = GetDatabase();
            var imagesCollection = database.GetCollection<ImageData>("Images");
            var filter = Builders<ImageData>.Filter.Where(x => x.Id == id);
            return imagesCollection.Find(filter).SingleOrDefault();
        }
    }
}
