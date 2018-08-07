using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CogsLite.Core
{
    public interface IGameStore : IBaseStore<Game>
    {
        Task<bool> TryAdd(Game game);
        Task<IEnumerable<Game>> Get();
        Task<Game> GetSingle(Guid ownerId, Guid gameId);
    }
}
