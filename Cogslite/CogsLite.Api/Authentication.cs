using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace CogsLite.Api
{
    public static class Authentication
    {
        private static readonly string _keysEndpoint = "https://cognito-idp.eu-west-2.amazonaws.com/eu-west-2_4YYIIEtaa/.well-known/jwks.json";
        private static Dictionary<string, SecurityKey> _keys;
        private static Task _keyLoaderTask;

        public static void LoadKeys()
        {
            _keyLoaderTask = Task.Run(() => 
            {
                var client = new HttpClient();
                var response = client.GetAsync(_keysEndpoint).GetAwaiter().GetResult();
                var amazonKeyData = response.Content.ReadAsAsync<AmazonKeyResponse>().GetAwaiter().GetResult();

                _keys = new Dictionary<string, SecurityKey>();
                foreach (var keyData in amazonKeyData.keys)
                {
                    var rsa = new RSACryptoServiceProvider();
                    rsa.ImportParameters(
                    new RSAParameters()
                    {
                        Modulus = Base64UrlEncoder.DecodeBytes(keyData.n),
                        Exponent = Base64UrlEncoder.DecodeBytes(keyData.e)
                    });
                    _keys.Add(keyData.kid, new RsaSecurityKey(rsa));
                }
            });
        }
        public static IEnumerable<SecurityKey> GetSigningKey(string keyId)
        {   
            _keyLoaderTask.Wait();
            return _keys.Where(kvp => kvp.Key == keyId).Select(kvp => kvp.Value);
        }

        public class AmazonKeyResponse
        {
            public AmazonKeyData[] keys { get; set; }
        }

        public class AmazonKeyData
        {
            public string kid { get; set; }

            public string alg { get; set; }

            public string kty { get; set; }

            public string e { get; set; }

            public string n { get; set; }

            public string use { get; set; }		
        }
    }
}