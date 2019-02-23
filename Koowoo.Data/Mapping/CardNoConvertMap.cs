using System.Data.Entity.ModelConfiguration;
using Koowoo.Domain;

namespace Koowoo.Data.Mapping
{
    public class CardNoConvertMap : KoowooEntityTypeConfiguration<CardNoConvertEntity>
    {
        public CardNoConvertMap()
        {
            ToTable("biz_CardNoConvert");
            HasKey(item => item.CardNOConverUUID);
            Property(item => item.CardNOConverUUID).HasColumnType("nvarchar").HasMaxLength(32);
            Property(item => item.CardUUID).HasColumnType("nvarchar").HasMaxLength(32);
            Property(item => item.CardConvertNo).HasColumnType("nvarchar").HasMaxLength(50);


            HasRequired(cc => cc.Card)
            .WithMany()
            .HasForeignKey(cc => cc.CardUUID);

            HasRequired(cc => cc.CardConvertTypeDict)
            .WithMany()
            .HasForeignKey(cc => cc.CardConvertType);
        }
    }
}
