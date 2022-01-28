﻿using AHB.Core;
using AHB.Extensions;

namespace AHB.Web
{
    public class RoutingStartup: IRsStartup
    {
        /// <summary>
        /// Add and configure any of the middleware
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Configuration of the application</param>
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            //add MiniProfiler services
            services.AddRsMiniProfiler();
        }

        /// <summary>
        /// Configure the using of added middleware
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        public void Configure(IApplicationBuilder application)
        {
            //use MiniProfiler must come before UseNopEndpoints
            application.UseMiniProfiler();

            //Add the RoutingMiddleware
            application.UseRouting();

            //Endpoints routing
            application.UseRsEndpoints();
        }

        /// <summary>
        /// Gets order of this startup configuration implementation
        /// </summary>
        public int Order => 400; // Routing should be loaded before authentication
    }
}
