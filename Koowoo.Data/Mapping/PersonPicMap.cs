using System.Data.Entity.ModelConfiguration;
using Koowoo.Domain;

namespace Koowoo.Data.Mapping
{
    public class PersonPicMap : KoowooEntityTypeConfiguration<PersonPicEntity>
    {
        public PersonPicMap()
        {
            ToTable("biz_PersonPic");
            HasKey(item => item.PersonPicUUID);
            Property(item => item.PersonPicUUID).HasColumnType("nvarchar").HasMaxLength(32);
            Property(item => item.PersonUUID).HasColumnType("nvarchar").HasMaxLength(32);
            Property(item => item.PersonPic).HasColumnType("nvarchar").HasMaxLength(100);
			Property(item => item.CommunityUUID).HasColumnType("nvarchar").HasMaxLength(32);

            HasRequired(cc => cc.Person)
                .WithMany(cc=>cc.PersonPics)
                .HasForeignKey(cc => cc.PersonUUID);

            HasRequired(cc => cc.PersonPicTypeDict)
                .WithMany()
                .HasForeignKey(cc => cc.PersonPicType);
        }
    }
}
