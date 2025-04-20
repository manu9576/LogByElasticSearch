using ApiLogElastic.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiLogElastic.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WeatherForecastController : ControllerBase
{
	private readonly ILogger<WeatherForecastController> _logger;

	private string[] _summaries =
	[
		"Freezing",
		"Bracing",
		"Chilly",
		"Cool",
		"Mild",
		"Warm",
		"Balmy",
		"Hot",
		"Sweltering",
		"Scorching"
	];

	public WeatherForecastController(ILogger<WeatherForecastController> logger)
	{
		_logger = logger;
	}

	[HttpGet]
	public ActionResult<IEnumerable<WeatherForecast>> Get()
	{
		_logger.LogInformation("WeatherForecastController Get executed at {date}", DateTime.UtcNow);

		Random rng = new();

		IEnumerable<WeatherForecast> forecast =
			Enumerable
				.Range(1, 5)
				.Select(index => new WeatherForecast(
					Date: DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
					TemperatureC: rng.Next(-20, 55),
					Summary: _summaries[rng.Next(_summaries.Length)]));

		return Ok(forecast);
	}
}
