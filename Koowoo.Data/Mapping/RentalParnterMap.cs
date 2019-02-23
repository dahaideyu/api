using System.Data.Entity.ModelConfiguration;
using Koowoo.Entity;

namespace Koowoo.Data.Mapping
{
    public class RentalParnterMap : KoowooEntityTypeConfiguration<RentalParnterEntity>
    {
        public RentalParnterMap()
        {
            ToTable("biz_RentalParnter");
           
            Property(item => item.RoomUUID).HasColumnType("nvarchar").HasMaxLength(32);
            Property(item => item.PersonUUID).HasColumnType("nvarchar").HasMaxLength(32);
            Property(item => item.Parm1).HasColumnType("nvarchar").HasMaxLength(50);
            Property(item => item.Parm2).HasColumnType("nvarchar").HasMaxLength(50);
            Property(item => item.crk_uuid).HasColumnType("nvarchar").HasMaxLength(50);
            Property(item => item.crk_crkno).HasColumnType("nvarchar").HasMaxLength(50);
			Property(item => item.CommunityUUID).HasColumnType("nvarchar").HasMaxLength(50);
        }
    }
}
