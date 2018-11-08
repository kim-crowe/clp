namespace CogsLite.Api.Payloads
{
    public class CardSearchRequest : PagedDataSearchRequest
    {
		public string CardName { get; set; }
		public string CardType { get; set; }
		public string [] Tags { get; set; }
		public string[] CardIds { get; set; }
    }
}