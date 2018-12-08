
using CogsLite.Core;
using CogsLite.LocalImageStore;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddLocalImageStore(this IServiceCollection serviceCollection, string basePath, string baseUri)
        {
            serviceCollection.Configure<ImageStoreOptions>(opts => {
                opts.BasePath = basePath;
                opts.BaseUri = baseUri;
            });
            serviceCollection.AddScoped<IImageStore, ImageStore>();
            return serviceCollection;
        }
    }
}