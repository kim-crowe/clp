using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CogsLite.Core;
using Marten;

namespace CogsLite.MartenStore
{
    public class GameStore : BaseStore<Game>, IGameStore
    {
        public GameStore(IDocumentStore documentStore) : base(documentStore)
        {            
            
        }        
    }
}
