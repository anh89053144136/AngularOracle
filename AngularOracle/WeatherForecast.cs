using System;

namespace AngularOracle
{
    public class WeatherForecast
    {
        public virtual int Id { get; set; }

        public virtual DateTime Date { get; set; }

        public virtual int TemperatureC { get; set; }

        public virtual string Summary { get; set; }
    }
}
