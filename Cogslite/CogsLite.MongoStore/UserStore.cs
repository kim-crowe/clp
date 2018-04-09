using CogsLite.Core;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
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
            var existingUser = Get(user.Username);
            if(existingUser != null)
                throw new InvalidOperationException("Could not add user as a user names must be unique");
            
            user.Id = Guid.NewGuid();
            Insert(user);
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

        public User Get(string username)
        {
            return FindWhere(usr => usr.Username == username).SingleOrDefault();            
        }
    }
}
