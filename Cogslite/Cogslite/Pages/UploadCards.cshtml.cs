using CogsLite.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;
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

        public IActionResult OnPostAsync(Guid gameId, int cardsPerRow, int cardCount, IFormFile cardSheet)
        {
            foreach (var imageData in ImageSlicer.Slice(cardsPerRow, cardCount, cardSheet.OpenReadStream()))
            {
                var card = new Card
                {
                    Id = Guid.NewGuid(),
                    Name = String.Empty,
                    GameId = gameId,
                    CreatedOn = DateTime.Now
                };

                _cardStore.Add(card);
                _imageStore.Add(new ImageData { Id = card.Id, Data = imageData, OriginalFileName = String.Empty });
            }

            //_gameStore.UpdateOne(gameId, g => g.CardCount += cardCount);
            return RedirectToPage("/Home");
        }        
    }
}