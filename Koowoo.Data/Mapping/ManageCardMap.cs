using System.Data.Entity.ModelConfiguration;
using Koowoo.Domain;

namespace Koowoo.Data.Mapping
{
    public class ManageCardMap : KoowooEntityTypeConfiguration<ManageCardEntity>
    {
        public ManageCardMap()
        {
            ToTable("biz_ManageCard");
            HasKey(item => item.Guid);

            Property(item => item.PersonUUID).HasColumnType("nvarchar").HasMaxLength(32);
            Property(item => item.CardUUID).HasColumnType("nvarchar").HasMaxLength(32);	            
        }
    }
}
