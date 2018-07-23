using CogsLite.Core;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CogsLite.MongoStore
{
    public class CardStore : BaseMongoStore<Card>, ICardStore
    {
        public CardStore(IConfiguration configuration) : base("Cards", configuration)
        {
        }

        public async Task Add(Card card)
        {
            await Insert(card);
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
