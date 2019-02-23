using System.Data.Entity.ModelConfiguration;
using Koowoo.Domain;

namespace Koowoo.Data.Mapping
{
    public class PersonMap : KoowooEntityTypeConfiguration<PersonEntity>
    {
        public PersonMap()
        {
            ToTable("biz_Person");
            HasKey(item => item.PersonUUID);
            Property(item => item.PersonUUID).HasColumnType("nvarchar").HasMaxLength(32);
            Property(item => item.PersonName).HasColumnType("nvarchar").HasMaxLength(50);
            Property(item => item.EnglishName).HasColumnType("nvarchar").HasMaxLength(20);
            Property(item => item.IDCardNo).HasColumnType("nvarchar").HasMaxLength(50);
            Property(item => item.IssueOrg).HasColumnType("nvarchar").HasMaxLength(100);
            Property(item => item.RegAddress).HasColumnType("nvarchar").HasMaxLength(100);
            Property(item => item.OriginRegAddress).HasColumnType("nvarchar").HasMaxLength(100);
            Property(item => item.LivingAddress).HasColumnType("nvarchar").HasMaxLength(100);
            Property(item => item.PhoneNo).HasColumnType("nvarchar").HasMaxLength(50);
            Property(item => item.BackUpPhoneNo).HasColumnType("nvarchar").HasMaxLength(50);
            Property(item => item.IDCardPic).HasColumnType("nvarchar").HasMaxLength(100);
			Property(item => item.CommunityUUID).HasColumnType("nvarchar").HasMaxLength(32);

            HasRequired(cc => cc.SexDict)
              .WithMany()
              .HasForeignKey(cc => cc.Sex);

            HasRequired(cc => cc.NationDict)
            .WithMany()
            .HasForeignKey(cc => cc.Nation);

            HasRequired(cc => cc.IDDocumentTypeDict)
          .WithMany()
          .HasForeignKey(cc => cc.IDDocumentType);

            HasRequired(cc => cc.PersonTypeDict)
          .WithMany()
          .HasForeignKey(cc => cc.PersonType);
        }
    }
}
