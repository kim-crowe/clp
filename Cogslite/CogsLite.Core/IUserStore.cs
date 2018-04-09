using System;

namespace CogsLite.Core
{
    public interface IUserStore
    {
        User GetByEmailAddress(String emailAddress);
        User GetByDisplayName(String displayName);
        void Add(User user);
    }
}
