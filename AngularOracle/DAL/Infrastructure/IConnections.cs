namespace AngularOracle.DAL.Infrastructure
{
    /// <summary>
    /// Доступные подключения к БД
    /// </summary>
    public interface IConnections
    {
        /// <summary>
        /// Подключение Default из appsettings.json
        /// </summary>
        /// <returns></returns>
        string GetDefaultConnectionString();
    }
}
