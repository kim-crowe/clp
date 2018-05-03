using System;
using System.Linq;
using Cogslite.DataModels;
using CogsLite.Core;
using Microsoft.AspNetCore.Mvc;

namespace Cogslite.Pages
{
    public class DeckPageModel : CogsPageModel
	{
		private readonly IDeckStore _deckStore;

		public DeckPageModel(IDeckStore deckStore)
		{
			_deckStore = deckStore ?? throw new ArgumentNullException(nameof(deckStore));
		}

		public IActionResult OnGetList(Guid gameId)
		{
			if (SignedInUser == null)
				return new JsonResult(new DeckData[0]);

			return new JsonResult(_deckStore.ByGameAndOwner(gameId, SignedInUser.Id).Select(DeckData.FromDeck));							
		}

		public IActionResult OnPostDeck([FromBody]DeckData deckData)
		{
			var deck = deckData.ToDeck();
			deck.Owner = SignedInUser;
			_deckStore.Save(deck);
			return new JsonResult(deck.Id);
		}
	}
}