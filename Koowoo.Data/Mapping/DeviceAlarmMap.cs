using System.Data.Entity.ModelConfiguration;
using Koowoo.Domain;

namespace Koowoo.Data.Mapping
{
    public class DeviceAlarmMap : KoowooEntityTypeConfiguration<DeviceAlarmEntity>
    {
        public DeviceAlarmMap()
        {
            ToTable("biz_DeviceAlarm");
            HasKey(item => item.AlarmUUID);
            Property(item => item.AlarmUUID).HasColumnType("nvarchar").HasMaxLength(32);
			Property(item => item.DeviceUUID).HasColumnType("nvarchar").HasMaxLength(32);
            Property(item => item.Remark).HasColumnType("nvarchar").HasMaxLength(256);
            Property(item => item.CardNo).HasColumnType("nvarchar").HasMaxLength(50);
			Property(item => item.CommunityUUID).HasColumnType("nvarchar").HasMaxLength(50);

            HasRequired(cc => cc.AlarmTypeDict)
             .WithMany()
             .HasForeignKey(cc => cc.AlarmType);

            HasRequired(cc => cc.Device)
            .WithMany()
            .HasForeignKey(cc => cc.DeviceUUID);
        }
    }
}
