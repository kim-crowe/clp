using System;
using System.Linq;
using System.Threading.Tasks;
using CogsLite.Api.Payloads;
using CogsLite.Core;
using Microsoft.AspNetCore.Mvc;

namespace CogsLite.Api.Controllers
{
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
    }
}