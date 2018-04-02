using System;
using System.Collections.Generic;
using System.Text;

namespace CogsLite.Core
{
    public interface IGameStore
    {
        void Add(Game game);
        bool TryAdd(Game game);
        IEnumerable<Game> Get();
    }
}
