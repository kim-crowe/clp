using System;
using System.Collections.Generic;
using System.Text;

namespace CogsLite.Core
{
    public class Game
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Member Owner { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
