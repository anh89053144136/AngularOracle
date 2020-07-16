using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;

namespace AngularOracle.DAL.Infrastructure
{
    public class NHibernateHelper
    {
        private const string CurrentSessionKey = "nhibernate.current_session";
        private static ISessionFactory _sessionFactory;
        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor httpContextAccessor;

        private static object syncRoot = new object();
        private static ISession internalSession;

        public string id
        {
            get
            {
                return (httpContextAccessor.HttpContext.Items[CurrentSessionKey] as ISession).ToString();
            }
        }

        public NHibernateHelper(Microsoft.AspNetCore.Http.IHttpContextAccessor httpContextAccessor,
            IConnections connections)
        {
            this.httpContextAccessor = httpContextAccessor;

            string connectionString = connections.GetDefaultConnectionString();

            FluentConfiguration fluentConfiguration = Fluently.Configure();
                fluentConfiguration = fluentConfiguration.Database(OracleClientConfiguration.Oracle10
                    .ConnectionString(connectionString));

            _sessionFactory = fluentConfiguration
             .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Mappings>())
             .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Program>())
             .ExposeConfiguration(x => x.SetProperty("hbm2ddl.keywords", "auto-quote"))
             .BuildSessionFactory();
        }

        /// <summary>
        /// Только один экземпляр ISession в рамках одного приложения. Из документации NHibernate:
        /// The application must be careful not to open two concurrent ISessions on the same ADO.NET connection!
        /// </summary>
        /// <returns></returns>
        public ISession GetCurrentSession()
        {
            ISession currentSession;

            /* RequestCollector - задача которая выполняется в фоновом режиме на протяжении всей жизни приложения. Выполняет удаление старых заявок.
             * Соответтвенно для RequestCollector не может существовать HttpContext. Т к есть только один экземпляр RequestCollector.
             * Для задачи правомерно вернуть экземпляр сессии.
            */
            if (httpContextAccessor.HttpContext == null)
            {
                if (internalSession == null)
                {
                    lock (syncRoot)
                    {
                        if (internalSession == null)
                            internalSession = _sessionFactory.OpenSession();
                    }
                }

                return internalSession;
            }

            if (!httpContextAccessor.HttpContext.Items.ContainsKey(CurrentSessionKey))
            {
                currentSession = _sessionFactory.OpenSession();
                httpContextAccessor.HttpContext.Items[CurrentSessionKey] = currentSession;
            }
            else
            {
                currentSession = httpContextAccessor.HttpContext.Items[CurrentSessionKey] as ISession;
            }

            return currentSession;
        }

        public void CloseSession()
        {
            var currentSession = httpContextAccessor.HttpContext.Items[CurrentSessionKey] as ISession;

            if (currentSession == null)
            {
                // No current session
                return;
            }

            currentSession.Close();
            httpContextAccessor.HttpContext.Items.Remove(CurrentSessionKey);
        }

        public void CloseSessionFactory()
        {
            if (_sessionFactory != null)
            {
                _sessionFactory.Close();
            }
        }
    }
}
