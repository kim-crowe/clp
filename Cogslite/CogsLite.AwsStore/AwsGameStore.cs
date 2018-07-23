using System;
using System.Collections.Generic;
using CogsLite.Core;

namespace CogsLite.AwsStore
{
    public class AwsGameStore : IGameStore
    {
        public void Add(Game item)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Game> Get()
        {
            throw new NotImplementedException();
        }

        public Game GetSingle(Guid gameId)
        {
            throw new NotImplementedException();
        }

        public bool TryAdd(Game game)
        {
            throw new NotImplementedException();
        }

        public void UpdateOne(Guid id, Action<Game> updateAction)
        {
            throw new NotImplementedException();
        }
    }
}
