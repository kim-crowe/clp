using System;
using System.Collections.Generic;
using System.Linq;
using CogsLite.Core;
using Microsoft.AspNetCore.Mvc;


namespace Cogslite.Pages
{
    public class CardsPageModel : CogsPageModel
    {
        private readonly IGameStore _gameStore;
        private readonly ICardStore _cardStore;
        private readonly IDeckStore _deckStore;
		private readonly IImageStore _imageStore;
        
        private Game _game;

        public CardsPageModel(IGameStore gameStore, ICardStore cardStore, IDeckStore deckStore, IImageStore imageStore)
        {
            _gameStore = gameStore ?? throw new ArgumentNullException(nameof(gameStore));
            _cardStore = cardStore ?? throw new ArgumentNullException(nameof(cardStore));
            _deckStore = deckStore ?? throw new ArgumentNullException(nameof(deckStore));
			_imageStore = imageStore ?? throw new ArgumentNullException(nameof(imageStore));
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

		public IActionResult OnGetDeck(Guid deckId)
		{
			var deck = _deckStore.Get(deckId);
			var cardCount = deck.Items.Sum(i => i.Count);
			return new FileContentResult(ImageSlicer.Composite(DataImages(deck), cardCount), "image/png");
		}

		private IEnumerable<byte[]> DataImages(Deck deck)
		{
			foreach(var item in deck.Items)
			{				
				for(int i = 0; i < item.Count; i++)
				{
					yield return _imageStore.Get(item.Card.Id).Data;
				}
			}
		}

        public JsonResult OnPostDeck([FromBody]DeckData deck)
        {
			var realDeck = deck.ToDeck();
			_deckStore.Save(realDeck);
			return new JsonResult(Url.Page("/Cards", "Deck", new { deckId = realDeck.Id }));
        }
    }

	public class DeckData
	{
		public string id { get; set; }
		public string name { get; set; }
		public DeckItemData[] items { get; set; }

		public Deck ToDeck()
		{
			return new Deck
			{
				Id = ParseOrCreateGuid(id),
				Name = name,
				Items = items.Select(i => new DeckItem
				{
					Card = new Card
					{
						Name = i.card.name,
						Id = ParseOrCreateGuid(i.card.id)
					},
					Count = i.count
				}).ToArray()
			};
		}

		private Guid ParseOrCreateGuid(string id)
		{
			return String.IsNullOrEmpty(id) ? Guid.NewGuid() : Guid.Parse(id);
		}
	}

	public class DeckItemData
	{
		public CardData card { get; set; }
		public int count { get; set; }
	}

	public class CardData
	{
		public string id { get; set; }
		public string name { get; set; }		
	}	
}