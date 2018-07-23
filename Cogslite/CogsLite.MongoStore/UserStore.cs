using CogsLite.Core;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CogsLite.MongoStore
{
    public class UserStore : BaseMongoStore<User>, IUserStore
    {
        public UserStore(IConfiguration configuration) : base("Users", configuration)
        {
        }

        public async Task Add(User user)
        {
            var errors = new List<string>();

			if (user.EmailAddress.IsEmailAddress() == false)
				errors.Add("Email address doesn't look right");

            if(await EmailAddressExistsInStore(user))
                errors.Add("Email Address is already registerd");

            if (await UsernameExistsInStore(user))
                errors.Add("User name is already in use");

            if(errors.Any())
                throw new InvalidOperationException(String.Join(",", errors));
            
            user.Id = Guid.NewGuid();
            Insert(user);
        }

        private async Task<bool> EmailAddressExistsInStore(User user)
        {
            return (await FindWhere(usr => usr.EmailAddress == user.EmailAddress)).Any();
        }

        private async Task<bool> UsernameExistsInStore(User user)
        {
            return (await FindWhere(usr => usr.Username == user.Username)).Any();
        }        

        public async Task<Member> GetByEmailAddress(string emailAddress)
        {
            return ( await FindWhere(usr => usr.EmailAddress == emailAddress)).SingleOrDefault();            
        }

        public async Task<Member> GetByUsername(string username)
        {
            return ( await FindWhere(usr => usr.Username == username)).SingleOrDefault();
        }

        public async Task<Member> SignIn(string emailAddress, string password)
        {
            var member = (await FindWhere(usr => usr.EmailAddress == emailAddress)).SingleOrDefault();
            if (member?.Password == password)
                return member;
            return null;
        }
    }
}
