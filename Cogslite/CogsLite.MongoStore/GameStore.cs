using CogsLite.Core;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CogsLite.MongoStore
{
    public class GameStore : BaseMongoStore<Game>, IGameStore
    {
        public GameStore(IConfiguration configuration) : base("Games", configuration)
        {
        }

        public async Task Add(Game game)
        {
            var existing = FindWhere(g => g.Name == game.Name).Result;
            if (existing.Any())
                throw new InvalidOperationException("A game with that name already exists");

            await Insert(game);
        }

        public async Task<bool> TryAdd(Game game)
        {
            try
            {
                await Add(game);
                return true;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }

        public async Task<IEnumerable<Game>> Get()
        {            
            return (await Collection.FindAsync(FilterDefinition<Game>.Empty)).ToList();
        }

        public async Task<Game> GetSingle(Guid ownerId, Guid gameId)
        {
            return await FindByIdAsync(gameId);            
        }
    }
}    

