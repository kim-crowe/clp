using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using CogsLite.Core;

namespace CogsLite.AwsStore
{
    public class AwsUserStore : IUserStore
    {                
        private readonly IAmazonCognitoIdentityProvider _identityProvider;
        private readonly string _clientId;
        private readonly string _userPoolId;

        public AwsUserStore(IAmazonCognitoIdentityProvider identityProvider, string clientId, string userPoolId)
        {
            _identityProvider = identityProvider ?? throw new ArgumentNullException(nameof(identityProvider));
            _clientId = clientId;
            _userPoolId = userPoolId;
        }

        public async Task<Member> GetByEmailAddress(String emailAddress)
        {
            var listUsersRequest = new ListUsersRequest
            {
                UserPoolId = _userPoolId,
                Filter = $"email=\"{emailAddress}\""
            };

            var response = await _identityProvider.ListUsersAsync(listUsersRequest);            
            if (response.Users.Count == 1)
                return response.Users.First().ToMember();

            return null;
        }

        public async Task<Member> GetByUsername(String username)
        {
            var listUsersRequest = new ListUsersRequest
            {
                UserPoolId = _userPoolId,               
                Filter = $"username=\"{username}\""
            };

            var response = await _identityProvider.ListUsersAsync(listUsersRequest);
            if (response.Users.Count == 1)
                return response.Users.First().ToMember();

            return null;
        }        

        public async Task Add(User user)
        {
            var signupRequest = new SignUpRequest
            {
                ClientId = _clientId,
                Username = user.Username,
                Password = user.Password,
                UserAttributes = new List<AttributeType>
                {
                    new AttributeType { Name = "id", Value = Guid.NewGuid().ToString() },
                    new AttributeType { Name = "email", Value = user.EmailAddress }
                }
            };

            var response = await _identityProvider.SignUpAsync(signupRequest);            
        }

        public async Task<Member> SignIn(string emailAddress, string password)
        {
            try
            {
                var authReq = new AdminInitiateAuthRequest
                {
                    UserPoolId = _userPoolId,
                    ClientId = _clientId,
                    AuthFlow = AuthFlowType.ADMIN_NO_SRP_AUTH
                };
                authReq.AuthParameters.Add("USERNAME", emailAddress);
                authReq.AuthParameters.Add("PASSWORD", password);

                var authResponse = await _identityProvider.AdminInitiateAuthAsync(authReq);

                return await GetByEmailAddress(emailAddress);
            }
            catch
            {
                return null;
            }
        }
    }
}
