using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CogsLite.Core
{
	public interface IDeckStore
	{
		Task Save(Deck deck);
		Task<Deck> Get(Guid ownerId, Guid deckId);
		Task<IEnumerable<Deck>> ByGameAndOwner(Guid gameId, Guid ownerId);
	}
}