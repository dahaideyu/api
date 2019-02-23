using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koowoo.Pojo
{
    public class EntryHistoryListDto
    {
    
        public string EntryUUID { get; set; }
        public string PersonUUID { get; set; }
        public string PersonPic { get; set; }
        public string PersonName { get; set; }
        public string DoorType { get; set; }
        public string DoorName { get; set; }
        public string OperationType { get; set; }
        public DateTime EntryTime { get; set; }
        public string EID { get; set; }
        public string DeviceSN { get; set; }
        public string DoorNo { get; set; }
        public string HomeName { get; set; }
        public string HomeNo { get; set; }
        public string RecordType { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string DeviceName { get; set; }
        public string DeviceType { get; set; }
        public string Photo { get; set; }
        public int? HeadNo { get; set; }
        public string DeviceNo { get; set; }
        public DateTime? Time { get; set; }
        public string Unlocked { get; set; }
        public int? UseType { get; set; }
        public string CardNo { get; set; }
        public string SixthCardNo { get; set; }
        public int? CardType { get; set; }
        public string EventId { get; set; }
        public string Token { get; set; }
    }
}
