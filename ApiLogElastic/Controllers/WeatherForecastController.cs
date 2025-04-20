using ApiLogElastic.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiLogElastic.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WeatherForecastController : ControllerBase
{
	string[] _summaries =
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

	[HttpGet]
	public ActionResult<IEnumerable<WeatherForecast>> Get()
	{
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
