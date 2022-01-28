using AHB.Core;
using AHB.Web;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Newtonsoft.Json.Serialization;
using System.Net;
using WebMarkupMin.AspNetCore6;
using WebMarkupMin.NUglify;

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


            var typeFinder = new WebAppTypeFinder();
            Singleton<ITypeFinder>.Instance = typeFinder;
            services.AddSingleton<ITypeFinder>(typeFinder);

            //add configuration parameters
            var configurations = typeFinder
                .FindClassesOfType<IConfig>()
                .Select(configType => (IConfig)Activator.CreateInstance(configType))
                .ToList();
            foreach (var config in configurations)
            {
                builder.Configuration.GetSection(config.Name).Bind(config, options => options.BindNonPublicProperties = true);
            }
            var appSettings = AppSettingsHelper.SaveAppSettings(configurations, Utils.DefaultFileProvider, false);
            services.AddSingleton(appSettings);

            //create engine and configure service provider
            var engine = EngineContext.Create();

            engine.ConfigureServices(services, builder.Configuration);

        }

        /// <summary>
        /// Add and configure WebMarkupMin service
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        static public void AddRsWebMarkupMin(this IServiceCollection services)
        {
            //check whether database is installed
            //if (!DataSettingsManager.IsDatabaseInstalled())
            //    return;

            services
                .AddWebMarkupMin(options =>
                {
                    options.AllowMinificationInDevelopmentEnvironment = true;
                    options.AllowCompressionInDevelopmentEnvironment = true;
                    options.DisableMinification = !EngineContext.Current.Resolve<CommonSettings>().EnableHtmlMinification;
                    options.DisableCompression = true;
                    options.DisablePoweredByHttpHeaders = true;
                })
                .AddHtmlMinification(options =>
                {
                    options.CssMinifierFactory = new NUglifyCssMinifierFactory();
                    options.JsMinifierFactory = new NUglifyJsMinifierFactory();
                })
                .AddXmlMinification(options =>
                {
                    var settings = options.MinificationSettings;
                    settings.RenderEmptyTagsWithSpace = true;
                    settings.CollapseTagsWithoutContent = true;
                });
        }

        /// <summary>
        /// Add and configure MVC for the application
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <returns>A builder for configuring MVC services</returns>
        static public IMvcBuilder AddRsMvc(this IServiceCollection services)
        {
            //add basic MVC feature
            var mvcBuilder = services.AddControllersWithViews();

            mvcBuilder.AddRazorRuntimeCompilation();

            var appSettings = Singleton<AppSettings>.Instance;
            if (appSettings.Get<CommonConfig>().UseSessionStateTempDataProvider)
            {
                //use session-based temp data provider
                mvcBuilder.AddSessionStateTempDataProvider();
            }
            else
            {
                //use cookie-based temp data provider
                mvcBuilder.AddCookieTempDataProvider(options =>
                {
                    options.Cookie.Name = $"{RsCookieDefaults.Prefix}{RsCookieDefaults.TempDataCookie}";
                    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                });
            }

            services.AddRazorPages();

            //MVC now serializes JSON with camel case names by default, use this code to avoid it
            mvcBuilder.AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());

            //set some options
            mvcBuilder.AddMvcOptions(options =>
            {
                //we'll use this until https://github.com/dotnet/aspnetcore/issues/6566 is solved 
                options.ModelBinderProviders.Insert(0, new InvariantNumberModelBinderProvider());
                //add custom display metadata provider 
                options.ModelMetadataDetailsProviders.Add(new RsMetadataProvider());

                //in .NET model binding for a non-nullable property may fail with an error message "The value '' is invalid"
                //here we set the locale name as the message, we'll replace it with the actual one later when not-null validation failed
                options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(_ => RsValidationDefaults.NotNullValidationLocaleName);
            });

            //add fluent validation
            mvcBuilder.AddFluentValidation(configuration =>
            {
                //register all available validators from Nop assemblies
                var assemblies = mvcBuilder.PartManager.ApplicationParts
                    .OfType<AssemblyPart>()
                    .Where(part => part.Name.StartsWith("Rs", StringComparison.InvariantCultureIgnoreCase))
                    .Select(part => part.Assembly);
                configuration.RegisterValidatorsFromAssemblies(assemblies);

                //implicit/automatic validation of child properties
                configuration.ImplicitlyValidateChildProperties = true;
            });

            //register controllers as services, it'll allow to override them
            mvcBuilder.AddControllersAsServices();

            return mvcBuilder;
        }


        /// <summary>
        /// Register custom RedirectResultExecutor
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddRsRedirectResultExecutor(this IServiceCollection services)
        {
            //we use custom redirect executor as a workaround to allow using non-ASCII characters in redirect URLs
            services.AddScoped<IActionResultExecutor<RedirectResult>, RsRedirectResultExecutor>();
        }

    }
}
