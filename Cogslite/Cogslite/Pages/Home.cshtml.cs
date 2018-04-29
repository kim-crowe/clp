using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CogsLite.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cogslite.Pages
{
    public class HomeModel : CogsPageModel
    {
        private readonly IGameStore _gameStore;
        private List<Game> _games;

        public HomeModel(IGameStore gameStore)
        {
            _gameStore = gameStore ?? throw new ArgumentNullException(nameof(gameStore));
        }

        public IEnumerable<Game> Games // => _games;
        {
            get
            {
                return _games;
            }
        }

        public void OnGet()
        {
            _games = _gameStore.Get().ToList();
			_games.AddRange(_games);
			_games.AddRange(_games);
			_games.AddRange(_games);

		}
    }
}
