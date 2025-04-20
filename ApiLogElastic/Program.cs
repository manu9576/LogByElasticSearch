using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;

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

		builder.Host.UseSerilog();
		
		builder.Services.AddControllers();

		builder.Services.AddControllers().AddJsonOptions(options =>
		{
			options.JsonSerializerOptions.Converters.Add
			(
				new System.Text.Json.Serialization.JsonStringEnumConverter()
			);
		});

		// Add services to the container.
		// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
		builder.Services.AddEndpointsApiExplorer();
		builder.Services.AddSwaggerGen();

		WebApplication app = builder.Build();

		// Configure the HTTP request pipeline.
		if (app.Environment.IsDevelopment())
		{
			app.UseSwagger();
			app.UseSwaggerUI();
		}

		app.MapControllers();

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