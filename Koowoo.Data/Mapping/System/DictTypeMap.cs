using System.Data.Entity.ModelConfiguration;
using Koowoo.Domain.System;

namespace Koowoo.Data.Mapping.System
{

    public class DictTypeMap : KoowooEntityTypeConfiguration<DictTypeEntity>
    {
        public DictTypeMap()
        {
            ToTable("Sys_DictType");
            HasKey(item => item.DictTypeCode);
            Property(item => item.DictTypeCode).HasColumnType("nvarchar").HasMaxLength(50);
            Property(item => item.DictTypeName).HasColumnType("nvarchar").HasMaxLength(50);
        }
    }
}
