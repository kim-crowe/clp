using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CogsLite.Core
{
    public interface IGameStore : IBaseStore<Game>
    {
        Task<IEnumerable<Game>> Get();
        Task<Game> GetSingle(Guid gameId);
    }
}
