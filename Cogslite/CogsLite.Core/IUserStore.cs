using System;
using System.Threading.Tasks;

namespace CogsLite.Core
{
    public interface IUserStore
    {
        Task<Member> GetByEmailAddress(string emailAddress);
        Task<Member> GetByUsername(string username);
        Task Add(User user);
        Task<Member> SignIn(string emailAddress, string password);
    }
}
