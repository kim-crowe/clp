using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CogsLite.Core
{
    public interface IGameStore : IBaseStore<Game>
    {
        Task<bool> TryAdd(Game game);
        IEnumerable<Game> Get();
        Game GetSingle(Guid gameId);
    }
}
