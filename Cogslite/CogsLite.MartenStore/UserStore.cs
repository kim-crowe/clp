using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using CogsLite.Core;
using Marten;

namespace CogsLite.MartenStore
{
    public class UserStore : BaseStore<User>, IUserStore
    {
        public UserStore(IDocumentStore documentStore) : base(documentStore)
        {            
        }

        public async Task<Member> GetByEmailAddress(string emailAddress)
        {            
            return await Single(u => u.EmailAddress == emailAddress);
        }

        public async Task<Member> GetByUsername(string username)
        {
            return await Single(u => u.Username == username);
        }

        public async Task<Member> SignIn(string emailAddress, string password)
        {
            var user = await GetByEmailAddress(emailAddress) as User;
            if(user != null && user.Password == password)
                return user;
            
            return null;
        }
    }
}
