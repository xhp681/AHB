using AHB.Core;

namespace AHB.Web
{
    public class BaseRouteProvider
    {
        /// <summary>
        /// Get pattern used to detect routes with language code
        /// </summary>
        /// <returns></returns>
        protected string GetLanguageRoutePattern()
        {
            var localizationSettings = EngineContext.Current.Resolve<LocalizationSettings>();
            if (localizationSettings.SeoFriendlyUrlsForLanguagesEnabled)
            {
                //this pattern is set once at the application start, when we don't have the selected language yet
                //so we use 'en' by default for the language value, later it'll be replaced with the working language code
                var code = "zh";
                return $"{{{RsPathRouteDefaults.LanguageRouteValue}:maxlength(2):{RsPathRouteDefaults.LanguageParameterTransformer}={code}}}";
            }

            return string.Empty;
        }
    }
}
