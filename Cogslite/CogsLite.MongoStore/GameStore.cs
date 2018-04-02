using CogsLite.Core;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace CogsLite.MongoStore
{
    public class GameStore : BaseMongoStore, IGameStore
    {
        public GameStore(IConfiguration configuration) : base(configuration)
        {
        }

        public void Add(Game game)
        {
            var database = GetDatabase();
            var gamesCollection = database.GetCollection<Game>("Games");

            var filter = Builders<Game>.Filter.Where(g => g.Name == game.Name && g.Owner == game.Owner);
            if (gamesCollection.Find(filter).Any())
                throw new InvalidOperationException("A game with that name already exists");

            gamesCollection.InsertOne(game);
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
            var database = GetDatabase();
            var gamesCollection = database.GetCollection<Game>("Games");
            return gamesCollection.Find(FilterDefinition<Game>.Empty).ToList();
        }
    }
}    

