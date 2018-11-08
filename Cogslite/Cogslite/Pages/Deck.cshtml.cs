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
			var deck = await _deckStore.Get(deckId);
			return new JsonResult(DeckData.FromDeck(deck));
		}

		public async Task<IActionResult> OnPostDeck([FromBody]DeckData deck)
		{
			var theDeck = deck.ToDeck();
			theDeck.Owner = SignedInUser;
			theDeck.Version++;
			await _deckStore.Save(theDeck);
			return new JsonResult(DeckData.FromDeck(theDeck));
		}		

		public async Task<IActionResult> OnGetSheet(Guid deckId)
		{	
			var deck = await _deckStore.Get(deckId);	
			var sheet = await _imageStore.Get("Deck", deckId);

			if(sheet == null)
			{
				var cardCount = deck.Items.Sum(i => i.Amount);
				var cardSheetData = ImageSlicer.Composite(DataImages(deck), cardCount);				
				await _imageStore.Add("Deck", deckId, "png", cardSheetData);
				return new FileContentResult(cardSheetData, "image/png");
			}
			
			return new FileContentResult(sheet, "image/png");
		}

		private IEnumerable<byte[]> DataImages(Deck deck)
		{
			foreach (var item in deck.Items)
			{
				for (int i = 0; i < item.Amount; i++)
				{
					yield return _imageStore.Get("Card",  item.CardId).GetAwaiter().GetResult();
				}
			}
		}
	}
}