using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CogsLite.Core;
using Marten;

namespace CogsLite.MartenStore
{
    public class UserStore : IUserStore
    {
        private readonly IDocumentStore _documentStore;

        public UserStore(IDocumentStore documentStore)
        {
            _documentStore = documentStore ?? throw new ArgumentNullException(nameof(documentStore));
        }

        public Task Add(Game item)
        {
            return Task.Run(() =>
            {
                using(var session = _documentStore.LightweightSession())
                {
                    session.Store(item);
                }
            });
        }

        public async Task<IEnumerable<Game>> Get()
        {            
            using(var querySession = _documentStore.QuerySession())
            {
                return await querySession.Query<Game>().ToListAsync();
            }            
        }

        public async Task<Game> GetSingle(Guid gameId)
        {
            using(var querySession = _documentStore.QuerySession())
            {
                return await querySession.Query<Game>().SingleOrDefaultAsync(g => g.Id == gameId);
            }    
        }

        public Task<bool> TryAdd(Game game)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateOne(Guid gameId, Action<Game> updateAction)
        {
            using(var session = _documentStore.OpenSession())
            {
                var game = await session.Query<Game>().SingleAsync(g => g.Id == gameId);
                updateAction(game);
                session.Store(game);
            }
        }
    }
}
