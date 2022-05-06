using FDA_API.Integration.Models;

namespace FDA_API.Integration.Abstractions
{
	public interface IFdaClient
	{
		/// <summary>
		/// Find report date with the fewest number of recalls by year.
		/// </summary>
		/// <param name="dateRange">Date range to pass to API</param>
		/// <returns>Report date</returns>
		Task<List<CountResult>?> FindReportDateWithFewestCountByYear(int year, CancellationToken cancellationToken);
	}
}
