using CogsLite.Core;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace CogsLite.MongoStore
{
    public class CardStore : BaseMongoStore, ICardStore
    {
        public CardStore(IConfiguration configuration) : base(configuration)
        {
        }

        public void Add(Card card)
        {
            var database = GetDatabase();
            var cardsCollection = database.GetCollection<Card>("Cards");
            cardsCollection.InsertOne(card);
        }

        public IEnumerable<Card> Get(Guid gameId)
        {
            var database = GetDatabase();
            var cardsCollection = database.GetCollection<Card>("Cards");
            return cardsCollection.Find(FilterDefinition<Card>.Empty).ToList();
        }
    }
}
