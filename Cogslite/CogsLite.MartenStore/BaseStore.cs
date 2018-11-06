using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using CogsLite.Core;
using Marten;
using System.Linq.Expressions;

namespace CogsLite.MartenStore
{
    public class BaseStore<TEntity> where TEntity : BaseObject
    {
        private readonly IDocumentStore _documentStore;

        public BaseStore(IDocumentStore documentStore)
        {
            _documentStore = documentStore ?? throw new ArgumentNullException(nameof(documentStore));
        }

        public async Task Add(TEntity item)
        {            
            using(var session = _documentStore.LightweightSession())
            {
                session.Store(item);
                await session.SaveChangesAsync();
            }            
        }

        public async Task<IEnumerable<TEntity>> Get()
        {            
            using(var querySession = _documentStore.QuerySession())
            {
                return await querySession.Query<TEntity>().ToListAsync();
            }            
        }

        public async Task<TEntity> GetSingle(Guid gameId)
        {
            using(var querySession = _documentStore.QuerySession())
            {
                return await querySession.Query<TEntity>().SingleOrDefaultAsync(g => g.Id == gameId);
            }    
        }

        public async Task UpdateOne(Guid id, Action<TEntity> updateAction)
        {
            using(var session = _documentStore.OpenSession())
            {
                var item = await session.Query<TEntity>().SingleAsync(x => x.Id == id);
                updateAction(item);
                session.Store(item);
            }
        }

        public async Task<IEnumerable<TEntity>> Query(Expression<Func<TEntity, bool>> predicate)
        {
            using(var session = _documentStore.QuerySession())
            {
                return await session.Query<TEntity>().Where(predicate).ToListAsync();
            }
        }

        public async Task<TEntity> Single(Expression<Func<TEntity, bool>> predicate)
        {
            using(var session = _documentStore.QuerySession())
            {
                return await session.Query<TEntity>().SingleOrDefaultAsync(predicate);
            }
        }
    }
}
