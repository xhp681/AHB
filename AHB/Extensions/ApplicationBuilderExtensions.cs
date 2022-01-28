using AHB.Core;
using AHB.Web;
using WebMarkupMin.AspNetCore6;

namespace AHB.Extensions
{
    static public class ApplicationBuilderExtensions
    {
        static public void ConfigureRequestPipeline(this IApplicationBuilder application)
        {
            EngineContext.Current.ConfigureRequestPipeline(application);            
        }

        static public void StartEngine(this IApplicationBuilder application)
        {
            var engine = EngineContext.Current;
        }

        /// <summary>
        /// Configure Endpoints routing
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        static public void UseRsEndpoints(this IApplicationBuilder application)
        {
            //Execute the endpoint selected by the routing middleware
            application.UseEndpoints(endpoints =>
            {
                //register all routes
                EngineContext.Current.Resolve<IRoutePublisher>().RegisterRoutes(endpoints);
            });
        }


        /// <summary>
        /// Configure WebMarkupMin
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        static public void UseRsWebMarkupMin(this IApplicationBuilder application)
        {
            //check whether database is installed
            //if (!DataSettingsManager.IsDatabaseInstalled())
            //    return;

            application.UseWebMarkupMin();
        }
    }
}
