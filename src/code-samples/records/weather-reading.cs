var today = new WeatherReading("Seattle", 9.5m);
var warmer = today with { TemperatureC = 12.0m };

public record WeatherReading(string City, decimal TemperatureC);
