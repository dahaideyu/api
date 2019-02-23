using System.Data.Entity.ModelConfiguration;
using Koowoo.Domain.System;
namespace Koowoo.Data.Mapping.System
{
    public class EvevatorConfigMap : KoowooEntityTypeConfiguration<EvevatorConfigEntity>
    {
        public EvevatorConfigMap()
        {
            ToTable("sys_EvevatorConfig");
            HasKey(item => item.Id);

            Property(item => item.EvevatorSN ).HasColumnType("nvarchar").HasMaxLength(50);
            Property(item => item.CommunityUUID).HasColumnType("nvarchar").HasMaxLength(32); 
        }
    }
}
