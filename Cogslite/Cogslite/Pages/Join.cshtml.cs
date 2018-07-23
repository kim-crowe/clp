using System;
using CogsLite.Core;
using Microsoft.AspNetCore.Mvc;
using GorgleDevs.Mvc;

namespace Cogslite.Pages
{
    public class JoinModel : CogsPageModel
    {
        private readonly IUserStore _userStore;

        public JoinModel(IUserStore userStore)
        {
            _userStore = userStore ?? throw new ArgumentNullException(nameof(userStore));
        }

		public string MessageClass => String.IsNullOrEmpty((string)ViewData["Message"]) ? "collapse" : String.Empty;

        public void OnGet(string message, string emailAddress, string displayName)
        {
            ViewData["Message"] = message;
            ViewData["EmailAddress"] = emailAddress;
            ViewData["DisplayName"] = displayName;
        }

        public IActionResult OnPost(string emailAddress, string displayName, string password, string confirmPassword)
        {
			if (password != confirmPassword)
                return RedirectToAction("Join", new { message = "Password and confirmation password do not match", emailAddress, displayName});
			
			if(emailAddress.IsEmailAddress() == false)
				return RedirectToAction("Join", new { message = "Your email address doesn't look right", emailAddress, displayName });

			if(PasswordAnalyser.PasswordStrength(password) <12)
				return RedirectToAction("Join", new { message = "Your password is a bit weak, try adding capitals, numbers or special characters", emailAddress, displayName });

			try
            {
                _userStore.Add(new User
                {
                    EmailAddress = emailAddress,
                    Username = displayName,
                    Password = password
                });
            }
            catch(InvalidOperationException ex)
            {
                return RedirectToAction("Join", new { message = ex.Message.Replace(",", "<br>"), emailAddress, displayName });
            }
            
            return Redirect("SignIn");
        }
    }
}