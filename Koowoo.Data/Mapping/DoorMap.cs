using System.Data.Entity.ModelConfiguration;
using Koowoo.Domain;

namespace Koowoo.Data.Mapping
{
    public class DoorMap : KoowooEntityTypeConfiguration<DoorEntity>
    {
        public DoorMap()
        {
            ToTable("biz_Door");
            HasKey(item => item.DoorUUID);
            Property(item => item.DoorUUID).HasColumnType("nvarchar").HasMaxLength(32).IsRequired();
            Property(item => item.DoorName).HasColumnType("nvarchar").HasMaxLength(50);
            Property(item => item.DeviceUUID).HasColumnType("nvarchar").HasMaxLength(32);
            Property(item => item.DoorNo).HasColumnType("nvarchar").HasMaxLength(10);
            Property(item => item.CommunityUUID).HasColumnType("nvarchar").HasMaxLength(32);
            Property(item => item.AreaUUID).HasColumnType("nvarchar").HasMaxLength(32);


            // Relationships
            HasRequired(cc => cc.DoorTypeDict)
               .WithMany()
               .HasForeignKey(cc => cc.DoorType);
          
            HasRequired(t => t.Device)
                .WithMany(t => t.Doors)
                .HasForeignKey(d => d.DeviceUUID);

            HasRequired(cc => cc.Area)
                 .WithMany()
                 .HasForeignKey(cc => cc.AreaUUID);
        }
    }
}
