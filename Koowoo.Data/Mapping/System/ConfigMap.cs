using System.Data.Entity.ModelConfiguration;
using Koowoo.Domain.System;

namespace Koowoo.Data.Mapping.System
{
    public class ConfigMap : KoowooEntityTypeConfiguration<ConfigEntity>
    {
        public ConfigMap()
        {
            ToTable("Sys_Config");
            HasKey(item => item.ConfigID);

            Property(item => item.ConfigKey).HasColumnType("varchar").HasMaxLength(50);
            Property(item => item.ConfigName).HasColumnType("nvarchar").HasMaxLength(50).IsRequired();
            Property(item => item.ConfigValue).HasColumnType("nvarchar").HasMaxLength(200);
			Property(item => item.CommunityUUID).HasColumnType("nvarchar").HasMaxLength(32);
        }
    }
}
