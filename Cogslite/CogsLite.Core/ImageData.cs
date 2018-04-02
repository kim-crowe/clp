using System;

namespace CogsLite.Core
{
    public class ImageData
    {
        public Guid Id { get; set; }
        public string OriginalFileName { get; set; }
        public byte[] Data { get; set; }
    }
}
