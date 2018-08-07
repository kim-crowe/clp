using CogsLite.Core;
using CogsLite.AwsStore;
using Amazon.Extensions.NETCore.Setup;
using Amazon.CognitoIdentityProvider;
using Amazon.S3;
using Amazon.DynamoDBv2;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Amazon.DynamoDBv2.DataModel;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddAwsStoresForCogs(this IServiceCollection services, AWSOptions awsOptions)
        {
            services.AddAWSService<IAmazonCognitoIdentityProvider>(awsOptions);            
            services.AddAWSService<IAmazonDynamoDB>(awsOptions);
            services.AddAWSService<IAmazonS3>(awsOptions);
            services.AddScoped<IDynamoDBContext, DynamoDBContext>();

            services.AddScoped<IUserStore, AwsUserStore>();
            services.AddScoped<IImageStore, AwsImageStore>();
            services.AddScoped<ICardStore, AwsCardStore>();
            services.AddScoped<IGameStore, AwsGameStore>();
            services.AddScoped<IDeckStore, AwsDeckStore>();

            return services;
        }

        public static IServiceCollection ConfigureCognito(this IServiceCollection services, IConfiguration config)
        {
            return services.Configure<UserStoreOptions>(config.GetSection("UserStoreOptions"));
        }
    }
}