using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CogsLite.Api.Controllers
{    
    public class ProfileController : ControllerBase
    {        
        private readonly IUserContext _userContext;

        public ProfileController(IUserContext userContext)
        {
            _userContext = userContext;
        }

        [Authorize]
        [HttpGet("api/profile")]
        public IActionResult GetUserProfile()
        {
            return new JsonResult(_userContext.SignedInUser);
        }
    }
}