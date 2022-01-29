namespace AHB.DataBase
{
    public interface IMappingEntityAccessor
    {
        /// <summary>
        /// Returns mapped entity descriptor
        /// </summary>
        /// <param name="entityType">Type of entity</param>
        /// <returns>Mapped entity descriptor</returns>
        RsEntityDescriptor GetEntityDescriptor(Type entityType);
    }
}