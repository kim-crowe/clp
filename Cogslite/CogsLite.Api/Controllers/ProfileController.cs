using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CogsLite.Api.Controllers
{    
    public class ProfileController : ControllerBase
    {
        [Authorize]
        [HttpGet("api/profile")]
        public string GetUserProfile()
        {
            return "Hello, world.";
        }
    }
}