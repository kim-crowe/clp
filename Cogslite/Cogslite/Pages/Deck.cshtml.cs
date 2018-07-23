using System;
using System.Collections.Generic;
using System.Linq;
using Cogslite.DataModels;
using CogsLite.Core;
using Microsoft.AspNetCore.Mvc;

namespace Cogslite.Pages
{
    public class DeckPageModel : CogsPageModel
	{
		private readonly IDeckStore _deckStore;
		private readonly IImageStore _imageStore;

		public DeckPageModel(IDeckStore deckStore, IImageStore imageStore)
		{
			_deckStore = deckStore ?? throw new ArgumentNullException(nameof(deckStore));
			_imageStore = imageStore ?? throw new ArgumentNullException(nameof(imageStore));
		}

		public IActionResult OnGetList(Guid gameId)
		{
			if (SignedInUser == null)
				return new JsonResult(new DeckData[0]);

			return new JsonResult(_deckStore.ByGameAndOwner(gameId, SignedInUser.Id).Select(DeckData.FromDeck));							
		}

		public IActionResult OnGet(Guid deckId)
		{
			return new JsonResult(DeckData.FromDeck(_deckStore.Get(deckId)));
		}

		public IActionResult OnPostDeck([FromBody]DeckData deck)
		{
			var theDeck = deck.ToDeck();
			theDeck.Owner = SignedInUser;
			_deckStore.Save(theDeck);
			return new JsonResult(DeckData.FromDeck(theDeck));
		}		

		public IActionResult OnGetSheet(Guid deckId)
		{		
			var deck = _deckStore.Get(deckId);
			var cardCount = deck.Items.Sum(i => i.Amount);
			return new FileContentResult(ImageSlicer.Composite(DataImages(deck), cardCount), "image/png");
		}

		private IEnumerable<byte[]> DataImages(Deck deck)
		{
			foreach (var item in deck.Items)
			{
				for (int i = 0; i < item.Amount; i++)
				{
					yield return _imageStore.Get(item.CardId).GetAwaiter().GetResult().Data;
				}
			}
		}
	}
}