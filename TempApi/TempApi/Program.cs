using InfluxDB.Client;
using TemperatureApi.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var config = builder.Configuration.GetSection(InfluxConfig.ConfigName).Get<InfluxConfig>();
var influxClient = new InfluxDBClient(config.Address, config.ApiKey);
builder.Services.AddSingleton<IInfluxDBClient>(influxClient);
builder.Services.AddScoped<IInfluxRepository, InfluxRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
