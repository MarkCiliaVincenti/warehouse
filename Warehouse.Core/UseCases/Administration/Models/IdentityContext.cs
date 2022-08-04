﻿using StackExchange.Redis;

namespace Warehouse.Core.UseCases.Administration.Models
{
    public class IdentityContext
    {
        public long? UserId { get; set; }
        public Role Role { get; set; }
        public long? ProviderId { get; set; }
    }
}
