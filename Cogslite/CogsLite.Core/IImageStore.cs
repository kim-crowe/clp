using System;

namespace CogsLite.Core
{
    public interface IImageStore
    {
        void Add(ImageData imageData);
        ImageData Get(Guid id);
    }
}
