using System;
using System.Threading.Tasks;

namespace CogsLite.Core
{
    public interface IImageStore
    {
        Task Add(ImageData imageData);        
        Task<ImageData> Get(string id);
    }
}
