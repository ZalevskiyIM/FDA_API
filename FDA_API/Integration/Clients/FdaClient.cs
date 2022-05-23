using FDA_API.Common;
using FDA_API.Integration.Abstractions;
using FDA_API.Integration.Models;
using Serilog;
using ILogger = Serilog.ILogger;

namespace FDA_API.Integration.Clients
{
	public class FdaClient : IFdaClient
	{
		private const string BaseFdaApiUrl = "https://api.fda.gov/";
		private const string EnforcementPrefix = "food/enforcement.json";
		private const string GetCountOfReportsByYearUrl = "?search=report_date:[{0}0101+TO+{0}1231]&count=report_date";
		private const string GetCountOfReportsByDateUrl = "?search=report_date:{0}&count=report_date";
		private const string GetReportsByDateAndLimitUrl = "?search=report_date:{0}&limit={1}";
		private const string GetReportsByDatePagedUrl = "?search=report_date:{0}&skip ={1}&limit={2}";

		private readonly HttpClient _httpClient;

		/// <summary>
		/// Logger
		/// </summary>
		private readonly ILogger _logger;

		public FdaClient(HttpClient httpClient)
		{
			_httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
			_httpClient.BaseAddress = new Uri(BaseFdaApiUrl);

			_logger = Log.Logger;
		}

		/// <inheritdoc/>
		public async Task<ApiResult<CountResult>?> GetAmountOfReportsByYear(int year, CancellationToken cancellationToken)
		{
			var url = $"{EnforcementPrefix}{string.Format(GetCountOfReportsByYearUrl, year)}";
			var response = await GetAsync<ApiResult<CountResult>>(url, cancellationToken);

			return response;
		}

		/// <inheritdoc/>
		public async Task<ApiResult<CountResult>?> GetAmountOfReportsByDate(string date, CancellationToken cancellationToken)
		{
			var url = $"{EnforcementPrefix}{string.Format(GetCountOfReportsByDateUrl, date)}";
			var response = await GetAsync<ApiResult<CountResult>>(url, cancellationToken);

			return response;
		}

		/// <inheritdoc/>
		public async Task<ApiResult<Report>?> FindReportsByDate(string date, int count, CancellationToken cancellationToken)
		{
			ApiResult<Report>? response = new();
			if(count <= Constants.MaxRecordsLimit)
			{
				var url = $"{EnforcementPrefix}{string.Format(GetReportsByDateAndLimitUrl, date, count)}";
				response = await GetAsync<ApiResult<Report>>(url, cancellationToken);
			}
			else
			{
				var skip = 0;
				var reports = new List<Report>();
				while (count > 0)
				{
					var url = $"{EnforcementPrefix}{string.Format(GetReportsByDatePagedUrl, date, skip, Constants.MaxRecordsLimit)}";
					var result = await GetAsync<ApiResult<Report>>(url, cancellationToken);
					if (result?.Results != null)
					{
						reports.AddRange(result.Results);
					}
					skip = Constants.MaxRecordsLimit;
					count -= Constants.MaxRecordsLimit;
				}

				response.Results = reports;
			}

			return response;
		}

		private async Task<T?> GetAsync<T>(string url, CancellationToken cancellationToken)
		{
			if (string.IsNullOrEmpty(url))
			{
				_logger.Error($"Url shoult be specified.");
				throw new ArgumentNullException(nameof(url), "Url should be specified!");
			}

			T? result;
			try
			{
				result = await _httpClient.GetFromJsonAsync<T>(url, cancellationToken);
			}
			catch (Exception ex)
			{
				_logger.Error(ex.Message);
				throw;
			}

			return result;
		}

	}
}
