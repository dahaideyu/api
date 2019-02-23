using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koowoo.Domain
{
    public class SyncLogEntity:BaseEntity
    {
        public string SyncId { get; set; }
        public string SyncType { get; set; }
        public string ResquestXml { get; set; }
        public string ResponseXml { get; set; }
        public int SyncResult { get; set; }
        public DateTime SyncTime { get; set; }
        public string CommunityId { get; set; }
    }
}
