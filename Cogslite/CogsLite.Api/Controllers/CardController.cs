using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CogsLite.Api.Payloads;
using CogsLite.Core;
using GorgleDevs.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace CogsLite.Api
{    
    [ApiController]
    public class CardController : ControllerBase
    {
        private readonly ICardStore _cardStore;
        private readonly IImageStore _imageStore;
        private readonly IUserContext _userContext;

        public CardController(ICardStore cardStore, IImageStore imageStore, IUserContext userContext)
        {
            _cardStore = cardStore ?? throw new ArgumentNullException(nameof(cardStore));
            _imageStore = imageStore ?? throw new ArgumentNullException(nameof(imageStore));
            _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
        }

		[HttpPost("/api/card/{cardId}/update")]
		public async Task<IActionResult> Update(Guid cardId, CardUpdateRequest cardUpdate)
		{		
			await _cardStore.UpdateOne(cardId, c =>
			{
				c.Name = cardUpdate.Name;
				c.Type = cardUpdate.Type;
			});

			return new JsonResult(true);
		}

		[HttpPost("/api/games/{gameId}/cards")]
        public async Task<IActionResult> Search(Guid gameId, CardSearchRequest searchRequest)
        {
            var pageIndex = searchRequest.Page - 1;
			var cards = ( await _cardStore.Get(gameId)).ToList();

			if (!String.IsNullOrEmpty(searchRequest.CardType))
			{
				cards = cards.Where(c => !String.IsNullOrEmpty(c.Type) && c.Type == searchRequest.CardType).ToList();
			}

			if (!String.IsNullOrEmpty(searchRequest.CardName))
			{
				cards = cards.Where(c => !String.IsNullOrEmpty(c.Name) && c.Name.ToLower().Contains(searchRequest.CardName.ToLower())).ToList();
			}

			foreach (var tag in searchRequest.Tags)
			{
				cards = cards.Where(c => c.Tags != null &&  c.Tags.Any(t => t == tag)).ToList();
			}

			if(searchRequest.CardIds.Any())
			{
				var convertedCardIds = searchRequest.CardIds.Select(ShortGuid.Parse).Select(g => g.ToString());
				cards = cards.Where(c => convertedCardIds.Contains(c.Id.ToString())).ToList();
			}

			var pageCount = cards.Count / searchRequest.ItemsPerPage;

			if (cards.Count % searchRequest.ItemsPerPage > 0)
				pageCount++;

			var result = new
			{
				cards = cards.SelectPage(
                    searchRequest.Page - 1,
                    searchRequest.ItemsPerPage,
                    c => new {
                        Id = c.Id.ToShortGuid(),
                        c.Name,
                        c.Type,
                        imageUrl = c.ImageUrl}),
				numberOfPages = pageCount
			};

			return new JsonResult(result);
        }
    }
}