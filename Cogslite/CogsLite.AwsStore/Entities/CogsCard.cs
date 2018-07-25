using System;
using Amazon.DynamoDBv2.DataModel;

namespace CogsLite.AwsStore.Entities
{
    [DynamoDBTable("Cogs.Cards")]
    public class CogsCard
    {
        [DynamoDBHashKey]
        public string GameId { get; set; }

        [DynamoDBRangeKey]
        public string Id { get; set; }

        public string Type { get; set; }

        public string Name { get; set; }
		public string[] Tags { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}