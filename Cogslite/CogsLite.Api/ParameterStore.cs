using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Extensions.NETCore.Setup;
using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;

namespace CogsLite.Api
{
    public class ParameterStore 
    {
        private readonly IAmazonSimpleSystemsManagement _ssm;

        public ParameterStore(AWSOptions awsOptions)
        {
                _ssm = awsOptions.CreateServiceClient<IAmazonSimpleSystemsManagement>();
        }

        public async Task<string> GetParameterAsync(string key)
        {
            var request = new GetParameterRequest
            {
                Name = key                
            };

            var response = await _ssm.GetParameterAsync(request);
            return response.Parameter.Value;
        }
    }
}