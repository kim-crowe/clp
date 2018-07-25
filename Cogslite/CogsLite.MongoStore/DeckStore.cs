using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CogsLite.Core;
using Microsoft.Extensions.Configuration;

namespace CogsLite.MongoStore
{
    public class DeckStore : BaseMongoStore<Deck>, IDeckStore
    {
        public DeckStore(IConfiguration configuration) : base("Decks", configuration)
        {

        }

        public async Task<Deck> Get(Guid ownerId, Guid deckId)
        {
            return await FindByIdAsync(deckId);
        }

		public async Task<IEnumerable<Deck>> ByGameAndOwner(Guid gameId, Guid ownerId)
		{
			return await FindWhere(d => d.GameId == gameId && d.Owner.Id == ownerId);
		}

        public async Task Save(Deck deck)
        {            
            var existing = await FindByIdAsync(deck.Id);
            if(existing == null)
            {
                await Insert(deck);
            }
            else
            {
                await Update(deck);
            }
        }
    }
}