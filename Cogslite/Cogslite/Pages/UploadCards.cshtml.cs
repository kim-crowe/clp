using CogsLite.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<IActionResult> OnPostAsync(Guid gameId, int cardsPerRow, int cardCount, IFormFile cardSheet, IFormFile infoSheet)
        {
			var cards = new Card[] { };
			if(infoSheet != null)
			{
				using (var reader = new System.IO.StreamReader(infoSheet.OpenReadStream()))
				{
					cards = CardList.FromString(reader.ReadToEnd()).ToArray();
				}
			}			

            using (var imageSlicer = new ImageSlicer(cardsPerRow, cardCount, cardSheet.OpenReadStream()))
            {
                _game = ( await _gameStore.Get()).SingleOrDefault(g => g.Id == gameId);

                if(_game.CardSize == null)
                {
                    _game.CardSize = imageSlicer.CardSize;
                    await _gameStore.UpdateOne(_game.Id, g => g.CardSize = imageSlicer.CardSize);
                }
                else if(_game.CardSize != imageSlicer.CardSize)
                {
                    // Problem, card sizes are different
                    return RedirectToAction("UploadCards");
                }				

				var index = 0;
				foreach (var imageData in imageSlicer.Slices)
                {
					var card = cards.Length > index ? cards[index] : new Card();
					card.Id = Guid.NewGuid();
					card.GameId = gameId;
					card.CreatedOn = DateTime.Now;					

                    await _cardStore.Add(card);
                    await _imageStore.Add(new ImageData { Id = card.Id, Data = imageData, OriginalFileName = String.Empty });
					index++;
                }

				await UpdateGameData(gameId);

                return RedirectToPage("/Cards", new { gameId });
            }
        }

		private async Task UpdateGameData(Guid gameId)
		{
			var allCards = await _cardStore.Get(gameId);
			await _gameStore.UpdateOne(gameId, g =>
			{
				g.CardTypes = allCards.Select(c => c.Type).Distinct().ToArray();
				g.CardCount = allCards.Count();
			});
		}
    }
}
