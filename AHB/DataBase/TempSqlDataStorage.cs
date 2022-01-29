using LinqToDB;

namespace AHB.DataBase
{
    public class TempSqlDataStorage<T> : TempTable<T>, ITempDataStorage<T> where T : class
    {
        public TempSqlDataStorage(string storageName, IQueryable<T> query, IDataContext dataConnection)
            : base(dataConnection, storageName, query, tableOptions: TableOptions.NotSet | TableOptions.DropIfExists)
        {
            dataConnection.CloseAfterUse = true;
        }
    }
}