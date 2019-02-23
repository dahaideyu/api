using System.Data.Entity.ModelConfiguration;
using Koowoo.Domain;

namespace Koowoo.Data.Mapping
{
    public class RoomUserMap : KoowooEntityTypeConfiguration<RoomUserEntity>
    {
        public RoomUserMap()
        {
            ToTable("biz_RoomUser");
            HasKey(item => item.RoomUserUUID);

            Property(item => item.RoomUserUUID).HasColumnType("nvarchar").HasMaxLength(32);
            Property(item => item.RoomUUID).HasColumnType("nvarchar").HasMaxLength(32);
            Property(item => item.PersonUUID).HasColumnType("nvarchar").HasMaxLength(32);
		

            HasRequired(cc => cc.Person)
                .WithMany()
                .HasForeignKey(cc => cc.PersonUUID);

            HasRequired(cc => cc.Room)
                .WithMany()
                .HasForeignKey(cc => cc.RoomUUID);

            HasRequired(cc => cc.FamilyRelationDict)
               .WithMany()
               .HasForeignKey(cc => cc.FamilyRelation);
        }
    }
}
