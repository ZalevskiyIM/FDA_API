using FDA_API.Integration.Abstractions;
using FDA_API.Integration.Models;
using Serilog;
using System.Globalization;
using ILogger = Serilog.ILogger;

namespace FDA_API.Integration.Services
{
	/// <inheritdoc/>
	public class FdaService : IFdaService
	{
		private readonly IFdaClient fdaClient;
		private readonly ILogger _logger;
		private const string DateFormat = "yyyyMMdd";//20120815

		public FdaService(IFdaClient fdaClient)
		{
			this.fdaClient = fdaClient ?? throw new ArgumentNullException(nameof(fdaClient));
			_logger = Log.Logger;
		}

		/// <inheritdoc/>
		public async Task<string> FindReportDateWithFewestCountByYear(int year, CancellationToken cancellationToken)
		{
			if (year <= 0)
			{
				throw new ArgumentException("Value shoul be greater than zero", nameof(year));
			}

			var response = await fdaClient.FindReportDateWithFewestCountByYear(year, cancellationToken);
			if (response == null || !response.Any())
			{
				_logger.Warning($"API method {nameof(FindReportDateWithFewestCountByYear)} returned no results!");
				return string.Empty;
			}

			var date = FindReportDate(response);

			if (DateTime.TryParseExact(date, DateFormat, new CultureInfo("en-US"), DateTimeStyles.None, out DateTime dateValue))
			{
				return dateValue.ToString("d");
			}

			return date;
		}

		public static string FindReportDate(List<CountResult> response)
		{
			if (response == null || !response.Any())
			{
				return string.Empty;
			}

			var sortedResult = response.Where(x => !string.IsNullOrEmpty(x.Time)).OrderBy(x => x.Count).ToList();

			return sortedResult.First() == null ? string.Empty : sortedResult.First().Time;
		}
	}
}
