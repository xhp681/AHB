using AHB.Core;
using System.Net;

namespace AHB.Extensions
{
    static public class ServiceCollectionExtensions
    {
        static public void ConfigureApplicationServices(this IServiceCollection services,WebApplicationBuilder builder)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.SystemDefault;
            //create default file provider
            Utils.DefaultFileProvider = new RsFileProvider(builder.Environment);

            //add accessor to HttpContext
            services.AddHttpContextAccessor();
            services.AddRazorPages();

        }
    }
}
