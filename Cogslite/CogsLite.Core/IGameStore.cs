using System;
using System.Collections.Generic;

namespace CogsLite.Core
{
    public interface IGameStore
    {
        void Add(Game game);
        bool TryAdd(Game game);
        IEnumerable<Game> Get();

        Game GetSingle(Guid gameId);
    }
}
