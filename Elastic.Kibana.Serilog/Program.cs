using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;
using System.Reflection;

internal class Program
{
	private static void Main(string[] args)
	{
		ConfigureLogging();

		// CreateHost(args);

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

	private static void ConfigureLogging()
	{
		string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "DEVELOPMENT";
		var configuration = new ConfigurationBuilder()
			.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
			.AddJsonFile(
				$"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json",
				optional: true)
			.Build();

		Log.Logger = new LoggerConfiguration()
			.Enrich.FromLogContext()
			.Enrich.WithExceptionDetails()
			.Enrich.WithMachineName()
			.WriteTo.Debug()
			.WriteTo.Console()
			.WriteTo.Elasticsearch(ConfigureElasticSink(configuration, environment))
			.Enrich.WithProperty("Environment", environment)
			.ReadFrom.Configuration(configuration)
			.CreateLogger();
	}
	private static ElasticsearchSinkOptions ConfigureElasticSink(IConfigurationRoot configuration, string environment)
	{
		string indexFormat = $"{Assembly.GetExecutingAssembly().GetName().Name?.ToLower().Replace(".", "-") ?? string.Empty}-{environment?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}";
		string confUri = configuration["ElasticConfiguration:Uri"] ?? "http://localhost:9200";

		return new ElasticsearchSinkOptions(new Uri(confUri))
		{
			AutoRegisterTemplate = true,
			IndexFormat = indexFormat
		};

	}

}