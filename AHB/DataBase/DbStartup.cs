using AHB.Core;
using FluentMigrator;
using FluentMigrator.Runner;

namespace AHB.DataBase
{
    public class DbStartup : IRsStartup
    {
        public int Order => 10;

        public void Configure(IApplicationBuilder application)
        {
            
        }

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            var typeFinder = Singleton<ITypeFinder>.Instance;
            var mAssemblies = typeFinder.FindClassesOfType<MigrationBase>()
                .Select(t => t.Assembly)
                .Where(assembly => !assembly.FullName.Contains("FluentMigrator.Runner"))
                .Distinct()
                .ToArray();

            services
                //// add common FluentMigrator services
                //.AddFluentMigratorCore()
                //.AddScoped<IProcessorAccessor, NopProcessorAccessor>()
                //// set accessor for the connection string
                //.AddScoped<IConnectionStringAccessor>(x => DataSettingsManager.LoadSettings())
                //.AddSingleton<IMigrationManager, MigrationManager>()
                //.AddSingleton<IConventionSet, NopConventionSet>()
                //.AddTransient<IMappingEntityAccessor>(x => x.GetRequiredService<IDataProviderManager>().DataProvider)
                .ConfigureRunner(rb =>
                    rb.WithVersionTable(new MigrationVersionInfo()).AddSqlServer().AddMySql5().AddPostgres()
                        // define the assembly containing the migrations
                        .ScanIn(mAssemblies).For.Migrations());
        }
    }
}
