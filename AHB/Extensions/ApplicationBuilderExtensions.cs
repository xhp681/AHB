using AHB.Core;

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
    }
}
