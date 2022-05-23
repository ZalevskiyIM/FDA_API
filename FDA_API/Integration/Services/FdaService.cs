using FDA_API.Common;
using FDA_API.Integration.Abstractions;
using FDA_API.Integration.Models;
using Newtonsoft.Json;
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

			var date = FindDateWithFewestCount(response.Results);

			if (DateTime.TryParseExact(date, Constants.DateFormat, new CultureInfo("en-US"), DateTimeStyles.None, out DateTime dateValue))
			{
				return dateValue.ToShortDateString();
			}

			return date;
		}

		/// <inheritdoc/>
		public string FindDateWithFewestCount(List<CountResult> response)
		{
			if (response == null || !response.Any())
			{
				return string.Empty;
			}

			var sortedResult = response.Where(x => !string.IsNullOrEmpty(x.Time)).OrderBy(x => x.Count).ToList();

			return sortedResult.FirstOrDefault() == null ? string.Empty : sortedResult.First().Time;
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
				Constants.MaxRecordsLimit;

			var response = await fdaClient.FindReportsByDate(formattedDate, count, cancellationToken);
			if (response?.Results == null || !response.Results.Any())
			{
				_logger.Warning($"API method {nameof(FindReportsByDate)} returned no results!");
				return null;
			}

			return response.Results.OrderBy(x => x.Recall_initiation_dateTime).ToList();
		}

		/// <inheritdoc/>
		public string FindMostFequentWord(string reportsJson)
		{
			if (string.IsNullOrEmpty(reportsJson))
			{
				return string.Empty;
			}

			var reports = JsonConvert.DeserializeObject<IList<Report>>(reportsJson);
			if (reports == null || !reports.Any())
			{
				return string.Empty;
			}

			var reasons = reports.Where(x => !string.IsNullOrEmpty(x.Reason_for_recall))
				.Select(x => x.Reason_for_recall.Split(' ').Where(y => y.Length >= Constants.MinWordLength).ToList())
				.ToList();
			if (reasons == null || !reasons.Any())
			{
				return string.Empty;
			}

			var wordAndCount = new Dictionary<string, int>();
			foreach (var reason in reasons)
			{
				foreach (var item in reason)
				{
					if (wordAndCount.ContainsKey(item))
					{
						var value = wordAndCount[item];
						wordAndCount[item] = ++value;
					}
					else
					{
						wordAndCount.Add(item, 1);
					}
				}
			}

			var result = wordAndCount.FirstOrDefault(x => x.Value == wordAndCount.Values.Max());

			return result.Key;
		}

		private static string GetFormattedDate(string date)
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
