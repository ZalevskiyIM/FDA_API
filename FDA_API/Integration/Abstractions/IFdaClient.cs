using FDA_API.Integration.Models;

namespace FDA_API.Integration.Abstractions
{
	public interface IFdaClient
	{
		/// <summary>
		/// Find report date with the fewest number of recalls by year.
		/// </summary>
		/// <param name="dateRange">Date range to pass to API</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>Report date</returns>
		Task<ApiResult<CountResult>> FindReportDateWithFewestCountByYear(int year, CancellationToken cancellationToken);

		/// <summary>
		/// Find all reports by date
		/// </summary>
		/// <param name="date">Date to find reports by</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>Reports</returns>
		Task<ApiResult<Report>> FindReportsByDate(string date, CancellationToken cancellationToken);
	}
}
