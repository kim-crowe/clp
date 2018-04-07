using System;

namespace CogsLite.Core
{
    public class Card
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid GameId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
