using AHB.Core;

namespace AHB.DataBase
{
    public class DataProviderManager: IDataProviderManager
    {
        /// <summary>
        /// Gets data provider by specific type
        /// </summary>
        /// <param name="dataProviderType">Data provider type</param>
        /// <returns></returns>
        public static IRsDataProvider GetDataProvider(DataProviderType dataProviderType)
        {
            return dataProviderType switch
            {
                DataProviderType.SqlServer => new MsSqlRsDataProvider(),
                //DataProviderType.MySql => new MySqlNopDataProvider(),
                //DataProviderType.PostgreSQL => new PostgreSqlDataProvider(),
                _ => throw new Exception($"Not supported data provider name: '{dataProviderType}'"),
            };
        }

        /// <summary>
        /// Gets data provider
        /// </summary>
        public IRsDataProvider DataProvider
        {
            get
            {
                var dataProviderType = Singleton<DataConfig>.Instance.DataProvider;

                return GetDataProvider(dataProviderType);
            }
        }
    }
}
