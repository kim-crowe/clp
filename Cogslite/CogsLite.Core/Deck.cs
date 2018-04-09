namespace CogsLite.Core
{
    public class Deck : BaseObject
    {
        public string Name { get; set; }
        public DeckItem[] Items { get; set; }
    }
}