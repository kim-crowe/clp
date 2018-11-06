using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using CogsLite.Core;
using Marten;

namespace CogsLite.MartenStore
{
    public class CardStore : BaseStore<Card>, ICardStore
    {    
        public CardStore(IDocumentStore documentStore) : base(documentStore)
        {    
        }
        

        public async Task<IEnumerable<Card>> Get(Guid gameId)
        {
            return await Query(c => c.GameId == gameId);
        }        
    }
}