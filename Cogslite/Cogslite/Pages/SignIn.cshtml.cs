using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CogsLite.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cogslite.Pages
{
    public class SignInModel : CogsPageModel
    {
        private readonly IUserStore _userStore;

        public SignInModel(IUserStore userStore)
        {
            _userStore = userStore ?? throw new ArgumentNullException(nameof(userStore));
        }

		public string MessageClass => String.IsNullOrEmpty((string)ViewData["Message"]) ? "collapse" : String.Empty;

		public void OnGet(string message)
        {
            ViewData["Message"] = message;
        }

        public async Task<IActionResult> OnPostAsync(string emailAddress, string password)
        {
            var member = await _userStore.SignIn(emailAddress, password);

            if (member != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, emailAddress),
                    new Claim("CogsMember", "Yes"),
                    new Claim(ClaimTypes.NameIdentifier, member.Id.ToString()),
                    new Claim(ClaimTypes.Name, member.Username)
                };

                var claimsIdentity = new ClaimsIdentity(claims, "login");
                var claimPrincipal = new ClaimsPrincipal(claimsIdentity);
                await HttpContext.SignInAsync(claimPrincipal);
                return Redirect("Home");
            }
            else
                return RedirectToAction("SignIn", new { message = "Invalid email address or password was entered" });
        }
    }
}