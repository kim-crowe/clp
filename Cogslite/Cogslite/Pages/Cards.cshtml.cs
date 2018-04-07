using System;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using CogsLite.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace Cogslite.Pages
{
    public class CardsPageModel : CogsPageModel
    {
        private readonly IGameStore _gameStore;
        private Game _game;

        public CardsPageModel(IGameStore gameStore)
        {
            _gameStore = gameStore ?? throw new ArgumentNullException(nameof(gameStore));
        }

        public Game Game => _game;

        public void OnGet(Guid gameId)
        {
            _game = _gameStore.GetSingle(gameId);            
        }

        public JsonResult OnGetCards(Guid gameId)
        {
            var cards = new []
            {
                new { id = Guid.NewGuid(),  name = "Alpha Base", },
                new { id = Guid.NewGuid(),  name = "Beta Gun", },
                new { id = Guid.NewGuid(),  name = "Charlie", },
                new { id = Guid.NewGuid(),  name = "Demon Upper", },
                new { id = Guid.NewGuid(),  name = "Edge of the galaxy", },
                new { id = Guid.NewGuid(),  name = "Fist of fury", },
                new { id = Guid.NewGuid(),  name = "Gorgebear", },
                new { id = Guid.NewGuid(),  name = "Halt!", },
                new { id = Guid.NewGuid(),  name = "Iguana", }                
            };

            return new JsonResult(cards);
        }

        public JsonResult OnPostDeck(Json.Deck deck)
        {
            // TODO: Save the deck
            return new JsonResult(deck.name);
        }
    }
}