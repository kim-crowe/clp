using System;
using System.Collections.Generic;

namespace CogsLite.Core
{
    public interface ICardStore : IBaseStore<Card>
    {
        IEnumerable<Card> Get(Guid gameId);
    }
}
