﻿using Vayosoft.Core.SharedKernel.Entities;

namespace Warehouse.Core.UseCases.Administration.Models
{
    public class SecurityObjectEntity : EntityBase<string>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class SecurityRoleEntity : EntityBase<string>
    {
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public ulong? ProviderId { get; set; }
    }

    public class SecurityRolePermissionsEntity : EntityBase<string>
    {
        public virtual string RoleId { get; set; }
        public virtual string ObjectId { get; set; }
        public virtual WarehousePermissions Permissions { get; set; }
    }

    public class UserRoleEntity : EntityBase<string>
    {
        public ulong UserId { get; set; }
        public string RoleId { get; set; }
    }

    [Flags]
    public enum WarehousePermissions
    {
        None = 0,
        View = 1,
        Add = 2,
        Edit = 4,
        Delete = 8,
        Execute = 16,
        Grant = 32
    }
}
