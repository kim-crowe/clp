using System.Drawing;

namespace CogsLite.Core
{
	public class JsonSize
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
