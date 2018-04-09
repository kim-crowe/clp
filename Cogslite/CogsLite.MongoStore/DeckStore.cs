using System;
using CogsLite.Core;
using MongoDB.Driver;
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

        public void Save(Deck deck)
        {            
            var existing = FindById(deck.Id);
            if(existing == null)
            {
                Insert(deck);
            }
            else
            {
                Update(deck);
            }
        }
    }
}