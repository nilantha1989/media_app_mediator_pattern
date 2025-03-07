using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<DataContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DBCS"));
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

using (var scope =app.Services.CreateScope()) {

var services=scope.ServiceProvider;
	try
	{
		var context = services.GetRequiredService<DataContext>();
		context.Database.Migrate();
	}
	catch (Exception ex)
	{

		var logger=services.GetRequiredService<ILogger<Program>>();
		logger.LogError(ex, "Error MG");
	}
}

app.Run();
