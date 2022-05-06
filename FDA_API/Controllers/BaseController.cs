using Microsoft.AspNetCore.Mvc;
using Serilog;
using ILogger = Serilog.ILogger;

namespace FDA_API.Controllers
{
	/// <summary>
	/// Contains usefull common methods for api controllers. 
	/// </summary>
	public class BaseController : Controller
	{
		protected readonly ILogger _logger;

		public BaseController()
		{
			_logger = Log.Logger;
		}

		public async Task<T> CallBusinessActionAsyncWithResult<T>(Func<Task<T>> func)
		{
			T result;
			try
			{
				result = await func.Invoke();
			}
			catch (Exception ex)
			{
				_logger.Error(ex, ex.Message);
				throw;
			}

			return result;
		}
	}
}
