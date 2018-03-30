using System;

namespace CogsLite.Core
{
    public interface IUserStore
    {
        User Get(String username);
        void Add(User user);
        bool TryAdd(User user);
    }
}
