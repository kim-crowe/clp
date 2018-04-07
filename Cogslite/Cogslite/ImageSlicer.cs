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

        public static void Composite()
        {
            var finalImage = new Image<Rgba32>(300, 249);

            var files = Directory.GetFiles(@"c:\utilities\split\");
            var counter = 0;

            foreach (var file in files)
            {
                var cardImage = Image.Load(file);
                var row = counter / 5;
                var column = counter % 5;

                finalImage.Mutate(x => x.DrawImage(cardImage, new Size(cardImage.Width, cardImage.Height), new Point(column * cardImage.Width, row * cardImage.Height), GraphicsOptions.Default));
                counter++;
            }

            using (var fileStream = File.OpenWrite(@"c:\utilities\split\comp.png"))
                finalImage.SaveAsPng(fileStream);
        }
    }
}
