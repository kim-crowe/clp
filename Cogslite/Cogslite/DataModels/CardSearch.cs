namespace Cogslite.DataModels
{
    public class CardSearch : PagedDataSearch
    {
		public string CardName { get; set; }
		public string CardType { get; set; }
		public string [] Tags { get; set; }
		public string[] CardIds { get; set; }
    }

	public abstract class PagedDataSearch
	{
		public int Page { get; set; }
		public int ItemsPerPage { get; set; }
	}
}
