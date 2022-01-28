namespace AHB.Web
{
    public static partial class RsValidationDefaults
    {
        /// <summary>
        /// Gets the name of a rule set used to validate model
        /// </summary>
        public static string ValidationRuleSet => "Validate";

        /// <summary>
        /// Gets the name of a locale used in not-null validation
        /// </summary>
        public static string NotNullValidationLocaleName => "Admin.Common.Validation.NotEmpty";
    }
}
