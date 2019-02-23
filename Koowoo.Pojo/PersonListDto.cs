using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koowoo.Pojo
{
    public class PersonListDto
    {
        public string PersonUUID { get; set; }
        public string PersonName { get; set; }
        public string IDCardNo { get; set; }

        public int Sex { get; set; }
        public string SexName { get; set; }

        public int Nation { get; set; }
        public string NationName { get; set; }

		public DateTime Birthday { get; set; }
        public string BirthdayVal { get; set; }
       

		public DateTime? ValidFrom { get; set; }
        public string ValidFromVal { get; set; }
        

        public DateTime? ValidTo { get; set; }
        public string ValidToVal { get; set; }

        public string RegAddress { get; set; }

        public int IsLocal { get; set; }
        public string IsLocalVal { get; set; }

        public bool IsRenter { get; set; }
        public string IsRenterVal { get; set; }

        public string PhoneNo { get; set; }

        public string CommunityUUID { get; set; }
        public string CommunityName { get; set; }

        public DateTime CreateTime { get; set; }
        public string CreateTimeVal { get; set; }

        //承租人关系
        public string FamilyRelationName { get; set; }
        public int? FamilyRelation { get; set; }

        //工作单位
        public string InsurCompany { get; set; }
        // 入住房间
        public string RoomName { get; set; }
		
		public string ResidenceVal { get; set; }
    }
}
