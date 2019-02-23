using System.Data.Entity.ModelConfiguration;
using Koowoo.Domain;

namespace Koowoo.Data.Mapping
{
    public class RoomMap : KoowooEntityTypeConfiguration<RoomEntity>
    {
        public RoomMap()
        {
            ToTable("biz_Room");
            HasKey(item => item.RoomUUID);
            Property(item => item.RoomUUID).HasColumnType("nvarchar").HasMaxLength(32).IsRequired();
            Property(item => item.RoomName).HasColumnType("nvarchar").HasMaxLength(50);
            Property(item => item.RoomFullName).HasColumnType("nvarchar").HasMaxLength(200);

            Property(item => item.OtherCode).HasColumnType("nvarchar").HasMaxLength(50);
            Property(item => item.Address).HasColumnType("nvarchar").HasMaxLength(256);
            Property(item => item.CertificationNo).HasColumnType("nvarchar").HasMaxLength(50);
            Property(item => item.PoliceStation).HasColumnType("nvarchar").HasMaxLength(128);
            Property(item => item.Policeman).HasColumnType("nvarchar").HasMaxLength(50);
            Property(item => item.RentalUseage).HasColumnType("nvarchar").HasMaxLength(200);
            Property(item => item.Registration).HasColumnType("nvarchar").HasMaxLength(128);
            Property(item => item.HomeNo).HasColumnType("nvarchar").HasMaxLength(20);
            Property(item => item.RentalInfo).HasColumnType("ntext");


            Property(item => item.AreaUUID).HasColumnType("nvarchar").HasMaxLength(32);
            Property(item => item.CommunityUUID).HasColumnType("nvarchar").HasMaxLength(32);

            HasRequired(cc => cc.ResidenceTypeDict)
            .WithMany()
            .HasForeignKey(cc => cc.ResidenceType);

            HasRequired(cc => cc.RoomTypeDict)
            .WithMany()
            .HasForeignKey(cc => cc.RoomType);

            HasRequired(cc => cc.OwnershipTypeDict)
            .WithMany()
            .HasForeignKey(cc => cc.OwnershipType);       

            HasRequired(cc => cc.NatureDict)
           .WithMany()
           .HasForeignKey(cc => cc.Nature);

            HasRequired(cc => cc.ManageLevelDict)
            .WithMany()
            .HasForeignKey(cc => cc.ManageLevel);

            HasRequired(cc => cc.RentalTypeDict)
            .WithMany()
            .HasForeignKey(cc => cc.RentalType);


            HasRequired(cc => cc.RoomStyleDict)
             .WithMany()
             .HasForeignKey(cc => cc.RoomStyle);

            HasRequired(cc => cc.Area)
            .WithMany()
            .HasForeignKey(cc => cc.AreaUUID);
        }
    }
}
