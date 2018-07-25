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

        public async Task UpdateOne(Guid id, Action<Card> updateAction)
        {
            var card = await FindById(id);
            updateAction(card);
            await PutItem(card);
        }

        protected override IMapper GetMapper()
        {
            throw new NotImplementedException();
        }
    }
}