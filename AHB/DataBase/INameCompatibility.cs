namespace AHB.DataBase
{
    public partial interface INameCompatibility
    {
        /// <summary>
        /// Gets table name for mapping with the type
        /// </summary>
        Dictionary<Type, string> TableNames { get; }

        /// <summary>
        ///  Gets column name for mapping with the entity's property and type
        /// </summary>
        Dictionary<(Type, string), string> ColumnName { get; }
    }
}