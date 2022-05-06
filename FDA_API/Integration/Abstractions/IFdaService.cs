﻿using FDA_API.Integration.Models;

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
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns>Report date</returns>
		Task<string> FindReportDateWithFewestCountByYear(int year, CancellationToken cancellationToken);

		/// <summary>
		/// Finds all reports by date.
		/// </summary>
		/// <param name="date">Date to find repots</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <returns></returns>
		Task<List<Report>> FindReportsByDate(string date, CancellationToken cancellationToken);
	}
}
