using AngularOracle.DAL.Infrastructure;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularOracle.Migrations
{
    /// <summary>
    /// Тип применяет миграции к БД. Сам находит их сборке и применяет.
    /// Использует подключение в БД по умолчанию.
    /// Миграции прописываются вручную и находятся в "Migrations\". 
    /// Миграции именуются по Migration_<порядковый номер>_<краткое описанеи>.cs
    /// </summary>
    public class DBMigrator
    {
        private IConnections connections;

        public DBMigrator(IConnections connections)
        {
            this.connections = connections;
        }

        /// <summary>
        /// Обновить структуру БД до последней версии. 
        /// Использует подключение в БД по умолчанию.
        /// </summary>
        public void UpdateDatabase()
        {
            string connectionString = connections.GetDefaultConnectionString();

            var serviceCollection = new ServiceCollection()

                .AddFluentMigratorCore()
                .ConfigureRunner(rb => {
                        rb.AddOracleManaged();

                    // задать подключение к БД
                    rb.WithGlobalConnectionString(connectionString)
                    // Сборка в которой искать миграции
                    .ScanIn(typeof(Program).Assembly).For.Migrations();

                });

            var serviceProvider = serviceCollection.BuildServiceProvider(false);

            using (var scope = serviceProvider.CreateScope())
            {
                var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
                runner.MigrateUp();
            }
        }
    }
}
