using FluentMigrator;
using FluentMigrator.Expressions;
using FluentMigrator.Infrastructure;

namespace AHB.DataBase
{
    public class NullMigrationContext : IMigrationContext
    {
        public IServiceProvider ServiceProvider { get; set; }

        public ICollection<IMigrationExpression> Expressions { get; set; } = new List<IMigrationExpression>();

        public IQuerySchema QuerySchema { get; set; }
#pragma warning disable 612
        public IAssemblyCollection MigrationAssemblies { get; set; }
        public object ApplicationContext { get; set; }
#pragma warning restore 612
        public string Connection { get; set; }
    }
}