namespace FDA_API.Integration.Models
{
	/// <summary>
	/// Response for count requets.
	/// </summary>
	public class CountResult
	{
		public string Time { get; set; }
		public int Count { get; set; }
		public string Term { get; set; }
	}
}
