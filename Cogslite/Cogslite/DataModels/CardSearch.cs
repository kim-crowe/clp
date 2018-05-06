using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cogslite.DataModels
{
    public class CardSearch : PagedDataSearch
    {
		public string CardName { get; set; }
		public int CardType { get; set; }
		public string [] Tags { get; set; }
		public string[] CardIds { get; set; }
    }

	public abstract class PagedDataSearch
	{
		public int Page { get; set; }
		public int ItemsPerPage { get; set; }
	}
}
