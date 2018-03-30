using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using CogsLite.Core;
using Newtonsoft.Json;

namespace CogsLite.BasicStore
{
    public class UserStore : IUserStore
    {
        public void Add(User user)
        {
            var users = GetUsers();
            if (users.Any(usr => usr.Username == user.Username))
                throw new InvalidOperationException("Could not add user as a user names must be unique");            

            users.Add(user);
            File.WriteAllText("users.txt", JsonConvert.SerializeObject(users));
        }

        public bool TryAdd(User user)
        {
            try
            {
                Add(user);
                return true;
            }
            catch(InvalidOperationException)
            {
                return false;
            }
        }

        public User Get(string username)
        {
            return GetUsers().SingleOrDefault(usr => usr.Username == username);
        }

        private List<User> GetUsers()
        {
            if (!File.Exists("users.txt"))
            {
                using (var file = File.Create("users.txt"))
                {
                }                
            }                

            var usersText = File.ReadAllText("users.txt");
            var users = JsonConvert.DeserializeObject<List<User>>(usersText) ?? new List<User>();
            return users;
         }
    }
}
