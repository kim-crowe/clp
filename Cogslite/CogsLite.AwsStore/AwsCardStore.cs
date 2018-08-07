using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CogsLite.Core;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using AutoMapper;

namespace CogsLite.AwsStore
{
    public class AwsCardStore : AwsBaseStore<Card, Entities.CogsCard>, ICardStore
    {                
        public AwsCardStore(IDynamoDBContext dbContext)
            : base(dbContext)
        {                        
        }

        public async Task Add(Card item)
        {
            await PutItem(item);
        }

        public async Task<IEnumerable<Card>> Get(Guid gameId)
        {
            return await Query(gameId);
        }

        public async Task UpdateOne(Guid gameId, Guid cardId, Action<Card> updateAction)
        {
            var card = await FindById(gameId, cardId);
            updateAction(card);
            await PutItem(card);
        }

        protected override void CreateInboundMap(IMappingExpression<Entities.CogsCard, Card> mapping)
        {
            mapping                
                .MapMember(x => x.GameId, y => Guid.Parse(y.GameId))
                .MapMember(x => x.Id, y => Guid.Parse(y.Id));
        }

        protected override void CreateOutboundMap(IMappingExpression<Card, Entities.CogsCard> mapping)
        {
            mapping
                .MapMember(x => x.GameId, y => y.GameId.ToString())
                .MapMember(x => x.Id, y => y.Id.ToString());
        }
    }
}