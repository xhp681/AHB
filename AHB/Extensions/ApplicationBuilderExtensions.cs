using AHB.Core;
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
