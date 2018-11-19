using System.IO;
using SixLabors.Primitives;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.PixelFormats;
using System.Collections.Generic;
using System;

namespace Cogslite
{
    public class ImageSlicer : IDisposable
    {
        private readonly int _rows;
        private readonly int _cardsPerRow;
        private readonly System.Drawing.Size _cardSize;
        private readonly Image<Rgba32> _image;

        public ImageSlicer(int cardWidth, int cardHeight, Stream imageData)
        {
            _cardSize = new System.Drawing.Size(cardWidth, cardHeight);            
            _image = Image.Load(imageData);

            if(_image.Width % cardWidth > 0)
                throw new ArgumentException("Card width is not consistent with image width");

            if(_image.Height % cardHeight > 0)
                throw new ArgumentException("Card height is not consistent with image height");

            _cardsPerRow = _image.Width / cardWidth;
            _rows = _image.Height / cardHeight;
        }

        public System.Drawing.Size CardSize => _cardSize;

        public IEnumerable<byte[]> Images
        {
            get
            {
                for (int row = 0; row < _rows; row++)
                {
                    for(int col = 0; col < _cardsPerRow; col++)
                    {
                        var splitImage = _image.Clone(x => x.Crop(new Rectangle(col * _cardSize.Width, row * _cardSize.Height, _cardSize.Width, _cardSize.Height)));

                        using (var memoryStream = new MemoryStream())
                        {
                            splitImage.SaveAsPng(memoryStream);
                            yield return memoryStream.GetBuffer();
                        }
                    }
                }
            }
        }

        public static byte[] Composite(IEnumerable<byte[]> imageData, int count)
        {
			Image<Rgba32> finalImage = null;
			var counter = 0;

            foreach (var image in imageData)
            {
				var cardImage = Image.Load(image);

				if (finalImage == null)
					finalImage = new Image<Rgba32>(cardImage.Width * 10, cardImage.Height * 7);
                
                var row = counter / 10;
                var column = counter % 10;

                finalImage.Mutate(x => x.DrawImage(cardImage, 1));
                counter++;
            }

			using (var imageStream = new MemoryStream())
			{
				finalImage.SaveAsPng(imageStream);
				return imageStream.GetBuffer();
			}
        }

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _image.Dispose();
                }
                
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
