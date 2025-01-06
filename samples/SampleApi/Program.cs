using SampleApi.WeatherForecasts;
using SimpleDependencyResolver;

var builder = WebApplication.CreateBuilder(args);

var assembly = typeof(Program).Assembly;
builder.Services.AddSimpleDependencyResolver(assembly);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app
    .MapGet("/weatherforecast", ([FromKeyedServices("Scoped")] IWeatherForecastService service) => service.GetWeatherForecasts())
    .WithName("GetWeatherForecast");

app.Run();