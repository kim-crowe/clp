using System;
using System.Drawing;

namespace CogsLite.Core
{
    public class Game : BaseObject
    {
        public string Name { get; set; }
        public Member Owner { get; set; }
        public DateTime CreatedOn { get; set; }
        public JsonSize CardSize {get; set;}
    }

    [Serializable]
    public struct JsonSize
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public static implicit operator Size(JsonSize s)
        {
            return new Size(s.Width, s.Height);
        }

        public static implicit operator JsonSize(Size s)
        {
            return new JsonSize { Width = s.Width, Height = s.Height };
        }
    }
}
