using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CogsLite.Core
{
    public interface ICardStore : IBaseStore<Card>
    {
        Task<IEnumerable<Card>> Get(Guid gameId);
    }
}
