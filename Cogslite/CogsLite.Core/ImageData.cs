using System;

namespace CogsLite.Core
{
    public class ImageData : BaseObject
    {
        public string OriginalFileName { get; set; }
        public byte[] Data { get; set; }
    }
}
