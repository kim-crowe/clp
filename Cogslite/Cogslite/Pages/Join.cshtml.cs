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

        public void OnGet(string message, string emailAddress, string displayName)
        {
            ViewData["Message"] = message;
            ViewData["EmailAddress"] = emailAddress;
            ViewData["DisplayName"] = displayName;
        }

        public async Task<IActionResult> OnPostAsync(string emailAddress, string displayName, string password, string confirmPassword)
        {
            if (password != confirmPassword)
                return RedirectToAction("Join", new { message = "Password and confirmation password do not match", emailAddress, displayName});

            try
            {
                _userStore.Add(new User
                {
                    EmailAddress = emailAddress,
                    DisplayName = displayName,
                    Password = password
                });
            }
            catch(InvalidOperationException ex)
            {
                return RedirectToAction("Join", new { message = ex.Message, emailAddress, displayName });
            }
            
            return Redirect("SignIn");
        }
    }
}