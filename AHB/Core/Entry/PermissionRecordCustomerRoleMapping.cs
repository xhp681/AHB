﻿namespace AHB.Core
{
    public class PermissionRecordCustomerRoleMapping: BaseEntity
    {
        /// <summary>
        /// Gets or sets the permission record identifier
        /// </summary>
        public int PermissionRecordId { get; set; }

        /// <summary>
        /// Gets or sets the customer role identifier
        /// </summary>
        public int CustomerRoleId { get; set; }
    }
}
