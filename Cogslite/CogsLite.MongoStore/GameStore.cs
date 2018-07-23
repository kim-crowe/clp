using CogsLite.Core;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Collections.Generic;

namespace CogsLite.MongoStore
{
    public class GameStore : BaseMongoStore<Game>, IGameStore
    {
        public GameStore(IConfiguration configuration) : base("Games", configuration)
        {
        }

        public void Add(Game game)
        {
            var existing = FindWhere(g => g.Name == game.Name).Result;
            if (existing.Any())
                throw new InvalidOperationException("A game with that name already exists");

            Insert(game);
        }

        public bool TryAdd(Game game)
        {
            try
            {
                Add(game);
                return true;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }

        public IEnumerable<Game> Get()
        {            
            return Collection.Find(FilterDefinition<Game>.Empty).ToList();
        }

        public Game GetSingle(Guid gameId)
        {
            return FindById(gameId);            
        }
    }
}    

