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
    public class AwsGameStore : AwsBaseStore<Game, Entities.CogsGame>, IGameStore
    {        
        public AwsGameStore(IDynamoDBContext dbContext)
            : base(dbContext)
        {                 
        }

        public async Task Add(Game game)
        {
            await PutItem(game);
        }

        public async Task<IEnumerable<Game>> Get()
        {
            return await Scan();            
        }

        public async Task<Game> GetSingle(Guid gameId)
        {
            return await FindById(gameId);
        }

        public async Task<bool> TryAdd(Game game)
        {
            try
            {
                await Add(game);
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

        public async Task UpdateOne(Guid id, Action<Game> updateAction)
        {
            // TODO: Consider an approach that only sends up the data changes
            var game = await GetSingle(id);
            updateAction(game);
            await PutItem(game);
        }        

        protected override void CreateOutboundMap(IMappingExpression<Game, Entities.CogsGame> mapping)
        {
            mapping
                .MapMember(x => x.Id, y => y.Id.ToString())
                .MapMember(x => x.OwnerId, y => y.Owner.Id.ToString());                    
        }

        protected override void CreateInboundMap(IMappingExpression<Entities.CogsGame, Game> mapping)
        {
            mapping
                .MapMember(x => x.Id, y => Guid.Parse(y.Id))
                .MapMember(x => x.Owner, y => new Member { Id = Guid.Parse(y.Id) });            
        }        
    }
}
