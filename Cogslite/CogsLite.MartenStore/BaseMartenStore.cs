using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CogsLite.Core;
using Marten;

namespace CogsLite.MartenStore
{
    public class BaseMartenStore<TEntity> where TEntity : BaseObject
    {
        private readonly IDocumentStore _documentStore;

        public BaseMartenStore(IDocumentStore documentStore)
        {
            _documentStore = documentStore ?? throw new ArgumentNullException(nameof(documentStore));
        }

        protected async Task<List<TEntity>> FindWhere(Expression<Func<TEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        protected async Task<TEntity> FindById(Guid id)
        {
            throw new NotImplementedException();
        }

        protected void Insert(TEntity item)
        {
            var existing = FindById(item.Id);
            if(existing != null)
                throw new InvalidOperationException("An item with the object id already exists");
            throw new NotImplementedException();
        }

        protected void Update(TEntity item)
        {
            var existing = FindById(item.Id);
            if(existing == null)
                throw new InvalidOperationException("Unable to find item to replace");
            throw new NotImplementedException();
        }

        public async Task UpdateOne(Guid itemId, Action<TEntity> action)
        {
            var item = await FindById(itemId);
            if (item == null)
                throw new InvalidOperationException("Unable to find item");
            action(item);
            Update(item);
        }
    }
}
