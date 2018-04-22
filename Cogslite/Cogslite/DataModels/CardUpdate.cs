using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cogslite.DataModels
{
    public class CardUpdate
    {
		public Guid CardId { get; set; }
		public string Name { get; set; }
		public string Type { get; set; }
		public string Tags { get; set; }
    }
}
