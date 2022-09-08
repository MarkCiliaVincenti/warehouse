﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Vayosoft.Core.SharedKernel.ValueObjects;
using Vayosoft.Data.EF.MySQL;
using Warehouse.Core.Entities.Models;

namespace Warehouse.Infrastructure.Persistence.Mapping
{
    public partial class DeviceEntityMap : EntityConfigurationMapper<DeviceEntity>
    {
        public override void Configure(EntityTypeBuilder<DeviceEntity> builder)
        {
            builder.ToTable("devices").HasKey(t => t.Id);
            builder.Property(t => t.Id).HasColumnName("deviceid").ValueGeneratedOnAdd();
            builder.Property(t => t.MacAddress).HasColumnName("mac_address");
                //.HasConversion<MacAddressConverter>();
            builder.Property(t => t.ProviderId).HasColumnName("providerid");

        }
    }

    public class MacAddressConverter : ValueConverter<MacAddress, string>
    {
        public MacAddressConverter()
            : base(                v => v.ToString(),
                v => MacAddress.Create(v))
        {
        }
    }
}
