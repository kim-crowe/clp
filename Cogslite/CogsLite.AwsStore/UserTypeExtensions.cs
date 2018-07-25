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
        public static Entities.CogsGame ToDynamoEntity(this Game game)
        {
            return new Entities.CogsGame
            {
                Id = game.Id.ToString(),
                Name = game.Name,
                CardCount = game.CardCount,
                CardSize = game.CardSize,
                CardTypes = game.CardTypes,
                CreatedOn = game.CreatedOn,
                OwnerId = game.Owner.Id.ToString()
            };
        }        
    }    
}
