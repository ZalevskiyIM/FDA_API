using FDA_API.Integration.Models;

namespace FDA_API.Integration.Abstractions
{
	public interface IFdaClient
	{
		/// <summary>
		/// Find report's amount by year.
		/// </summary>
		/// <param name="year">Year to find report's amount</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>Report date</returns>
		Task<ApiResult<CountResult>?> GetAmountOfReportsByYear(int year, CancellationToken cancellationToken);

		/// <summary>
		/// Find report's amount by date.
		/// </summary>
		/// <param name="date">Date to find report's amount</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns></returns>
		Task<ApiResult<CountResult>?> GetAmountOfReportsByDate(string date, CancellationToken cancellationToken);

		/// <summary>
		/// Find all reports by date
		/// </summary>
		/// <param name="date">Date to find reports by</param>
		/// <param name="count">Number of reports to get</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>Reports</returns>
		Task<ApiResult<Report>?> FindReportsByDate(string date, int count, CancellationToken cancellationToken);
	}
}
