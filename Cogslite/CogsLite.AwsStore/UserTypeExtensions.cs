using System;
using System.Linq;
using Amazon.CognitoIdentityProvider.Model;
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
}
