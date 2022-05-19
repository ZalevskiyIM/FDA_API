using FDA_API.Integration.Abstractions;
using FDA_API.Integration.Models;

namespace FDA_API.Integration.Clients
{
	public class FdaClient : ApiClient, IFdaClient
	{
		private const string BaseFdaApiUrl = "https://api.fda.gov/food/enforcement.json";
		private const string GetCountOfReportsByYearUrl = "?search=report_date:[{0}0101+TO+{0}1231]&count=report_date";
		private const string GetCountOfReportsByDateUrl = "?search=report_date:{0}&count=report_date";
		private const string GetReportsByDateAndLimitUrl = "?search=report_date:{0}&limit={1}";
		private const string GetReportsByDatePagedUrl = "?search=report_date:{0}&skip ={1}&limit={2}";

		private const int MaxLimit = 1000;

		/// <inheritdoc/>
		public async Task<ApiResult<CountResult>> GetAmountOfReportsByYear(int year, CancellationToken cancellationToken)
		{
			var url = new Uri(string.Format($"{BaseFdaApiUrl}{GetCountOfReportsByYearUrl}", year ));
			var response = await GetAsync<ApiResult<CountResult>>(url, cancellationToken);

			return response;
		}

		/// <inheritdoc/>
		public async Task<ApiResult<CountResult>> GetAmountOfReportsByDate(string date, CancellationToken cancellationToken)
		{
			var url = new Uri(string.Format($"{BaseFdaApiUrl}{GetCountOfReportsByDateUrl}", date));
			var response = await GetAsync<ApiResult<CountResult>>(url, cancellationToken);

			return response;
		}

		/// <inheritdoc/>
		public async Task<ApiResult<Report>> FindReportsByDate(string date, int count, CancellationToken cancellationToken)
		{
			ApiResult<Report> response = new ApiResult<Report>();
			if(count <= MaxLimit)
			{
				var url = new Uri(string.Format($"{BaseFdaApiUrl}{GetReportsByDateAndLimitUrl}", date, count));
				 response = await GetAsync<ApiResult<Report>>(url, cancellationToken);
			}
			else
			{
				var skip = 0;
				var reports = new List<Report>();
				while (count > 0)
				{
					var url = new Uri(string.Format($"{BaseFdaApiUrl}{GetReportsByDatePagedUrl}", date, skip, MaxLimit));
					var result = await GetAsync<ApiResult<Report>>(url, cancellationToken);
					if (result?.Results != null)
					{
						reports.AddRange(result.Results);
					}
					skip = MaxLimit;
					count -= MaxLimit;
				}

				response.Results = reports;
			}

			return response;
		}

		
	}
}
