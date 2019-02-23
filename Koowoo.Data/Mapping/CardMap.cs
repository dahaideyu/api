using System.Data.Entity.ModelConfiguration;
using Koowoo.Domain;

namespace Koowoo.Data.Mapping
{
    public class CardMap : KoowooEntityTypeConfiguration<CardEntity>
    {
        public CardMap()
        {
            ToTable("biz_Card");
            HasKey(item => item.CardUUID);
            Property(item => item.CardUUID).HasColumnType("nvarchar").HasMaxLength(32).IsRequired();
            Property(item => item.CardNo).HasColumnType("nvarchar").HasMaxLength(50).IsRequired();
            Property(item => item.AreaKey).HasColumnType("nvarchar").HasMaxLength(8);
            Property(item => item.AreaKey1).HasColumnType("nvarchar").HasMaxLength(8);
            Property(item => item.CardLast4NO).HasColumnType("nvarchar").HasMaxLength(32);
            Property(item => item.CommunityUUID).HasColumnType("nvarchar").HasMaxLength(32);

            HasRequired(cc => cc.CardTypeDict)
           .WithMany()
           .HasForeignKey(cc => cc.CardType);

            HasRequired(cc => cc.CardTypeOfLKDict)
         .WithMany()
         .HasForeignKey(cc => cc.CardTypeOfLK);
        }
    }
}
