using System;

namespace Cogslite.Json
{
    public class Deck
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public DeckItem[] items { get; set; }
    }

    public class DeckItem
    {
        public Card card { get; set; }
        public int count { get; set; }
    }

    public class Card
    {
        public string name { get; set; }
        public Guid id { get; set; }
    }
}