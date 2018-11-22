using System;
using CogsLite.Core;

namespace CogsLite.Api
{
    public interface IUserContext
    {
        Member SignedInUser { get; }
    }
}