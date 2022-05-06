namespace FDA_API.Integration.Models
{
	/// <summary>
	/// Paging information.
	/// </summary>
	public class PagingResult
	{
		public int Skip { get; set; }
		public int Limit { get; set; }
		public int Total { get; set; }
	}
}
