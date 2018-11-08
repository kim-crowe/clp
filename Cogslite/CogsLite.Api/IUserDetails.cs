using System;
using CogsLite.Core;

namespace CogsLite.Api
{
    public interface IUserContext
    {
        User SignedInUser { get; set; }
    }
}