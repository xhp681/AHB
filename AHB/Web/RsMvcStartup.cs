using AHB.Core;
using AHB.Extensions;

namespace AHB.Web
{
    public class RsMvcStartup:IRsStartup
    {
        /// <summary>
        /// Add and configure any of the middleware
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Configuration of the application</param>
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            //add WebMarkupMin services to the services container
            services.AddRsWebMarkupMin();

            //add and configure MVC feature
            services.AddRsMvc();

            services.AddWebEncoders();

            //add custom redirect result executor
            services.AddRsRedirectResultExecutor();
        }

        /// <summary>
        /// Configure the using of added middleware
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        public void Configure(IApplicationBuilder application)
        {
            //use WebMarkupMin
            application.UseRsWebMarkupMin();
        }

        /// <summary>
        /// Gets order of this startup configuration implementation
        /// </summary>
        public int Order => 1000; //MVC should be loaded last
    }
}
