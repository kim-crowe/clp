using CogsLite.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

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

        public void OnGet(Guid id)
        {
            _game = _gameStore.Get().SingleOrDefault(g => g.Id == id);
        }

        public IActionResult OnPostAsync(Guid gameId, int cardsPerRow, int cardCount, IFormFile cardSheet, IFormFile infoSheet)
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
                _game = _gameStore.Get().SingleOrDefault(g => g.Id == gameId);

                if(_game.CardSize == null)
                {
                    _game.CardSize = imageSlicer.CardSize;
                    _gameStore.UpdateOne(_game.Id, g => g.CardSize = imageSlicer.CardSize);
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

                    _cardStore.Add(card);
                    _imageStore.Add(new ImageData { Id = card.Id, Data = imageData, OriginalFileName = String.Empty });
					index++;
                }

				UpdateGameData(gameId);

                return RedirectToPage("/Cards", new { gameId });
            }
        }

		private void UpdateGameData(Guid gameId)
		{
			var allCards = _cardStore.Get(gameId);
			_gameStore.UpdateOne(gameId, g =>
			{
				g.CardTypes = allCards.Select(c => c.Type).Distinct().ToArray();
				g.CardCount = allCards.Count();
			});
		}
    }
}
