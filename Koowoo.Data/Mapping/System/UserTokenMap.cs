using System.Data.Entity.ModelConfiguration;
using Koowoo.Domain.System;

namespace Koowoo.Data.Mapping.System
{
    public class UserTokenMap : KoowooEntityTypeConfiguration<UserTokenEntity>
    {
        public UserTokenMap()
        {
            ToTable("Sys_UserToken");
            HasKey(item => item.Token);
            Property(item => item.Token).HasColumnType("varchar").HasMaxLength(36);
        }
    }
}
