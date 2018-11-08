using System;
using System.Linq;
using System.Threading.Tasks;
using CogsLite.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CogsLite.Api
{
    public class CardUploadController : ControllerBase
    {
        private readonly ICardStore _cardStore;
        private readonly IImageStore _imageStore;
        private readonly IUserContext _userContext;
        private readonly IGameStore _gameStore;

        public CardUploadController(ICardStore cardStore, IGameStore gameStore, IImageStore imageStore, IUserContext userContext)
        {
            _cardStore = cardStore ?? throw new ArgumentNullException(nameof(cardStore));
            _imageStore = imageStore ?? throw new ArgumentNullException(nameof(imageStore));
            _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
            _gameStore = gameStore ?? throw new ArgumentNullException(nameof(gameStore));
        }

        public async Task<IActionResult> UploadCards(Guid gameId, int cardsPerRow, int cardCount, IFormFile cardSheet)
        {
			var cards = new Card[] { };					

            using (var imageSlicer = new ImageSlicer(cardsPerRow, cardCount, cardSheet.OpenReadStream()))
            {
                var game = ( await _gameStore.GetSingle(gameId));

                if(game.CardSize == null)
                {
                    game.CardSize = imageSlicer.CardSize;
                    await _gameStore.UpdateOne(gameId, g => g.CardSize = imageSlicer.CardSize);
                }
                else if (game.CardSize != imageSlicer.CardSize)
                {
                    throw new System.InvalidOperationException("Card size error");
                }				

				var index = 0;
				foreach (var imageData in imageSlicer.Slices)
                {                    
                 	var card = cards.Length > index ? cards[index] : new Card();
					card.Id = Guid.NewGuid();
					card.GameId = gameId;
					card.CreatedOn = DateTime.Now;					
                    card.ImageUrl = await _imageStore.Add("Card", card.Id, "png", imageData);                    
                    await _cardStore.Add(card);                    
					index++;
                }

				await UpdateGameData(gameId);

                return Ok();
            }
        }

		private async Task UpdateGameData(Guid gameId)
		{
			var allCards = await _cardStore.Get(gameId);
			await _gameStore.UpdateOne(gameId, g =>
			{
				g.CardCount = allCards.Count();
			});
		}
    }
}