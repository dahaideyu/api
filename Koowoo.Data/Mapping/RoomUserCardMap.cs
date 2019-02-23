using System.Data.Entity.ModelConfiguration;
using Koowoo.Domain;

namespace Koowoo.Data.Mapping
{
    public class RoomUserCardMap : KoowooEntityTypeConfiguration<RoomUserCardEntity>
    {
        public RoomUserCardMap()
        {
            ToTable("biz_RoomUserCard");
            HasKey(item => item.RoomUserCardUUID);

            Property(item => item.RoomUserCardUUID).HasColumnType("nvarchar").HasMaxLength(32);
            Property(item => item.RoomUserUUID).HasColumnType("nvarchar").HasMaxLength(32);
            Property(item => item.CardUUID).HasColumnType("nvarchar").HasMaxLength(32);
		

            HasRequired(cc => cc.RoomUser)
                .WithMany()
                .HasForeignKey(cc => cc.RoomUserUUID);

            HasRequired(cc => cc.Card)
                .WithMany()
                .HasForeignKey(cc => cc.CardUUID);
        }
    }
}
