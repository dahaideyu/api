using System.Data.Entity.ModelConfiguration;
using Koowoo.Domain.System;

namespace Koowoo.Data.Mapping.System
{
   public class UserMap : KoowooEntityTypeConfiguration<UserEntity>
    {
        public UserMap()
        {
            ToTable("Sys_User");
            HasKey(item => item.UserID);

            Property(item => item.UserName).HasColumnType("nvarchar").HasMaxLength(20).IsRequired();
            Property(item => item.Password).HasColumnType("varchar").HasMaxLength(50).IsRequired();
            Property(item => item.Salt).HasColumnType("varchar").HasMaxLength(10).IsRequired();
            Property(item => item.Mobile).HasColumnType("varchar").HasMaxLength(13);
            Property(item => item.LastLoginIP).HasColumnType("varchar").HasMaxLength(30);
            Property(item => item.Email).HasColumnType("varchar").HasMaxLength(250);
            Property(item => item.Remark).HasColumnType("nvarchar").HasMaxLength(250);
            Property(item => item.CommunityUUID).HasColumnType("nvarchar").HasMaxLength(32);

            this.HasMany(c => c.Roles)
                .WithMany()
                 .Map(m =>
                 {
                     m.ToTable("Sys_UserRole_Mapping");
                     m.MapLeftKey("UserID");
                     m.MapRightKey("RoleID");
                 });
        }
    }
}
