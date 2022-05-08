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
		private const int MaxLimit = 1000;

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
				throw new ArgumentException("Value should be greater than zero", nameof(year));
			}

			var response = await fdaClient.GetAmountOfReportsByYear(year, cancellationToken);
			if (response?.Results == null || !response.Results.Any())
			{
				_logger.Warning($"API method {nameof(FindReportDateWithFewestCountByYear)} returned no results!");
				return string.Empty;
			}

			var date = FindReportDate(response.Results);

			if (DateTime.TryParseExact(date, DateFormat, new CultureInfo("en-US"), DateTimeStyles.None, out DateTime dateValue))
			{
				return dateValue.ToShortDateString();
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

		/// <inheritdoc/>
		public async Task<List<Report>> FindReportsByDate(string date, CancellationToken cancellationToken)
		{
			if (string.IsNullOrEmpty(date))
			{
				throw new ArgumentException("Value should be specified", nameof(date));
			}

			var formattedDate = GetFormattedDate(date);

			var getAmountResponse = await fdaClient.GetAmountOfReportsByDate(formattedDate, cancellationToken);

			var count = (getAmountResponse?.Results != null && getAmountResponse.Results.Any()) ? 
				getAmountResponse.Results.First().Count : 
				MaxLimit;

			var response = await fdaClient.FindReportsByDate(formattedDate, count, cancellationToken);
			if (response?.Results == null || !response.Results.Any())
			{
				_logger.Warning($"API method {nameof(FindReportsByDate)} returned no results!");
				return null;
			}

			return response.Results.OrderBy(x => x.Recall_initiation_dateTime).ToList();
		}

		private string GetFormattedDate(string date)
		{
			if (string.IsNullOrEmpty(date))
			{
				return string.Empty;
			}

			var array = date.Split(".");
			Array.Reverse(array);

			return String.Concat(array);
		}
	}
}
