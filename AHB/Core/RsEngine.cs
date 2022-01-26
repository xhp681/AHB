using AutoMapper;
using System.Reflection;

namespace AHB.Core
{
    public class RsEngine : IEngine
    {
        /// <summary>
        /// Get IServiceProvider
        /// </summary>
        /// <returns>IServiceProvider</returns>
        protected IServiceProvider GetServiceProvider(IServiceScope scope = null)
        {
            if (scope == null)
            {
                var accessor = ServiceProvider?.GetService<IHttpContextAccessor>();
                var context = accessor?.HttpContext;
                return context?.RequestServices ?? ServiceProvider;
            }
            return scope.ServiceProvider;
        }

        /// <summary>
        /// Run startup tasks
        /// </summary>
        protected virtual void RunStartupTasks()
        {
            //find startup tasks provided by other assemblies
            var typeFinder = Singleton<ITypeFinder>.Instance;
            var startupTasks = typeFinder.FindClassesOfType<IStartupTask>();

            //create and sort instances of startup tasks
            //we startup this interface even for not installed plugins. 
            //otherwise, DbContext initializers won't run and a plugin installation won't work
            var instances = startupTasks
                .Select(startupTask => (IStartupTask)Activator.CreateInstance(startupTask))
                .OrderBy(startupTask => startupTask.Order);

            //execute tasks
            foreach (var task in instances)
                task.ExecuteAsync().Wait();
        }

        /// <summary>
        /// Register and configure AutoMapper
        /// </summary>
        protected virtual void AddAutoMapper()
        {
            //find mapper configurations provided by other assemblies
            var typeFinder = Singleton<ITypeFinder>.Instance;
            var mapperConfigurations = typeFinder.FindClassesOfType<IOrderedMapperProfile>();

            //create and sort instances of mapper configurations
            var instances = mapperConfigurations
                .Select(mapperConfiguration => (IOrderedMapperProfile)Activator.CreateInstance(mapperConfiguration))
                .OrderBy(mapperConfiguration => mapperConfiguration.Order);

            //create AutoMapper configuration
            var config = new MapperConfiguration(cfg =>
            {
                foreach (var instance in instances)
                {
                    cfg.AddProfile(instance.GetType());
                }
            });

            //register
            AutoMapperConfiguration.Init(config);
        }

        private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            //check for assembly already loaded
            var assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.FullName == args.Name);
            if (assembly != null)
                return assembly;

            //get assembly from TypeFinder
            var typeFinder = Singleton<ITypeFinder>.Instance;
            assembly = typeFinder?.GetAssemblies().FirstOrDefault(a => a.FullName == args.Name);
            return assembly;
        }


        public void ConfigureRequestPipeline(IApplicationBuilder application)
        {
            ServiceProvider = application.ApplicationServices;

            //find startup configurations provided by other assemblies
            var typeFinder = Singleton<ITypeFinder>.Instance;
            var startupConfigurations = typeFinder.FindClassesOfType<IRsStartup>();

            //create and sort instances of startup configurations
            var instances = startupConfigurations
                .Select(startup => (IRsStartup)Activator.CreateInstance(startup))
                .OrderBy(startup => startup.Order);

            //configure request pipeline
            foreach (var instance in instances)
                instance.Configure(application);
        }

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            //register engine
            services.AddSingleton<IEngine>(this);

            //find startup configurations provided by other assemblies
            var typeFinder = Singleton<ITypeFinder>.Instance;
            var startupConfigurations = typeFinder.FindClassesOfType<IRsStartup>();

            //create and sort instances of startup configurations
            var instances = startupConfigurations
                .Select(startup => (IRsStartup)Activator.CreateInstance(startup))
                .OrderBy(startup => startup.Order);

            //configure services
            foreach (var instance in instances)
                instance.ConfigureServices(services, configuration);

            services.AddSingleton(services);

            //register mapper configurations
            AddAutoMapper();

            //run startup tasks
            RunStartupTasks();

            //resolve assemblies here. otherwise, plugins can throw an exception when rendering views
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }

        public T Resolve<T>(IServiceScope scope = null) where T : class
        {
            return (T)Resolve(typeof(T), scope);
        }

        public object Resolve(Type type, IServiceScope scope = null)
        {
            return GetServiceProvider(scope)?.GetService(type);
        }

        public virtual IEnumerable<T> ResolveAll<T>()
        {
            return (IEnumerable<T>)GetServiceProvider().GetServices(typeof(T));
        }

        public virtual object ResolveUnregistered(Type type)
        {
            Exception innerException = null;
            foreach (var constructor in type.GetConstructors())
            {
                try
                {
                    //try to resolve constructor parameters
                    var parameters = constructor.GetParameters().Select(parameter =>
                    {
                        var service = Resolve(parameter.ParameterType);
                        if (service == null)
                            throw new Exception("Unknown dependency");
                        return service;
                    });

                    //all is ok, so create instance
                    return Activator.CreateInstance(type, parameters.ToArray());
                }
                catch (Exception ex)
                {
                    innerException = ex;
                }
            }

            throw new Exception("No constructor was found that had all the dependencies satisfied.", innerException);
        }

        public virtual IServiceProvider ServiceProvider { get; protected set; }
    }
}