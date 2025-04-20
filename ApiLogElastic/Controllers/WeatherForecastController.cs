using ApiLogElastic.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiLogElastic.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WeatherForecastController : ControllerBase
{
	string[] summaries =
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
	public IEnumerable<WeatherForecast> Get()
	{
		Random rng = new Random();
		IEnumerable<WeatherForecast> forecast = 
			Enumerable
				.Range(1, 5)
				.Select(index => new WeatherForecast(
					Date: DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
					TemperatureC: rng.Next(-20, 55),
					Summary: summaries[rng.Next(summaries.Length)]));

		return [.. forecast];
	}
}
