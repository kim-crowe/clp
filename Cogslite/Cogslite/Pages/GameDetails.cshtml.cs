using System;
using System.IO;
using System.Threading.Tasks;
using CogsLite.Core;
using GorgleDevs.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace Cogslite.Pages
{
    public class GameDetailsModel : CogsPageModel
    {
        private readonly IGameStore _gameStore;
        private readonly IImageStore _imageStore;

        public GameDetailsModel(IGameStore gameStore, IImageStore imageStore)
        {
            _gameStore = gameStore ?? throw new ArgumentNullException(nameof(gameStore));
            _imageStore = imageStore ?? throw new ArgumentNullException(nameof(imageStore));
        }

        public void OnGet(string message, string name)
        {
            ViewData["Message"] = message;
            ViewData["Name"] = name;
        }

        public async Task<IActionResult> OnPostAsync(string name, IFormFile image)
        {
			Game newGame = new Game
			{
				Id = Guid.NewGuid(),
				Name = name,
				CreatedOn = DateTime.Now,
				Owner = SignedInUser,
				CardCount = 0
            };

            if (! await _gameStore.TryAdd(newGame))
            {
                return RedirectToAction("Join", new { message = "A game with this name already exists", name});
            }

            if (image != null)
            {
                using (var ms = new MemoryStream())
                {
                    image.CopyTo(ms);
                    var imageData = new ImageData
                    {
                        Id = newGame.Id.ToShortGuid(),
                        OriginalFileName = image.FileName,
                        Data = ms.GetBuffer()
                    };
                    await _imageStore.Add(imageData);
                }
            }
                
            return Redirect("Home");
        }
    }
}