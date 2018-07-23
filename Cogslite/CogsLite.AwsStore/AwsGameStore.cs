using System;
using System.Collections.Generic;
using CogsLite.Core;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using System.Threading.Tasks;

namespace CogsLite.AwsStore
{
    public class AwsGameStore : IGameStore
    {
        private readonly IAmazonDynamoDB _dynamoService;

        public AwsGameStore(IAmazonDynamoDB dynamoService)
        {
            _dynamoService = dynamoService ?? throw new ArgumentNullException(nameof(dynamoService));
        }

        public async Task Add(Game item)
        {
            var putItemRequest = new PutItemRequest
            {
                TableName = "Cogs.Games",
                Item = item.ToDynamoItem()
            };

            await _dynamoService.PutItemAsync(putItemRequest);            
        }

        public IEnumerable<Game> Get()
        {
            throw new NotImplementedException();
        }

        public Game GetSingle(Guid gameId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> TryAdd(Game game)
        {
            throw new NotImplementedException();
        }

        public void UpdateOne(Guid id, Action<Game> updateAction)
        {
            throw new NotImplementedException();
        }
    }
}
