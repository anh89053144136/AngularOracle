using FluentNHibernate.Mapping;

namespace AngularOracle.DAL.Maping
{
    public class WeatherForecastMap : ClassMap<WeatherForecast>
    {
        public WeatherForecastMap()
        {
            Table("WeatherForecasts");

            Id(x => x.Id).GeneratedBy.Sequence("WeatherForecast_Sequence");

            Map(x => x.Date).Column("WeatherDate");
            Map(x => x.TemperatureC);
            Map(x => x.Summary);
        }
    }
}