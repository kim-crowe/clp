using Amazon.Extensions.NETCore.Setup;
using Amazon.S3;
using CogsLite.AwsStore;
using CogsLite.Core;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddS3ImageStore(this IServiceCollection services, AWSOptions awsOptions)
        {
            services.AddAWSService<IAmazonS3>(awsOptions);
            services.AddScoped<IImageStore, ImageStore>();
            return services;
        }
    }
}