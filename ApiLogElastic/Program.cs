internal class Program
{
	private static void Main(string[] args)
	{
		WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
		
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
}