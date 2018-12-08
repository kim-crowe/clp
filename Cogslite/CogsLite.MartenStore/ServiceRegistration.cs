
using CogsLite.Core;
using Marten;
using Microsoft.Extensions.DependencyInjection;

namespace CogsLite.MartenStore
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddMarten(this IServiceCollection serviceCollection, string connectionString)
        {
            var documentStore = DocumentStore.For(connectionString);
            serviceCollection.AddSingleton<IDocumentStore>(documentStore);
            serviceCollection.AddScoped<IGameStore, GameStore>();
            serviceCollection.AddScoped<ICardStore, CardStore>();
            serviceCollection.AddScoped<IDeckStore, DeckStore>();
            return serviceCollection;
        }
    }
}