using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using CogsLite.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GorgleDevs.Mvc;


namespace Cogslite.Pages
{
    public class UploadCardsModel : CogsPageModel
    {
        private readonly ICardStore _cardStore;
        private readonly IGameStore _gameStore;
        private readonly IImageStore _imageStore;
        private Game _game;

        public UploadCardsModel(ICardStore cardStore, IGameStore gameStore, IImageStore imageStore)
        {
            _cardStore = cardStore ?? throw new ArgumentNullException(nameof(cardStore));
            _gameStore = gameStore ?? throw new ArgumentNullException(nameof(gameStore));
            _imageStore = imageStore ?? throw new ArgumentNullException(nameof(imageStore));
        }

        public Game Game => _game;

        public async Task OnGet(Guid id)
        {
            _game = (await _gameStore.Get()).SingleOrDefault(g => g.Id == id);
        }

        public async Task<IActionResult> OnPostAsync(Guid ownerId, Guid gameId, int cardWidth, int cardHeight, IFormFile cardData, IFormFile images)
        {
            if(cardData == null)
                throw new ArgumentNullException(nameof(cardData));

			var cards = new List<Card>();
            			
            using (var reader = new System.IO.StreamReader(cardData.OpenReadStream()))
            {
                var data = new DataTable().PopulateFromTsv(reader);
                foreach(var row in data.Rows.Cast<DataRow>())
                {
                    var card = new Card
                    {
                        Id = Guid.NewGuid(),
                        GameId = gameId,
                        Name = row["Name"].ToString(),
                        Type = row["Type"].ToString(),
                        Tags = row["Tags"].ToString().Split(","),
                        CreatedOn = DateTime.UtcNow
                    };
                    cards.Add(card);
                }
            }

            if(images != null)
            {
                using(var imageSlicer = new ImageSlicer(cardWidth, cardHeight, images.OpenReadStream()))
                {
                    int cardIndex = 0;
                    foreach(var image in imageSlicer.Images)
                    {
                        if(cardIndex >= cards.Count)
                            break;

                        var card = cards[cardIndex];
                        card.ImageUrl = await _imageStore.Add("Card", card.Id, "png", image);
                        cardIndex++;
                    }
                }
            }

            foreach(var card in cards)
            {
                await _cardStore.Add(card);
            }

            await UpdateGameData(ownerId, gameId);

            return RedirectToPage("/Cards", new { ownerId, gameId });
            
        }

		private async Task UpdateGameData(Guid ownerId, Guid gameId)
		{
			var allCards = await _cardStore.Get(gameId);
			await _gameStore.UpdateOne(gameId, g =>
			{
				g.CardCount = allCards.Count();
			});
		}
    }
}
