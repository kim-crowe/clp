using System;

namespace CogsLite.Core
{
    public interface IDeckStore
    {
        void Save(Deck deck);
        Deck Get(Guid deckId);
    }
}