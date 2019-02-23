using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koowoo.Pojo
{
    public class RoomUserListDto
    {
        public string FamilyRelationName { get; set; }
        public string SexName { get; set; }
        public string PersonName { get; set; }
        public string IDCardNo { get; set; }
        public string PhoneNo { get; set; }
        public DateTime? LiveDate { get; set; }
        public DateTime? LeaveTime { get; set; }
    }
}
