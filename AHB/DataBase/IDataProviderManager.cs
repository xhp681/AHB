namespace AHB.DataBase
{
    public partial interface IDataProviderManager
    {
        /// <summary>
        /// Gets data provider
        /// </summary>
        IRsDataProvider DataProvider { get; }
    }
}