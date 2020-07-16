using FluentMigrator;
using FluentMigrator.Oracle;

namespace AngularOracle.Migrations
{
    [Migration(20200527000001)]
    public class Migration1_WeatherForecast : Migration
    {
        private const string tabWeatherForecasts = "WeatherForecasts";
        private const string seqWeatherForecasts = "WeatherForecast_Sequence";

        public override void Up()
        {
            Create.Sequence(seqWeatherForecasts).StartWith(1).IncrementBy(1).MinValue(1).MaxValue(long.MaxValue);

            Create.Table(tabWeatherForecasts)
                .WithColumn("Id").AsInt16().PrimaryKey()
                .WithColumn("WeatherDate").AsDate()
                .WithColumn("TemperatureC").AsInt16().WithDefaultValue(0)
                .WithColumn("Summary").AsString(1000);
        }

        public override void Down()
        {
            Delete.Table(tabWeatherForecasts);

            Delete.Sequence(seqWeatherForecasts);
        }
    }
}
