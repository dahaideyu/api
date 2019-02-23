using System.Data.Entity.ModelConfiguration;
using Koowoo.Domain.System;

namespace Koowoo.Data.Mapping.System
{
    public class MenuMap : KoowooEntityTypeConfiguration<MenuEntity>
    {
        public MenuMap()
        {
            ToTable("Sys_Menu");
            HasKey(item => item.MenuID);

            Property(item => item.MenuName).HasColumnType("nvarchar").HasMaxLength(50).IsRequired();
            Property(item => item.MenuUrl).HasColumnType("varchar").HasMaxLength(150);
            Property(item => item.Perms).HasColumnType("varchar").HasMaxLength(150);
            Property(item => item.MenuIcon).HasColumnType("varchar").HasMaxLength(50);

            this.HasMany(pr => pr.Roles)
               .WithMany(cr => cr.Menus)
               .Map(m =>
               {
                   m.ToTable("Sys_RoleMenu_Mapping");
                   m.MapLeftKey("MenuID");
                   m.MapRightKey("RoleID");
               });

        }
    }
}
