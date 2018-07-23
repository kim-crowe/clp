using System;
using System.Collections.Generic;
using CogsLite.Core;
using Microsoft.Extensions.Configuration;

namespace CogsLite.MongoStore
{
    public class DeckStore : BaseMongoStore<Deck>, IDeckStore
    {
        public DeckStore(IConfiguration configuration) : base("Decks", configuration)
        {

        }

        public Deck Get(Guid deckId)
        {
            return FindById(deckId);
        }

		public IEnumerable<Deck> ByGameAndOwner(Guid gameId, Guid ownerId)
		{
			return FindWhere(d => d.GameId == gameId && d.Owner.Id == ownerId).Result;
		}

        public void Save(Deck deck)
        {            
            var existing = FindById(deck.Id);
            if(existing == null)
            {
                Insert(deck).GetAwaiter().GetResult();
            }
            else
            {
                Update(deck);
            }
        }
    }
}