using Newtonsoft.Json;
using Serilog;
using ILogger = Serilog.ILogger;

namespace FDA_API.Integration.Clients
{
	/// <summary>
	/// Base class for all API clients
	/// </summary>
	public class ApiClient
	{
		private static HttpClient? _httpClient;
		private static readonly object _lock = new();

		/// <summary>
		/// Logger
		/// </summary>
		private readonly ILogger _logger;

		/// <summary>
		/// Common HttpClient.
		/// </summary>
		public static HttpClient HttpClient
		{
			get
			{
				if (_httpClient == null)
				{
					lock (_lock)
					{
						if (_httpClient == null)
						{
							_httpClient = new HttpClient();
						}
					}
				}

				return _httpClient;
			}
		}

		public ApiClient() 
		{
			_logger = Log.Logger;
		}

		public async Task<T> GetAsync<T>(Uri url, CancellationToken cancellationToken)
		{
			if (url == null)
			{
				_logger.Error($"Url shoult be specified.");
				throw new ArgumentNullException(nameof(url), "Url should be specified!");
			}

			HttpResponseMessage response;
			T result;
			try
			{
				response = await HttpClient.GetAsync(url, cancellationToken);
				if (!response.IsSuccessStatusCode)
				{
					_logger.Error($"API call was processed with error. Status code - {response.StatusCode}");
					throw new Exception(response.ReasonPhrase);
				}

				var content = await response.Content.ReadAsStringAsync(cancellationToken);
				result = JsonConvert.DeserializeObject<T>(content);
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
