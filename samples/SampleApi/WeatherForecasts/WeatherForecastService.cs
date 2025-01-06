using SimpleDependencyResolver.Attributes;

namespace SampleApi.WeatherForecasts;

[Scoped(ServiceKey = "Scoped")]
public class WeatherForecastService : IWeatherForecastService
{
    private readonly string[] _summaries = { "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" };

    public WeatherForecast[] GetWeatherForecasts() =>
        Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            _summaries[Random.Shared.Next(_summaries.Length)]
        ))
        .ToArray();
}
