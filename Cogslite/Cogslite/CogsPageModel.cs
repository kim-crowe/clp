using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CogsLite.Core;

namespace Cogslite
{
    public abstract class CogsPageModel : PageModel
    {
        public bool IsSignedIn => HttpContext.User.Claims.Any(c => c.Type == "CogsMember" && c.Value == "Yes");

        public Member SignedInUser
        {
            get
            {
                if (!IsSignedIn)
                    return null;

                return new Member
                {
                    Id = Guid.Parse(HttpContext.User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value),
                    EmailAddress = HttpContext.User.Claims.Single(c => c.Type == ClaimTypes.Email).Value,
                    DisplayName = HttpContext.User.Claims.Single(c => c.Type == ClaimTypes.Name).Value
                };
            }
        }
    }
}
