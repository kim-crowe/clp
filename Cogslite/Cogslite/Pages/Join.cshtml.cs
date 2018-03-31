using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CogsLite.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cogslite.Pages
{
    public class JoinModel : CogsPageModel
    {
        private readonly IUserStore _userStore;

        public JoinModel(IUserStore userStore)
        {
            _userStore = userStore ?? throw new ArgumentNullException(nameof(userStore));
        }

        public void OnGet(string message, string username)
        {
            ViewData["Message"] = message;
            ViewData["UserName"] = username;
        }

        public async Task<IActionResult> OnPostAsync(string username, string password, string confirmPassword)
        {
            var existingUser = _userStore.Get(username);
            if (existingUser != null)
                return RedirectToAction("Join", new { message = "A user with this username already exists", username });

            if (password != confirmPassword)
                return RedirectToAction("Join", new { message = "Password and confirmation password do not match", username});


            User newUser = new User
            {
                Username = username,
                Password = password
            };

            if (!_userStore.TryAdd(newUser))
            {
                return RedirectToAction("Join", new { message = "A user with this username already exists", username });
            }
            
            return Redirect("SignIn");
        }
    }
}