using System.Data.Entity.ModelConfiguration;
using Koowoo.Domain;

namespace Koowoo.Data.Mapping
{
    public class DeviceMap : KoowooEntityTypeConfiguration<DeviceEntity>
    {
        public DeviceMap()
        {
            ToTable("biz_Device");
            HasKey(item => item.DeviceUUID);
            Property(item => item.DeviceUUID).HasColumnType("nvarchar").HasMaxLength(32).IsRequired();
            Property(item => item.DeviceName).HasColumnType("nvarchar").HasMaxLength(64);
            Property(item => item.SNNumber).HasColumnType("nvarchar").HasMaxLength(50);
            Property(item => item.Producer).HasColumnType("nvarchar").HasMaxLength(200);
            Property(item => item.IPAddress).HasColumnType("nvarchar").HasMaxLength(20);
            Property(item => item.Mac).HasColumnType("nvarchar").HasMaxLength(17);
            Property(item => item.Port).HasColumnType("nvarchar").HasMaxLength(10);          
            Property(item => item.InnerKey).HasColumnType("nvarchar").HasMaxLength(50);
            Property(item => item.CommunityUUID).HasColumnType("nvarchar").HasMaxLength(32);

            HasRequired(cc => cc.DeviceTypeDict)
           .WithMany()
           .HasForeignKey(cc => cc.DeviceType);

            HasRequired(cc => cc.LockTypeDict)
              .WithMany()
              .HasForeignKey(cc => cc.LockType);

            HasRequired(cc => cc.TransferTypeDict)
                  .WithMany()
                  .HasForeignKey(cc => cc.TransferType);

            HasRequired(cc => cc.CardConvertTypeDict)
                 .WithMany()
                 .HasForeignKey(cc => cc.CardConvertType);

        }
    }
}
