using AHB.Core;
using FluentMigrator.Runner.VersionTableInfo;

namespace AHB.DataBase
{
    public class MigrationVersionInfo : BaseEntity, IVersionTableMetaData
    {
        public MigrationVersionInfo()
        {
            TableName = nameof(MigrationVersionInfo);
            ColumnName = nameof(Version);
            DescriptionColumnName = nameof(Description);
            AppliedOnColumnName = nameof(AppliedOn);
        }

        /// <summary>
        /// Version
        /// </summary>
        public long Version { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Applied on date time
        /// </summary>
        public DateTime AppliedOn { get; set; }

        public object ApplicationContext { get; set; }

        public bool OwnsSchema => true;

        public string SchemaName => String.Empty;

        public string TableName { get; }

        public string ColumnName { get; }

        public string DescriptionColumnName { get; }

        public string UniqueIndexName { get; } = "RC_Version";

        public string AppliedOnColumnName { get; }
    }
}