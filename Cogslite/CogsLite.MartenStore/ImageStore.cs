using System;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using CogsLite.Core;

namespace CogsLite.MartenStore
{
    public class ImageStore : IImageStore
    {        
        public Task<string> Add(string associatedObjectType, Guid associatedObjectId, string imageType, byte[] data)
        {
            File.WriteAllBytes($"wwwroot\\images\\store\\{associatedObjectType}\\{associatedObjectId}.{imageType}", data);
            var filePath = $"/images/store/{associatedObjectType}/{associatedObjectId}.{imageType}";
            return Task.FromResult(filePath);
        }

        public Task<byte[]> Get(string associatedObjectType, Guid associatedObjectId)
        {
            return Task.Run(() =>
            {
                var file = Directory
                    .GetFiles($"wwwroot\\images\\store\\{associatedObjectType}", $"{associatedObjectId}.*")
                    .SingleOrDefault();

                if (file != null)
                    return File.ReadAllBytes(file);
                
                return null;
            });
        }
    }
}