using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koowoo.Pojo
{
    public class AuthAllDto
    {
        public string PersonUUID { get; set; }
        public string CardUUID { get; set; }
        public DateTime LiveDate { get; set; }
        public DateTime LeaveDate { get; set; }
        public int AuthType { get; set; }
        public string CommunityUUID { get; set; }

    }

    public class ManageCardAuthDto
    {
        public string PersonUUID { get; set; }
        public string CardUUID { get; set; }
        public List<string> DoorUUIDList { get; set; } = new List<string>();
        public DateTime LiveDate { get; set; }
        public DateTime LeaveDate { get; set; }
        public int AuthType { get; set; }
        public string CommunityUUID { get; set; }
    }

}
