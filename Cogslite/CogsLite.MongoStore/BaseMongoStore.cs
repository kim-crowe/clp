using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;

namespace CogsLite.MongoStore
{
    public abstract class BaseMongoStore
    {
        private readonly IConfiguration _configuration;
        private readonly Lazy<MongoConfiguration> _mongoConfiguration;

        public BaseMongoStore(IConfiguration configuration)
        {
            _configuration = configuration;
            _mongoConfiguration = new Lazy<MongoConfiguration>(() =>
            {
                var mongoConfiguration = new MongoConfiguration();
                _configuration.GetSection("MongoStore").Bind(mongoConfiguration);
                return mongoConfiguration;
            });
        }

        protected IMongoDatabase GetDatabase()
        {
            var client = new MongoClient(_mongoConfiguration.Value.ConnectionString);
            return client.GetDatabase(_mongoConfiguration.Value.Database);
        }
    }
}
