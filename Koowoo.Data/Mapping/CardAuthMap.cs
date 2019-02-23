using System.Data.Entity.ModelConfiguration;
using Koowoo.Domain;

namespace Koowoo.Data.Mapping
{
    public class CardAuthMap : KoowooEntityTypeConfiguration<CardAuthEntity>
    {
        public CardAuthMap()
        {
            ToTable("biz_CardAuth");
            HasKey(item => item.CardAuthUUID);
            Property(item => item.CardAuthUUID).HasColumnType("nvarchar").HasMaxLength(32);
            Property(item => item.AuthType).HasColumnType("nvarchar").HasMaxLength(32);
            Property(item => item.CardUUID).HasColumnType("nvarchar").HasMaxLength(32);
            Property(item => item.DoorUUID).HasColumnType("nvarchar").HasMaxLength(32);

            HasRequired(cc => cc.Card)
            .WithMany()
            .HasForeignKey(cc => cc.CardUUID);

            HasRequired(cc => cc.Door)
            .WithMany()
            .HasForeignKey(cc => cc.DoorUUID);
        }
    }
}
