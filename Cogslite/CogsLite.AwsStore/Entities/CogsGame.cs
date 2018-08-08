using System;
using Amazon.DynamoDBv2.DataModel;
using CogsLite.Core;
using AutoMapper;

namespace CogsLite.AwsStore.Entities
{
    [DynamoDBTable("Cogs.Games")]
    public class CogsGame
    {
        [DynamoDBHashKey]
        public string OwnerId { get; set; }

        [DynamoDBRangeKey]
        public string Id { get; set; }

        public string Name { get; set; }        

        public Member Owner { get; set; }
        public DateTime CreatedOn { get; set; }
        public JsonSize CardSize {get; set;}
		public int CardCount { get; set; }        
    }    
}