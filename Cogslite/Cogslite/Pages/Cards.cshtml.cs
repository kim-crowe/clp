using System;
using System.Collections.Generic;
using System.Linq;
using Cogslite.DataModels;
using CogsLite.Core;
using Microsoft.AspNetCore.Mvc;

namespace Cogslite.Pages
{
    public class CardsPageModel : CogsPageModel
    {
        private readonly IGameStore _gameStore;
        private readonly ICardStore _cardStore;        
		private readonly IImageStore _imageStore;
        
        private Game _game;
		private List<string> _tags;
		private bool _isGameOwner;

        public CardsPageModel(IGameStore gameStore, ICardStore cardStore, IImageStore imageStore)
        {
            _gameStore = gameStore ?? throw new ArgumentNullException(nameof(gameStore));
            _cardStore = cardStore ?? throw new ArgumentNullException(nameof(cardStore));            
			_imageStore = imageStore ?? throw new ArgumentNullException(nameof(imageStore));
        }

        public Game Game => _game;

		public IEnumerable<string> Tags => _tags;

		public bool IsGameOwner => _isGameOwner;

        public void OnGet(Guid gameId)
        {
            _game = _gameStore.GetSingle(gameId);
			var cards = _cardStore.Get(gameId);
			_tags = cards.Where(c => c.Tags != null).SelectMany(c => c.Tags).Distinct().ToList();
			_isGameOwner = IsSignedIn && _game.Owner.Id == SignedInUser.Id;
        }		

		public IActionResult OnPostCardSearch(Guid gameId, [FromBody] CardSearch cardSearch)
		{
			var pageIndex = cardSearch.Page - 1;
			_game = _gameStore.GetSingle(gameId);
			var cards = _cardStore.Get(gameId).ToList();

			if (cardSearch.CardType > -1)
			{
				cards = cards.Where(c => !String.IsNullOrEmpty(c.Type) && c.Type == _game.CardTypes[cardSearch.CardType]).ToList();
			}

			if (!String.IsNullOrEmpty(cardSearch.CardName))
			{
				cards = cards.Where(c => !String.IsNullOrEmpty(c.Name) && c.Name.ToLower().Contains(cardSearch.CardName.ToLower())).ToList();
			}

			foreach (var tag in cardSearch.Tags)
			{
				cards = cards.Where(c => c.Tags != null &&  c.Tags.Any(t => t == tag)).ToList();
			}

			var pageCount = cards.Count / cardSearch.ItemsPerPage;

			if (cards.Count % cardSearch.ItemsPerPage > 0)
				pageCount++;

			var result = new
			{
				cards = cards.Skip(pageIndex * cardSearch.ItemsPerPage).Take(cardSearch.ItemsPerPage),
				numberOfPages = pageCount
			};

			return new JsonResult(result);
		}

		public IActionResult OnPostCardUpdate([FromBody] CardUpdate cardUpdate)
		{
			_cardStore.UpdateOne(cardUpdate.CardId, c =>
			{
				c.Name = cardUpdate.Name;
				c.Type = cardUpdate.Type;
				c.Tags = cardUpdate.Tags.Split(',');
			});

			return new JsonResult(true);
		}        
    }	
}