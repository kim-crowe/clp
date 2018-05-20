using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GorgleDevs.Mvc;

namespace Cogslite.DataModels
{
    public class CardUpdate
    {
		public String CardId { get; set; }
		public Guid Id => ShortGuid.Parse(CardId);
		public string Name { get; set; }
		public string Type { get; set; }		
    }
}
