using CogsLite.Core;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace CogsLite.MongoStore
{
    public class CardStore : BaseMongoStore<Card>, ICardStore
    {
        public CardStore(IConfiguration configuration) : base("Cards", configuration)
        {
        }

        public void Add(Card card)
        {
            Insert(card);
        }

        public IEnumerable<Card> Get(Guid gameId)
        {
            return FindWhere(c => c.GameId == gameId).Result;            
        }

		public Card GetSingle(Guid cardId)
		{
			return FindById(cardId);
		}
    }
}
