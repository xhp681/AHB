using System.Runtime.Serialization;

namespace AHB.DataBase
{
    public enum DataProviderType
    {
        /// <summary>
        /// Unknown
        /// </summary>
        [EnumMember(Value = "")]
        Unknown,

        /// <summary>
        /// MS SQL Server
        /// </summary>
        [EnumMember(Value = "sqlserver")]
        SqlServer,

        ///// <summary>
        ///// MySQL
        ///// </summary>
        //[EnumMember(Value = "mysql")]
        //MySql,

        ///// <summary>
        ///// PostgreSQL
        ///// </summary>
        //[EnumMember(Value = "postgresql")]
        //PostgreSQL
    }
}