using System;
using System.Threading.Tasks;

namespace CogsLite.Core
{
    public interface IImageStore
    {
        Task<string> Add(string associatedObjectType, Guid associatedObjectId, int version, string imageType, byte[] data);
        Task<byte[]> Get(string associatedObjectType, Guid associatedObjectId, int version);
    }
}
