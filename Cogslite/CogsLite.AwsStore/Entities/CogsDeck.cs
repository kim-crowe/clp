using System;
using System.Collections.Generic;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;

namespace CogsLite.AwsStore.Entities
{
    [DynamoDBTable("Cogs.Decks")]
    public class CogsDeck
    {
        [DynamoDBHashKey]        
        [DynamoDBProperty(typeof(GuidConverter))]
        public Guid OwnerId { get; set; }

        [DynamoDBRangeKey]
        [DynamoDBProperty(typeof(GuidConverter))]
        public Guid Id { get; set; }
        
        [DynamoDBProperty(typeof(GuidConverter))]
        public Guid GameId { get; set; }
        
        public string Name { get; set; }

        public Dictionary<string, int> Items { get; set; }
    }

    public class GuidConverter : IPropertyConverter
    {
        public object FromEntry(DynamoDBEntry entry)
        {
            return entry.AsGuid();
        }

        public DynamoDBEntry ToEntry(object value)
        {
            return (DynamoDBEntry)(Guid.Parse(value.ToString()));
        }
    }
}