using AngularOracle;
using AngularOracle.DAL.Infrastructure;
using AngularOracle.Migrations;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace AllTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void MigrateToLast()
        {
            var migrator = new DBMigrator(new Connections());

            migrator.UpdateDatabase();
        }

        [TestMethod]
        public void InsertFirtRecord()
        {
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(x => x.Items).Returns(new Dictionary<object, object>());

            mockHttpContextAccessor.Setup(req => req.HttpContext).Returns(mockHttpContext.Object);

            NHibernateHelper hibernateHelper = new NHibernateHelper(mockHttpContextAccessor.Object, new Connections());

            var session = hibernateHelper.GetCurrentSession();

            using (var transaction = session.BeginTransaction())
            {
                for (int i = 0; i < 3; i++)
                    session.Save(new WeatherForecast()
                    {
                        Date = DateTime.Now,
                        Summary = i.ToString(),
                        TemperatureC = i
                    });

                transaction.Commit();
            }
        }
    }
}