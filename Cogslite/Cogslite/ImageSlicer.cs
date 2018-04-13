using System.IO;
using SixLabors.Primitives;
using SixLabors.ImageSharp;
using System.Collections.Generic;
using System;

namespace Cogslite
{
    public class ImageSlicer : IDisposable
    {
        private readonly int _cardCount;
        private readonly int _rows;
        private readonly int _cardsPerRow;
        private readonly System.Drawing.Size _cardSize;
        private readonly Image<Rgba32> _image;

        public ImageSlicer(int cardsPerRow, int cardCount, Stream imageData)
        {
            _cardCount = cardCount;
            _cardsPerRow = cardsPerRow;

            _rows = (cardCount / cardsPerRow);
            if (cardCount % cardsPerRow > 0)
                _rows++;

            _image = Image.Load(imageData);

            _cardSize = new System.Drawing.Size(_image.Width / cardsPerRow, _image.Height / _rows);
        }

        public System.Drawing.Size CardSize => _cardSize;

        public IEnumerable<byte[]> Slices
        {
            get
            {
                for (int i = 0; i < _cardCount; i++)
                {
                    int row = i / _cardsPerRow;
                    int column = i % _cardsPerRow;

                    var splitImage = _image.Clone(x => x.Crop(new Rectangle(column * _cardSize.Width, row * _cardSize.Height, _cardSize.Width, _cardSize.Height)));

                    using (var memoryStream = new MemoryStream())
                    {
                        splitImage.SaveAsPng(memoryStream);
                        yield return memoryStream.GetBuffer();
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

                finalImage.Mutate(x => x.DrawImage(cardImage, new Size(cardImage.Width, cardImage.Height), new Point(column * cardImage.Width, row * cardImage.Height), GraphicsOptions.Default));
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
