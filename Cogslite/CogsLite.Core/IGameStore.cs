using System;
using System.Collections.Generic;

namespace CogsLite.Core
{
    public interface IGameStore : IBaseStore<Game>
    {
        bool TryAdd(Game game);
        IEnumerable<Game> Get();
        Game GetSingle(Guid gameId);
    }
}
