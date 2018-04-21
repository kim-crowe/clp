using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CogsLite.Core
{
	public class Game : BaseObject
    {
        public string Name { get; set; }
        public Member Owner { get; set; }
        public DateTime CreatedOn { get; set; }
        public JsonSize CardSize {get; set;}
		public string[] CardTypes { get; set; }
		public int CardCount { get; set; }		
    }
}
