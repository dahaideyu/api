using System.Data.Entity.ModelConfiguration;
using Koowoo.Domain;

namespace Koowoo.Data.Mapping
{
    public class FingerPrintMap : KoowooEntityTypeConfiguration<FingerPrintEntity>
    {
        public FingerPrintMap()
        {
            ToTable("biz_FingerPrint");
            HasKey(item => item.FingerPrintUUID);

            Property(item => item.FingerPrintUUID).HasColumnType("nvarchar").HasMaxLength(32);
            Property(item => item.PersonUUID).HasColumnType("nvarchar").HasMaxLength(32);
            Property(item => item.FingerPrintPic).HasColumnType("nvarchar").HasMaxLength(100);
			Property(item => item.CommunityUUID).HasColumnType("nvarchar").HasMaxLength(32);

            HasRequired(cc => cc.Person)
                .WithMany(cc=>cc.FingerPrints)
                .HasForeignKey(cc => cc.PersonUUID);

            HasRequired(cc => cc.FingerPrintTypeDict)
                .WithMany()
                .HasForeignKey(cc => cc.FingerPrintType);
        }
    }
}
