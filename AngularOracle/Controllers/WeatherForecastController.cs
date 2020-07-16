using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngularOracle.DAL.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AngularOracle.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly IQueryable<WeatherForecast> data = new WeatherForecast[] {
            new WeatherForecast() { Id = 1, Summary = "Freezing", TemperatureC = 10, Date = new DateTime(2020, 12, 29) },
            new WeatherForecast() { Id = 2, Summary = "Bracing", TemperatureC = 9, Date = new DateTime(2020, 12, 29) },
            new WeatherForecast() { Id = 3, Summary = "Chilly", TemperatureC = 8, Date = new DateTime(2020, 12, 29) },
            new WeatherForecast() { Id = 4, Summary = "Cool", TemperatureC = 7, Date = new DateTime(2020, 12, 29) },
            new WeatherForecast() { Id = 5, Summary = "Mild", TemperatureC = 6, Date = new DateTime(2020, 12, 29) },
            new WeatherForecast() { Id = 6, Summary = "Warm", TemperatureC = 5, Date = new DateTime(2020, 12, 29) },
            new WeatherForecast() { Id = 7, Summary = "Balmy", TemperatureC = 4, Date = new DateTime(2020, 12, 29) },
            new WeatherForecast() { Id = 8, Summary = "Hot", TemperatureC = 3, Date = new DateTime(2020, 12, 29) },
            new WeatherForecast() { Id = 9, Summary = "Sweltering", TemperatureC = 2, Date = new DateTime(2020, 12, 29) },
            new WeatherForecast() { Id = 10, Summary = "Scorching", TemperatureC = 1, Date = new DateTime(2020, 12, 29) }
        }.AsQueryable();

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet("list")]
        public IEnumerable<WeatherForecast> List(string orderField, string orderDirection) => 
            data.ApplyOrder(orderField, orderDirection);

        [HttpGet("{id}")]
        public WeatherForecast Get(int id) => 
            data.FirstOrDefault(x => x.Id == id);

        [HttpPost]
        public void Save([FromBody] WeatherForecast weatherForecast)
        {
            string str = "";
        }
    }
}
