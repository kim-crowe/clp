using System;
using System.Linq;
using Cogslite.DataModels;
using CogsLite.Core;
using Microsoft.AspNetCore.Mvc;

namespace Cogslite.Pages
{
    public class HomeModel : CogsPageModel
    {
        private readonly IGameStore _gameStore;

        public HomeModel(IGameStore gameStore)
        {
            _gameStore = gameStore ?? throw new ArgumentNullException(nameof(gameStore));
        }        
        			
        public void OnGet()
        {
            
		}

		public IActionResult OnPostSearch([FromBody]GameSearch searchData)
		{
			var games = _gameStore.Get().Where(g => g.Name.ToLower().Contains(searchData.SearchText.ToLower())).Select(g => new
			{
				id = g.Id,
				name = g.Name,
				userName = g.Owner.DisplayName,
				createdOn = g.CreatedOn.ToString("yyyy-MM-dd"),
				cardCount = g.CardCount
			}).ToList();

			var pageIndex = searchData.Page - 1;
			var pageCount = games.Count / searchData.ItemsPerPage;

			if (games.Count % searchData.ItemsPerPage > 0)
				pageCount++;

			var result = new
			{
				games = games.Skip(pageIndex * searchData.ItemsPerPage).Take(searchData.ItemsPerPage),
				numberOfPages = pageCount
			};

			return new JsonResult(result);
		}
    }
}
