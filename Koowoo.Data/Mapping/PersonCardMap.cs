using System.Data.Entity.ModelConfiguration;
using Koowoo.Data.Domain;

namespace Koowoo.Data.Mapping
{
    public class PersonCardMap : KoowooEntityTypeConfiguration<PersonCardEntity>
    {
        public PersonCardMap()
        {
            ToTable("biz_PersonCard");
            Property(item => item.PersonCardUUID).HasColumnType("nvarchar").HasMaxLength(32).IsRequired();

            Property(item => item.PersonUUID).HasColumnType("nvarchar").HasMaxLength(32).IsRequired();
            Property(item => item.CardUUID).HasColumnType("nvarchar").HasMaxLength(32).IsRequired();
            Property(item => item.IDCard).HasColumnType("nvarchar").HasMaxLength(50);
			Property(item => item.CommunityUUID).HasColumnType("nvarchar").HasMaxLength(32);

            HasRequired(cc => cc.Person)
          .WithMany(cc=>cc.Cards)
          .HasForeignKey(cc => cc.PersonUUID);

            HasRequired(cc => cc.Card)
          .WithMany()
          .HasForeignKey(cc => cc.CardUUID);
        }
    }
}
