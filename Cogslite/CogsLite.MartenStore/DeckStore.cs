using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using CogsLite.Core;
using Marten;

namespace CogsLite.MartenStore
{
    public class DeckStore : BaseStore<Deck>, IDeckStore
    {    
        public DeckStore(IDocumentStore documentStore) : base(documentStore)
        {    
        }

        public async Task<IEnumerable<Deck>> ByGameAndOwner(Guid gameId, Guid ownerId)
        {
            return await Query(d => d.GameId == gameId && d.Owner.Id == ownerId);
        }

        public async Task<Deck> Get(Guid deckId)
        {
            return await Single(d => d.Id == deckId);
        }

        public async Task Save(Deck deck)
        {
            await Add(deck);
        }
    }
}