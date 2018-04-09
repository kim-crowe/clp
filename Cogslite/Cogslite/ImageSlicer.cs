using System.IO;
using SixLabors.Primitives;
using SixLabors.ImageSharp;
using System.Collections.Generic;

namespace Cogslite
{
    public static class ImageSlicer
    {
        public static IEnumerable<byte[]> Slice(int cardsPerRow, int cardCount, Stream imageData)
        {
            int rows = (cardCount / cardsPerRow);
            if (cardCount % cardsPerRow > 0)
                rows++;

            var image = Image.Load(imageData);
            //if (image.Width % cardsPerRow > 0 || image.Height % rows > 0)
            //	throw new InvalidOperationException("Cannot split image into equal parts based on rows and columns supplied");

            var splitWidth = image.Width / cardsPerRow;
            var splitHeight = image.Height / rows;

            for (int i = 0; i < cardCount; i++)
            {
                int row = i / cardsPerRow;
                int column = i % cardsPerRow;

                var splitImage = image.Clone(x => x.Crop(new Rectangle(column * splitWidth, row * splitHeight, splitWidth, splitHeight)));

                using (var memoryStream = new MemoryStream())
                {
                    splitImage.SaveAsPng(memoryStream);
                    yield return memoryStream.GetBuffer();
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
    }
}
