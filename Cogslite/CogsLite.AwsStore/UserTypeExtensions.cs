using System;
using System.Collections.Generic;
using System.Linq;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.DynamoDBv2.Model;
using CogsLite.Core;

namespace CogsLite.AwsStore
{
    public static class UserTypeExtensions
    {
        public static Member ToMember(this UserType cognitoUser)
        {
            return new Member
            {
                Username = cognitoUser.Username,
                EmailAddress = cognitoUser.Attributes.Single(att => att.Name == "email").Value,
                Id = Guid.Parse(cognitoUser.Attributes.Single(att => att.Name == "id").Value)
            };
        }
    }

    public static class DomainObjectExtensions
    {
        public static Dictionary<string, AttributeValue> ToDynamoItem(this Game game)
        {
            return new Dictionary<string, AttributeValue>
            {
                { "Id", new AttributeValue(game.Id.ToString()) },
                { "Name", new AttributeValue(game.Name) },
                { "CardTypes", new AttributeValue(game.CardTypes.ToList()) },
                { "CardCount", new AttributeValue { N = game.CardCount.ToString() } },
                { "CreatedOn", new AttributeValue(game.CreatedOn.ToString("dd-MM-yyyy") )  }
            };
        }
    }
}
