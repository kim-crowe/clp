using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CogsLite.MongoStore
{
    public abstract class BaseMongoStore<T> where T : Core.BaseObject
    {
        private readonly IConfiguration _configuration;
        private readonly Lazy<MongoConfiguration> _mongoConfiguration;
        private readonly Lazy<IMongoCollection<T>> _getCollection;
        private readonly string _collectionName;

        public BaseMongoStore(string collectionName, IConfiguration configuration)
        {
            _collectionName = collectionName;
            _configuration = configuration;
            
            _mongoConfiguration = new Lazy<MongoConfiguration>(() =>
            {
                var mongoConfiguration = new MongoConfiguration();
                _configuration.GetSection("MongoStore").Bind(mongoConfiguration);
                return mongoConfiguration;
            });

            _getCollection = new Lazy<IMongoCollection<T>>(() => 
            {
                return GetDatabase().GetCollection<T>(_collectionName);
            });
        }        

        protected IMongoDatabase GetDatabase()
        {
            var client = new MongoClient(_mongoConfiguration.Value.ConnectionString);
            return client.GetDatabase(_mongoConfiguration.Value.Database);
        }

        protected FilterDefinition<T> CreateFilter(Expression<Func<T, bool>> predicate)
        {
            return Builders<T>.Filter.Where(predicate);
        }

        protected async Task<List<T>> FindWhere(Expression<Func<T, bool>> predicate)
        {
            var results = await Collection.FindAsync(CreateFilter(predicate));
            return results.ToList();
        }

        protected T FindById(Guid id)
        {
            return Collection.Find(CreateFilter(x => x.Id == id)).SingleOrDefault();
        }

        protected void Insert(T item)
        {
            var existing = FindById(item.Id);
            if(existing != null)
                throw new InvalidOperationException("An item with the object id already exists");
            Collection.InsertOne(item);
        }

        protected void Update(T item)
        {
            var existing = FindById(item.Id);
            if(existing == null)
                throw new InvalidOperationException("Unable to find item to replace");
            Collection.ReplaceOne(CreateFilter(x => x.Id == item.Id), item);
        }

        public void UpdateOne(Guid itemId, Action<T> action)
        {
            var item = FindById(itemId);
            if (item == null)
                throw new InvalidOperationException("Unable to find item");
            action(item);
            Update(item);
        }

        protected IMongoCollection<T> Collection => _getCollection.Value;    
    }
}
