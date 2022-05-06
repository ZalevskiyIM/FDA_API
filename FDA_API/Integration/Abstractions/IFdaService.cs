namespace FDA_API.Integration.Abstractions
{
	/// <summary>
	/// Service that works with Fda API.
	/// </summary>
	public interface IFdaService
	{
		/// <summary>
		/// Find report date with the fewest number of recalls by year.
		/// </summary>
		/// <param name="year">Year to search</param>
		/// <returns>Report date</returns>
		Task<string> FindReportDateWithFewestCountByYear(int year, CancellationToken cancellationToken);
	}
}
