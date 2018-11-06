using System.Threading.Tasks;
using CogsLite.Core;

namespace CogsLite.MartenStore
{
    public class ImageStore : IImageStore
    {
        public Task Add(ImageData imageData)
        {
            throw new System.NotImplementedException();
        }

        public Task<ImageData> Get(string id)
        {
            throw new System.NotImplementedException();
        }
    }
}