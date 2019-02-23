using System.Data.Entity.ModelConfiguration;
using Koowoo.Domain;

namespace Koowoo.Data.Mapping
{
    public class EntryHistoryMap : KoowooEntityTypeConfiguration<EntryHistoryEntity>
    {
        public EntryHistoryMap()
        {
            ToTable("biz_EntryHistory");
            HasKey(item => item.EntryUUID);
            Property(item => item.EntryUUID).HasColumnType("nvarchar").HasMaxLength(32);
            Property(item => item.CardUUID).HasColumnType("nvarchar").HasMaxLength(32);
            Property(item => item.PersonUUID).HasColumnType("nvarchar").HasMaxLength(32);
            Property(item => item.DoorUUID).HasColumnType("nvarchar").HasMaxLength(32);
			//Property(item => item.CommunityUUID).HasColumnType("nvarchar").HasMaxLength(32);

            HasRequired(cc => cc.Card)
             .WithMany()
             .HasForeignKey(cc => cc.CardUUID);         

            HasRequired(cc => cc.Door)
           .WithMany()
           .HasForeignKey(cc => cc.DoorUUID);

            HasRequired(cc => cc.Person)
            .WithMany(cc=> cc.EntryHistorys)
            .HasForeignKey(cc => cc.PersonUUID);
        }
    }
}
