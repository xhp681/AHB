using AHB.Core;

namespace AHB.Web
{
    public class RoutePublisher: IRoutePublisher
    {
        /// <summary>
        /// Type finder
        /// </summary>
        protected readonly ITypeFinder _typeFinder;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="typeFinder">Type finder</param>
        public RoutePublisher(ITypeFinder typeFinder)
        {
            _typeFinder = typeFinder;
        }

        /// <summary>
        /// Register routes
        /// </summary>
        /// <param name="endpointRouteBuilder">Route builder</param>
        public virtual void RegisterRoutes(IEndpointRouteBuilder endpointRouteBuilder)
        {
            //find route providers provided by other assemblies
            var routeProviders = _typeFinder.FindClassesOfType<IRouteProvider>();

            //create and sort instances of route providers
            var instances = routeProviders
                .Select(routeProvider => (IRouteProvider)Activator.CreateInstance(routeProvider))
                .OrderByDescending(routeProvider => routeProvider.Priority);

            //register all provided routes
            foreach (var routeProvider in instances)
                routeProvider.RegisterRoutes(endpointRouteBuilder);
        }
    }
}
