using System.Linq;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication;

namespace Cogslite
{
    public abstract class CogsPageModel : PageModel
    {
        public bool IsSignedIn => HttpContext.User.Claims.Any(c => c.Type == "CogsMember" && c.Value == "Yes");
    }
}
