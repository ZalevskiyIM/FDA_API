namespace FDA_API.Integration.Models
{
	/// <summary>
	/// Model of Fda Api response.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class ApiResult<T>
	{
		public Meta Meta { get; set; }
		public List<T> Results { get; set; }

		public ApiResult()
		{
			Results = new List<T>();
			Meta = new Meta();
		}
	}
}
