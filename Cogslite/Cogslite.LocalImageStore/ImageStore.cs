using System;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using CogsLite.Core;
using Microsoft.Extensions.Options;

namespace CogsLite.LocalImageStore
{
    public class ImageStore : IImageStore
    {        
        private ImageStoreOptions _options;
        public ImageStore(IOptions<ImageStoreOptions> options)
        {
            _options = options.Value;
        }

        public Task<string> Add(string associatedObjectType, Guid associatedObjectId, int version, string imageType, byte[] data)
        {
            EnsureDirectory(associatedObjectType);
            File.WriteAllBytes($"{_options.BasePath}\\{associatedObjectType}\\{associatedObjectId}.{imageType}", data);
            var imageUri = $"{_options.BaseUri}/{associatedObjectType}/{associatedObjectId}.{imageType}";
            return Task.FromResult(imageUri);
        }

        public Task<byte[]> Get(string associatedObjectType, Guid associatedObjectId, int version)
        {
            return Task.Run(() =>
            {
                var file = Directory
                    .GetFiles($"{_options.BasePath}\\{associatedObjectType}", $"{associatedObjectId}.*")
                    .SingleOrDefault();

                if (file != null)
                    return File.ReadAllBytes(file);
                
                return null;
            });
        }

        private void EnsureDirectory(string objectType)
        {
            var path = $"{_options.BasePath}\\{objectType}";
            if(Directory.Exists(path) == false)
            {
                Directory.CreateDirectory(path);
            }
        }
    }

    public class ImageStoreOptions
    {
        public string BasePath { get; set; }
        public string BaseUri { get; set; }
    }
}