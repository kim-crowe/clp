using System;
using System.Collections.Generic;
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
        private readonly ICardStore _cardStore;
        private Game _game;

        public CardsPageModel(IGameStore gameStore, ICardStore cardStore)
        {
            _gameStore = gameStore ?? throw new ArgumentNullException(nameof(gameStore));
            _cardStore = cardStore ?? throw new ArgumentNullException(nameof(cardStore));
        }

        public Game Game => _game;

        public void OnGet(Guid gameId)
        {
            _game = _gameStore.GetSingle(gameId);            
        }

        public JsonResult OnGetCards(Guid gameId)
        {
            _game = _gameStore.GetSingle(gameId);
            var cards = _cardStore.Get(gameId);
            return new JsonResult(cards);
        }

        public JsonResult OnPostDeck(Json.Deck deck)
        {
            // TODO: Save the deck
            return new JsonResult(deck.name);
        }
    }
}