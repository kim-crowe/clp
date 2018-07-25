using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using CogsLite.Core;

namespace CogsLite.AwsStore
{
    public class AwsDeckStore : AwsBaseStore<Deck, Entities.CogsDeck>, IDeckStore
    {
        public AwsDeckStore(IDynamoDBContext dbContext)
            : base(dbContext)
        {}
        public async Task<IEnumerable<Deck>> ByGameAndOwner(Guid gameId, Guid ownerId)
        {
            var decks = await Query(ownerId);
            return decks.Where(d => d.GameId == gameId);
        }

        public async Task<Deck> Get(Guid ownerId, Guid deckId)
        {
            var decks = await Query(ownerId);
            return decks.SingleOrDefault(d => d.Id == deckId);
        }

        public async Task Save(Deck deck)
        {
            await PutItem(deck);
        }

        protected override void CreateInboundMap(AutoMapper.IMappingExpression<Entities.CogsDeck, Deck> mapping)
        {            
        }

        protected override void CreateOutboundMap(AutoMapper.IMappingExpression<Deck, Entities.CogsDeck> mapping)
        {

        }
    }
}