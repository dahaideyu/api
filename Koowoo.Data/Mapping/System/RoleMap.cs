using System.Data.Entity.ModelConfiguration;
using Koowoo.Domain.System;

namespace Koowoo.Data.Mapping.System
{
    public class RoleMap : KoowooEntityTypeConfiguration<RoleEntity>
    {
        public RoleMap()
        {
            ToTable("Sys_Role");
            HasKey(item => item.RoleID);

            Property(item => item.RoleName).HasColumnType("nvarchar").HasMaxLength(50).IsRequired();
            Property(item => item.Description).HasColumnType("nvarchar").HasMaxLength(50).IsRequired();
        }
    }
}
