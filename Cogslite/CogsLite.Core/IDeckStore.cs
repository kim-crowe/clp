using System;
using System.Collections.Generic;

namespace CogsLite.Core
{
	public interface IDeckStore
	{
		void Save(Deck deck);
		Deck Get(Guid deckId);
		IEnumerable<Deck> ByGameAndOwner(Guid gameId, Guid ownerId);
	}
}