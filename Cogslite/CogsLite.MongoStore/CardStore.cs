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

        public async Task<IEnumerable<Card>> Get(Guid gameId)
        {
            return await FindWhere(c => c.GameId == gameId);
        }

		public Card GetSingle(Guid cardId)
		{
			return FindById(cardId);
		}
    }
}
