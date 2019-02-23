using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koowoo.Pojo.Request
{
    public class EntryListReq:QueryListReq
    {
        public string personId { get; set; }
        public string DoorUUID { get; set; }
        public string PersonName { get; set; }
        public string CardNo { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
}
