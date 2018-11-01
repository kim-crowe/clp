using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CogsLite.Core;

namespace CogsLite.NpgSql
{
    public class GameStore : IGameStore
    {
        public Task Add(Game item)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Game>> Get()
        {
            throw new NotImplementedException();
        }

        public Task<Game> GetSingle(Guid ownerId, Guid gameId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> TryAdd(Game game)
        {
            throw new NotImplementedException();
        }

        public Task UpdateOne(Guid hashKey, Guid rangeKey, Action<Game> updateAction)
        {
            throw new NotImplementedException();
        }
    }
}