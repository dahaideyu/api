using System.Data.Entity.ModelConfiguration;
using Koowoo.Domain;

namespace Koowoo.Data.Mapping
{
    public class DeviceStatusMap : KoowooEntityTypeConfiguration<DeviceStatusEntity>
    {
        public DeviceStatusMap()
        {
            ToTable("biz_DeviceStatus");
            HasKey(item => item.DeviceUUID);
            Property(item => item.DeviceUUID).HasColumnType("nvarchar").HasMaxLength(32).IsRequired();
            Property(item => item.HardwardVersion).HasColumnType("nvarchar").HasMaxLength(50);
            Property(item => item.SoftwareVersion).HasColumnType("nvarchar").HasMaxLength(50);
            Property(item => item.IMSI).HasColumnType("nvarchar").HasMaxLength(50);
            Property(item => item.MISSDN).HasColumnType("nvarchar").HasMaxLength(50);
            Property(item => item.WorkMode).HasColumnType("nvarchar").HasMaxLength(2);
            Property(item => item.PowerMode).HasColumnType("nvarchar").HasMaxLength(50);

            //this.HasRequired(t => t.Device)
            //   .WithOptional(t => t.DeviceStatus);
            
            ///一对一
            this.HasRequired(t => t.Device)
               .WithRequiredDependent(t => t.DeviceStatus);
        }
    }
}
