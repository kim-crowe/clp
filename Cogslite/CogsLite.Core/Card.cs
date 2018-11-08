using System;

namespace CogsLite.Core
{
    public class Card : BaseObject
    {        
        public string Name { get; set; }
		public string Type { get; set; }
		public string[] Tags { get; set; }
        public Guid GameId { get; set; }
        public DateTime CreatedOn { get; set; }

        public string ImageUrl { get; set; }
    }
}
