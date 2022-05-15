using FDA_API.Integration.Abstractions;
using FDA_API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FDA_API.Controllers
{
	public class HomeController : BaseController
	{
		private readonly IFdaService fdaService;

		public HomeController(IFdaService fdaService): base()
		{
			this.fdaService = fdaService ?? throw new ArgumentNullException(nameof(fdaService));
		}

		public IActionResult Index()
		{
			var model = new InexViewModel();

			return View(model);
		}

		[HttpGet]
		public async Task<JsonResult> FindReportDate(int year, CancellationToken cancellationToken)
		{
			var result = await CallBusinessActionAsyncWithResult( () => 
				 fdaService.FindReportDateWithFewestCountByYear(year, cancellationToken));

			return Json(result);
		}

		[HttpGet]
		public async Task<JsonResult> FindReportsByDate(string date, CancellationToken cancellationToken)
		{
			var result = await CallBusinessActionAsyncWithResult(() =>
				fdaService.FindReportsByDate(date, cancellationToken));

			return Json(result);
		}

		[HttpPost]
		public JsonResult FindMostFequentWord([FromBody] string reportsJson, CancellationToken cancellationToken)
		{
			var result = CallBusinessActionWithResult(() =>
				 fdaService.FindMostFequentWord(reportsJson));

			return Json(result);
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}