using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using Amazon.Extensions.NETCore.Setup;

namespace CogsLite.Api
{
    public static class Authentication
    {
        public static SecurityKey GetCognitoKey(AWSOptions awsOptions)
        {
            var cognitoService = awsOptions.CreateServiceClient<IAmazonCognitoIdentityProvider>();
            var request = new GetSigningCertificateRequest
            {
                UserPoolId = "eu-west-2_4YYIIEtaa"
            };
            var response = cognitoService.GetSigningCertificateAsync(request).GetAwaiter().GetResult();
            Console.WriteLine(response.Certificate);
            var cert = new X509Certificate2(Convert.FromBase64String(response.Certificate));
            return new X509SecurityKey(cert);
        }
    }
}