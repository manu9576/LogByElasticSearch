using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Elastic.Kibana.Serilog.Models;

namespace Elastic.Kibana.Serilog.Controllers;

public class HomeController : Controller
{
	private readonly ILogger<HomeController> _logger;

	public HomeController(ILogger<HomeController> logger)
	{
		_logger = logger;
	}

	public IActionResult Index()
	{
		_logger.LogInformation("HomeController Index executed at {date}", DateTime.UtcNow);
		return View();
	}

	public IActionResult Privacy()
	{
		try
		{
			throw new Exception("Some bad code was executed");
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "An unknown error occurred on the Index action of the HomeController");
		}
		return View();
	}

	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	public IActionResult Error()
	{
		return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
	}
}
