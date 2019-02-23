using System.Data.Entity.ModelConfiguration;
using Koowoo.Domain;

namespace Koowoo.Data.Mapping
{
    public class AreaMap : KoowooEntityTypeConfiguration<AreaEntity>
    {
        public AreaMap()
        {
            ToTable("biz_Area");
            HasKey(item => item.AreaUUID);
            Property(item => item.AreaUUID).HasColumnType("nvarchar").HasMaxLength(32).IsRequired();
            Property(item => item.Code).HasColumnType("nvarchar").HasMaxLength(60).IsRequired();
            Property(item => item.AreaCode).HasColumnType("nvarchar").HasMaxLength(60);
            Property(item => item.ChineseName).HasColumnType("nvarchar").HasMaxLength(60);
            Property(item => item.EnglishName).HasColumnType("nvarchar").HasMaxLength(50);
            Property(item => item.Remark).HasColumnType("nvarchar").HasMaxLength(500);
            Property(item => item.KeyCode).HasColumnType("nvarchar").HasMaxLength(50);
            Property(item => item.KeyCode1).HasColumnType("nvarchar").HasMaxLength(50);
            Property(item => item.IsParent).HasColumnType("nvarchar").HasMaxLength(1);
            Property(item => item.ParentCode).HasColumnType("nvarchar").HasMaxLength(60);

            //一对多自反
            //this.HasOptional(t => t.ParentArea)
            //   .WithMany(t => t.Children)
            //   .HasForeignKey(d => d.ParentCode);

            HasRequired(cc => cc.ParentArea)
           .WithMany(t => t.Children)
           .HasForeignKey(cc => cc.ParentCode);
        }
    }
}
