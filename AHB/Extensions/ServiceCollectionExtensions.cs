using System.Net;

namespace AHB.Extensions
{
    static public class ServiceCollectionExtensions
    {
        static public void ConfigureApplicationServices(this IServiceCollection services,WebApplicationBuilder builder)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.SystemDefault;
            services.AddRazorPages();

        }
    }
}
