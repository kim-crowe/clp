using System;

namespace CogsLite.Core
{
    public class Deck : BaseObject
    {
        public Member Owner { get; set; }
        public Guid GameId { get; set; }
        public string Name { get; set; }
        public DeckItem[] Items { get; set; }
        public int Version { get; set; }
    }
}