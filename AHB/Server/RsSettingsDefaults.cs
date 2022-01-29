using AHB.Core;

namespace AHB.Server
{
    public static partial class RsSettingsDefaults
    {
        /// <summary>
        /// Gets a key for caching
        /// </summary>
        public static CacheKey SettingsAllAsDictionaryCacheKey => new("Rs.setting.all.dictionary.", RsEntityCacheDefaults<Setting>.Prefix);
    }
}