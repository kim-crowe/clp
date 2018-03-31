using CogsLite.Core;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;

namespace CogsLite.MongoStore
{
    public class UserStore : BaseMongoStore, IUserStore
    {
        public UserStore(IConfiguration configuration) : base(configuration)
        {
        }

        public void Add(User user)
        {
            var existingUser = Get(user.Username);
            if(existingUser != null)
                throw new InvalidOperationException("Could not add user as a user names must be unique");

            var database = GetDatabase();
            var userCollection = database.GetCollection<User>("Users");
            user.Id = Guid.NewGuid();
            userCollection.InsertOne(user);
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
            var database = GetDatabase();
            var userCollection = database.GetCollection<User>("Users");
            var filter = Builders<User>.Filter.Where(u => u.Username == username);
            return userCollection.Find(filter).SingleOrDefault();
        }
    }
}
