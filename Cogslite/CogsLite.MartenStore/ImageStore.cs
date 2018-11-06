using System.IO;
using System.Threading.Tasks;
using CogsLite.Core;

namespace CogsLite.MartenStore
{
    public class ImageStore : IImageStore
    {        
        public Task Add(ImageData imageData)
        {
            File.WriteAllBytes($"image_data\\{imageData.Id}.dat", imageData.Data);
            return Task.CompletedTask;
        }

        public Task<ImageData> Get(string id)
        {            
            var result = new ImageData
            {
                Id = id,
                Data = File.ReadAllBytes($"image_data\\{id}.dat"),
                OriginalFileName = string.Empty,
                Version = 1
            };

            return Task.FromResult(result);
        }
    }
}