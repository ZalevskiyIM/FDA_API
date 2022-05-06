namespace FDA_API.Integration.Models
{
	/// <summary>
	/// Meta data part of Api response.
	/// </summary>
	public class Meta
	{
		public string Disclaimer { get; set; }
		public string Terms { get; set; }
		public string License { get; set; }
		public string Last_updated { get; set; }
		public PagingResult Results { get; set; }

		public Meta()
		{
			Results = new PagingResult();
		}
	}
}
