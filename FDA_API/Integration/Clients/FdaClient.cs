using FDA_API.Integration.Abstractions;
using FDA_API.Integration.Models;

namespace FDA_API.Integration.Clients
{
	public class FdaClient : BaseApiClient, IFdaClient
	{
		private const string baseFdaApiUrl = "https://api.fda.gov/food/enforcement.json";
		public const string GetCountOfReportsByYearUrl = "?search=report_date:[{0}0101+TO+{0}1231]&count=report_date";
		public const string GetReportsByDateAndCount = "?search=report_date:{0}&limit={1}";

		public async Task<List<CountResult>?> FindReportDateWithFewestCountByYear(int year, CancellationToken cancellationToken)
		{
			var url = new Uri(string.Format($"{baseFdaApiUrl}{GetCountOfReportsByYearUrl}", year ));
			var response = await GetAsync<ApiResult<CountResult>>(url, cancellationToken);

			return response?.Results;
		}
	}
}
