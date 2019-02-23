using System.Data.Entity.ModelConfiguration;
using Koowoo.Domain;

namespace Koowoo.Data.Mapping
{
    public class RentalContractMap : KoowooEntityTypeConfiguration<RentalContractEntity>
    {
        public RentalContractMap()
        {
            ToTable("biz_RentalContract");
            HasKey(item => item.ContractUUID);
            Property(item => item.ContractUUID).HasColumnType("nvarchar").HasMaxLength(32).IsRequired();
            Property(item => item.PersonUUID).HasColumnType("nvarchar").HasMaxLength(32);
            Property(item => item.RoomUUID).HasColumnType("nvarchar").HasMaxLength(32);
			Property(item => item.CommunityUUID).HasColumnType("nvarchar").HasMaxLength(32);

            HasRequired(cc => cc.Person)
              .WithMany(cc=>cc.RentalContracts)
              .HasForeignKey(cc => cc.PersonUUID);

            HasRequired(cc => cc.Room)
              .WithMany()
              .HasForeignKey(cc => cc.RoomUUID);

            HasRequired(cc => cc.RentalStautsDict)
             .WithMany()
             .HasForeignKey(cc => cc.RentalStauts);
        }
    }
}
