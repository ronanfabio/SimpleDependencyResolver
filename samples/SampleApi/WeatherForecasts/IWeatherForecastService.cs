using System;

namespace SampleApi.WeatherForecasts;

public interface IWeatherForecastService
{
    WeatherForecast[] GetWeatherForecasts();
}
