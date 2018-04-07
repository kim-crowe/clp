using System;
using System.Collections.Generic;

namespace CogsLite.Core
{
    public interface ICardStore
    {
        void Add(Card card);
        IEnumerable<Card> Get(Guid gameId);
    }
}
