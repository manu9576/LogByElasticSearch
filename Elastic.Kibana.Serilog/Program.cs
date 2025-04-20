using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;
using System.Reflection;

internal class Program
{
	private static void Main(string[] args)
	{		
		IConfiguration configuration = new ConfigurationBuilder()
			.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
			.AddJsonFile(
				$"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json",
				optional: true)
			.Build();		

		ConfigureLogging(configuration);

		WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

		// Add services to the container.
		builder.Services.AddControllersWithViews();

		builder.Host.UseSerilog();

		var app = builder.Build();

		// Configure the HTTP request pipeline.
		if (!app.Environment.IsDevelopment())
		{
			app.UseExceptionHandler("/Home/Error");
		}
		app.UseStaticFiles();

		app.UseRouting();

		app.UseAuthorization();

		app.MapControllerRoute(
			name: "default",
			pattern: "{controller=Home}/{action=Index}/{id?}");

		app.Run();
	}

	private static void ConfigureLogging(IConfiguration configuration)
	{
		string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "DEVELOPMENT";

		string elasticUri = configuration["ElasticConfiguration:Uri"] ?? "http://localhost:9200";
		string applicationName = configuration["ElasticConfiguration:ApplicationName"] ?? "Elastic.Kibana.Serilog";

		Log.Logger = new LoggerConfiguration()
			.Enrich.FromLogContext()
			.Enrich.WithExceptionDetails()
			.Enrich.WithMachineName()
			.WriteTo.Debug()
			.WriteTo.Console()
			.WriteTo.Elasticsearch(ConfigureElasticSink(elasticUri, applicationName, environment))
			.Enrich.WithProperty("Environment", environment)
			.ReadFrom.Configuration(configuration)
			.CreateLogger();
	}
	private static ElasticsearchSinkOptions ConfigureElasticSink(string elasticUri, string applicationName, string environment)
	{
		string indexFormat = $"{applicationName}-{environment?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}";
		return new ElasticsearchSinkOptions(new Uri(elasticUri))
		{
			AutoRegisterTemplate = true,
			IndexFormat = indexFormat
		};
	}

}