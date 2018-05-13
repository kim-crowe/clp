using CogsLite.Core;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CogsLite.MongoStore
{
    public class UserStore : BaseMongoStore<User>, IUserStore
    {
        public UserStore(IConfiguration configuration) : base("Users", configuration)
        {
        }

        public void Add(User user)
        {
            var errors = new List<string>();

			if (user.EmailAddress.IsEmailAddress() == false)
				errors.Add("Email address doesn't look right");

            if(EmailAddressExistsInStore(user))
                errors.Add("Email Address is already registerd");

            if (DisplayNameExistsInStore(user))
                errors.Add("User name is already in use");

            if(errors.Any())
                throw new InvalidOperationException(String.Join(",", errors));
            
            user.Id = Guid.NewGuid();
            Insert(user);
        }

        private bool EmailAddressExistsInStore(User user)
        {
            return FindWhere(usr => usr.EmailAddress == user.EmailAddress).Any();
        }

        private bool DisplayNameExistsInStore(User user)
        {
            return FindWhere(usr => usr.DisplayName == user.DisplayName).Any();
        }

        public bool TryAdd(User user)
        {
            try
            {
                Add(user);
                return true;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }

        public User GetByEmailAddress(string emailAddress)
        {
            return FindWhere(usr => usr.EmailAddress == emailAddress).SingleOrDefault();            
        }

        public User GetByDisplayName(string displayName)
        {
            return FindWhere(usr => usr.DisplayName == displayName).SingleOrDefault();
        }
    }
}
