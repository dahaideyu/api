using System.Data.Entity.ModelConfiguration;
using Koowoo.Domain.System;
namespace Koowoo.Data.Mapping.System
{
    public class DictMap : KoowooEntityTypeConfiguration<DictEntity>
    {
        public DictMap()
        {
            ToTable("Sys_Dict");
            HasKey(item => item.DictID);

            Property(item => item.DictType).HasColumnType("nvarchar").HasMaxLength(50);
            Property(item => item.DictName).HasColumnType("nvarchar").HasMaxLength(50);
            Property(item => item.DictCode).HasColumnType("nvarchar").HasMaxLength(10);
        }
    }
}
