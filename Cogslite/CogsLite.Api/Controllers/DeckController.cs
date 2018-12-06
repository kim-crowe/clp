using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CogsLite.Api.Payloads;
using CogsLite.Core;
using Microsoft.AspNetCore.Mvc;

namespace CogsLite.Api.Controllers
{
	[ApiController]
    public class DeckController : ControllerBase
    {
        private readonly IDeckStore _deckStore;
		private readonly IImageStore _imageStore;
        private readonly IUserContext _userContext;
        
        public DeckController(IDeckStore deckStore, IImageStore imageStore, IUserContext userContext)
        {
            _deckStore = deckStore ?? throw new ArgumentNullException(nameof(deckStore));
            _imageStore = imageStore ?? throw new ArgumentNullException(nameof(imageStore));
            _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
        }

        [HttpGet("/api/decks/{gameId}")]
        public async Task<IActionResult> OnGetList(Guid gameId)
		{
			var decks = await _deckStore.ByGameAndOwner(gameId, _userContext.SignedInUser.Id);
			return new JsonResult(decks.Select(DeckData.FromDeck));
		}

        [HttpGet("/api/deck/{deckId}")]
        public async Task<IActionResult> OnGet(Guid deckId)
		{			
			var deck = await _deckStore.Get(deckId);
			return new JsonResult(DeckData.FromDeck(deck));
		}

        [HttpPost("/api/deck")]
		public async Task<IActionResult> OnPostDeck(DeckData deck)
		{
			var theDeck = deck.ToDeck();
			theDeck.Owner = _userContext.SignedInUser;
			theDeck.Version++;
			await _deckStore.Save(theDeck);
			return new JsonResult(DeckData.FromDeck(theDeck));
		}		

        [HttpGet("/api/deck/{deckId}/sheet")]
		public async Task<IActionResult> OnGetSheet(Guid deckId)
		{	
			var deck = await _deckStore.Get(deckId);	
			var sheet = await _imageStore.Get("Deck", deck.Id, deck.Version);

			if(sheet == null)
			{
				var cardCount = deck.Items.Sum(i => i.Amount);
				var cardSheetData = ImageSlicer.Composite(DataImages(deck), cardCount);				
				await _imageStore.Add("Deck", deckId, deck.Version, "png", cardSheetData);
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
					yield return _imageStore.Get("Card",  item.CardId, 1).GetAwaiter().GetResult();
				}
			}
		}
    }
}