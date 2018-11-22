using System;
using System.Security.Claims;
using CogsLite.Core;
using Microsoft.AspNetCore.Http;

namespace CogsLite.Api
{
    public class ClaimsIdentityUserContext : IUserContext
    {
        private readonly IHttpContextAccessor _httpContextProvider;

        public ClaimsIdentityUserContext(IHttpContextAccessor httpContextProvider)
        {
            _httpContextProvider = httpContextProvider;
        }

        public Member SignedInUser  
        {
            get
            {
                var user = _httpContextProvider.HttpContext.User;
                var id = user.FindFirst(ClaimTypes.NameIdentifier).Value;
                var emailAddress = user.FindFirst(ClaimTypes.Email).Value;
                
                return new Member
                {
                    Id = Guid.Parse(id),
                    UserName = user.FindFirst("cognito:username").Value,
                    EmailAddress = emailAddress
                };
            }
        }
    }
}