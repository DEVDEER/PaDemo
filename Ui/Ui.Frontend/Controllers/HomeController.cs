namespace Ui.Frontend.Controllers
{
	using System;
	using System.Diagnostics;
	using System.Linq;

	using Microsoft.AspNetCore.Mvc;
	using Microsoft.Extensions.Logging;

	using Models;

	public class HomeController : Controller
	{
		#region member vars

		private readonly ILogger<HomeController> _logger;

		#endregion

		#region constructors and destructors

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}

		#endregion

		#region methods

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		#endregion
	}
}
