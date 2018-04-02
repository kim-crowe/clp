using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CogsLite.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cogslite.Pages
{
    public class GameDetailsModel : CogsPageModel
    {
        private readonly IGameStore _gameStore;

        public GameDetailsModel(IGameStore gameStore)
        {
            _gameStore = gameStore ?? throw new ArgumentNullException(nameof(gameStore));
        }

        public void OnGet(string message, string name)
        {
            ViewData["Message"] = message;
            ViewData["Name"] = name;
        }

        public async Task<IActionResult> OnPostAsync(string name)
        {

            Game newGame = new Game
            {
                Name = name,
                CreatedOn = DateTime.Now,
                Owner = SignedInUser
            };

            if (!_gameStore.TryAdd(newGame))
            {
                return RedirectToAction("Join", new { message = "A game with this name already exists", name});
            }

            return Redirect("Home");
        }
    }
}