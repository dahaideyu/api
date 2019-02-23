using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koowoo.Pojo.Request
{
    public class PersonReq: QueryListReq
    {
        public string PersonName { get; set; }
        public string IDCardNo { get; set; }
        public int? Sex { get; set; }
        public string PhoneNo { get; set; }
        public string RegAddress { get; set; }
        public bool IsRenter { get; set; }
        public string ICCardNo { get; set; }
        public string IDCardInternalNO { get; set; }
        public DateTime? BirthDay { get; set; }
        public string AreaUUID { get; set; }

        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }

        public List<string> PersonIds { get; set; }
    }
}
