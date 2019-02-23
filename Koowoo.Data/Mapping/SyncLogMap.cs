using System.Data.Entity.ModelConfiguration;
using Koowoo.Domain;

namespace Koowoo.Data.Mapping
{
    public class SyncLogMap : KoowooEntityTypeConfiguration<SyncLogEntity>
    {
        public SyncLogMap()
        {
            ToTable("biz_SyncLog");
            HasKey(item => item.SyncId);
            Property(item => item.SyncType).HasColumnType("nvarchar").HasMaxLength(32);
            Property(item => item.ResquestXml).HasColumnType("nvarchar");	            
            Property(item => item.ResponseXml).HasColumnType("nvarchar");
            Property(item => item.CommunityId).HasColumnType("nvarchar").HasMaxLength(32);
        }
    }
}
