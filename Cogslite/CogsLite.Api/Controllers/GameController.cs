using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CogsLite.Api.Payloads;
using CogsLite.Core;
using GorgleDevs.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CogsLite.Api.Controllers
{
    [Route("/api/games")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IGameStore _gameStore;
        private readonly IImageStore _imageStore;
        private readonly IUserContext _userContext;

        public GameController(IGameStore gameStore, IImageStore imageStore, IUserContext userContext)
        {
            _gameStore = gameStore ?? throw new ArgumentNullException(nameof(gameStore));
            _imageStore = imageStore ?? throw new ArgumentNullException(nameof(imageStore));
            _userContext = userContext;            
        }        

        [HttpPost("search")]
        public async Task<IActionResult> Search(GameSearchRequest searchRequest)
        {
            var games = (await _gameStore.Get()).Where(g => g.Name.ToLower().Contains(searchRequest.SearchText.ToLower())).Select(g => new
			{
				id = g.Id.ToShortGuid(),
				ownerId = g.Owner.Id.ToShortGuid(),
				name = g.Name,
				userName = g.Owner.Username,
				createdOn = g.CreatedOn.ToString("yyyy-MM-dd"),
				cardCount = g.CardCount,
				imageUrl = g.ImageUrl
			}).ToList();

			var pageIndex = searchRequest.Page - 1;
			var pageCount = games.Count() / searchRequest.ItemsPerPage;

			if (games.Count() % searchRequest.ItemsPerPage > 0)
				pageCount++;

			var result = new
			{
				games = games.Skip(pageIndex * searchRequest.ItemsPerPage).Take(searchRequest.ItemsPerPage),
				numberOfPages = pageCount
			};

			return new JsonResult(result);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(string name, IFormFile image)
        {
            Game newGame = new Game
			{
				Id = Guid.NewGuid(),
				Name = name,
				CreatedOn = DateTime.Now,
				Owner = _userContext.SignedInUser,
				CardCount = 0
            };

            if (image != null)
            {
                using (var ms = new MemoryStream())
                {
                    image.CopyTo(ms);                    
                    string imageType = Path.GetExtension(image.FileName);
                    newGame.ImageUrl = await _imageStore.Add("Game", newGame.Id, imageType, ms.GetBuffer());
                }
            }

            await _gameStore.Add(newGame);
                
            return Ok();
        }       
    }
}
