using System.Data.Entity.ModelConfiguration;
using Koowoo.Domain;

namespace Koowoo.Data.Mapping
{
    public class RenterMap : KoowooEntityTypeConfiguration<RenterEntity>
    {
        public RenterMap()
        {
            ToTable("biz_Renter");
            HasKey(item => item.PersonUUID);
            Property(item => item.PersonUUID).HasColumnType("nvarchar").HasMaxLength(32).IsRequired();

            Property(item => item.EpidemicPrevention).HasColumnType("nvarchar").HasMaxLength(200);
            Property(item => item.StudyHistory).HasColumnType("nvarchar").HasMaxLength(200);
          //  Property(item => item.Inoculability).HasColumnType("nvarchar").HasMaxLength(50);
            Property(item => item.StayCard).HasColumnType("nvarchar").HasMaxLength(50);
            Property(item => item.WorkingAddress).HasColumnType("nvarchar").HasMaxLength(60);

            Property(item => item.MarriageStatus).HasColumnType("nvarchar").HasMaxLength(50);
            Property(item => item.MateName).HasColumnType("nvarchar").HasMaxLength(50);
            Property(item => item.MateNo).HasColumnType("nvarchar").HasMaxLength(20);
			Property(item => item.GestationNo).HasColumnType("nvarchar").HasMaxLength(50);
			
			Property(item => item.ContraceptionCount).HasColumnType("nvarchar").HasMaxLength(20);		
			
			Property(item => item.InsurCompany).HasColumnType("nvarchar").HasMaxLength(60);
            Property(item => item.InsurCompanyContact).HasColumnType("nvarchar").HasMaxLength(50);
            Property(item => item.ICCPhoneNo).HasColumnType("nvarchar").HasMaxLength(50);
			Property(item => item.EduHistory).HasColumnType("nvarchar").HasMaxLength(200);
			Property(item => item.School).HasColumnType("nvarchar").HasMaxLength(50);
			
			Property(item => item.RegCertNo).HasColumnType("nvarchar").HasMaxLength(40);
			Property(item => item.Remark).HasColumnType("nvarchar").HasMaxLength(200);
			Property(item => item.CommunityUUID).HasColumnType("nvarchar").HasMaxLength(32);


            HasRequired(cc => cc.PoliticalStatusDict)
            .WithMany()
            .HasForeignKey(cc => cc.PoliticalStatus);

            HasRequired(cc => cc.EduLevelDict)
          .WithMany()
          .HasForeignKey(cc => cc.EduLevel);

            HasRequired(cc => cc.ResidenceTypeDict)
          .WithMany()
          .HasForeignKey(cc => cc.ResidenceType);

            HasRequired(cc => cc.RegCertTypeDict)
          .WithMany()
          .HasForeignKey(cc => cc.RegCertType);

            HasRequired(cc => cc.RentalStatusDict)
          .WithMany()
          .HasForeignKey(cc => cc.RentalStatus);

            HasRequired(cc => cc.LivingReasonDict)
                    .WithMany()
                    .HasForeignKey(cc => cc.LivingReason);
        }
    }
}
