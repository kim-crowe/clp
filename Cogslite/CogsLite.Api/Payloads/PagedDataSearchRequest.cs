namespace CogsLite.Api.Payloads
{
    public abstract class PagedDataSearchRequest
	{
		public int Page { get; set; }
		public int ItemsPerPage { get; set; }
	}
}