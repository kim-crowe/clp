using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cogslite.DataModels;
using CogsLite.Core;
using GorgleDevs.Mvc;
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

		public async Task<IActionResult> OnGetList(Guid gameId)
		{
			if (SignedInUser == null)
				return new JsonResult(new DeckData[0]);

			var decks = await _deckStore.ByGameAndOwner(gameId, SignedInUser.Id);
			return new JsonResult(decks.Select(DeckData.FromDeck));
		}

		public async Task<IActionResult> OnGet(Guid deckId)
		{			
			var deck = await _deckStore.Get(SignedInUser.Id, deckId);
			return new JsonResult(DeckData.FromDeck(deck));
		}

		public async Task<IActionResult> OnPostDeck([FromBody]DeckData deck)
		{
			var theDeck = deck.ToDeck();
			theDeck.Owner = SignedInUser;
			await _deckStore.Save(theDeck);
			return new JsonResult(DeckData.FromDeck(theDeck));
		}		

		public async Task<IActionResult> OnGetSheet(Guid deckId)
		{		
			var deck = await _deckStore.Get(SignedInUser.Id, deckId);
			var cardCount = deck.Items.Sum(i => i.Amount);
			return new FileContentResult(ImageSlicer.Composite(DataImages(deck), cardCount), "image/png");
		}

		private IEnumerable<byte[]> DataImages(Deck deck)
		{
			foreach (var item in deck.Items)
			{
				for (int i = 0; i < item.Amount; i++)
				{
					yield return _imageStore.Get(item.CardId.ToShortGuid()).GetAwaiter().GetResult().Data;
				}
			}
		}
	}
}