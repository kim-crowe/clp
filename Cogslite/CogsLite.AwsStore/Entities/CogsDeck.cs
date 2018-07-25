using System;
using Amazon.DynamoDBv2.DataModel;

namespace CogsLite.AwsStore.Entities
{
    [DynamoDBTable("Cogs.Decks")]
    public class CogsDeck
    {
        [DynamoDBHashKey]
        public Guid OwnerId { get; set; }

        [DynamoDBRangeKey]
        public Guid Id { get; set; }
        
        public Guid GameId { get; set; }
        
        public string Name { get; set; }
        public Core.DeckItem[] Items { get; set; }
    }
}