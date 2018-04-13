using CogsLite.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Drawing;

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
            using (var imageSlicer = new ImageSlicer(cardsPerRow, cardCount, cardSheet.OpenReadStream()))
            {
                _game = _gameStore.Get().SingleOrDefault(g => g.Id == gameId);
                if(_game.CardSize == Size.Empty)
                {
                    _game.CardSize = imageSlicer.CardSize;
                    _gameStore.UpdateOne(_game.Id, g => g.CardSize = imageSlicer.CardSize);
                }
                else if(_game.CardSize != imageSlicer.CardSize)
                {
                    // Problem, card sizes are different
                    return RedirectToAction("UploadCards");
                }
                
                foreach (var imageData in imageSlicer.Slices)
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

                return RedirectToPage("/Cards", new { gameId = gameId });
            }
        }        
    }
}
