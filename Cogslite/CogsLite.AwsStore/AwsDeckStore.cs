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
            mapping            
                .MapMember(x => x.Owner, y => new Member{Id = y.OwnerId})
                .MapMember(x => x.Items, 
                    y => y.Items == null 
                        ? new DeckItem[0] 
                        : y.Items.Select(i => new DeckItem { CardId = Guid.Parse(i.Key), Amount = i.Value } ).ToArray());
        }

        protected override void CreateOutboundMap(AutoMapper.IMappingExpression<Deck, Entities.CogsDeck> mapping)
        {
            mapping
                .MapMember(x => x.OwnerId, y => y.Owner.Id)
                .MapMember(x => x.Items, y => y.Items.ToDictionary(i => i.CardId.ToString(), i => i.Amount));
        }
    }
}