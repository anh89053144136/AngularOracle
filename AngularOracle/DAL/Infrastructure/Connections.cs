using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AngularOracle.DAL.Infrastructure
{
    public class Connections : IConnections
    {
        private const string fileName = "appsettings.json";
        private readonly IConfigurationRoot configuration;

        public Connections()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(fileName, optional: true, reloadOnChange: true);

            configuration = builder.Build();
        }

        public string GetDefaultConnectionString()
        {
            return configuration.GetConnectionString("default");
        }
    }
}
