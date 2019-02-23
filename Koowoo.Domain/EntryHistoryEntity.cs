using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koowoo.Domain
{
    public class EntryHistoryEntity:BaseEntity
    {
        public string CommunityId { get; set; }
        public string EntryUUID { get; set; }
        /// <summary>
        /// CardID
        /// </summary>
        public string CardUUID { get; set; }

        /// <summary>
        /// PersonID
        /// </summary>
        public string PersonUUID { get; set; }

        /// <summary>
        /// 门ID
        /// </summary>
        public string DoorUUID { get; set; }

        /// <summary>
        /// 打开方式，数据字典
        /// </summary>
        public int OperationType { get; set; }
  

        public string Photo { get; set; }


        /// <summary>
        /// 开门时间
        /// </summary>
        public DateTime EntryTime { get; set; }

        /// <summary>
        /// 写入数据库时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 社区关联ID
        /// </summary>
       // public string CommunityUUID { get; set; }
        public bool Deleted { get; set; }

        public bool SyncStatus { get; set; }
        public int SyncVersion { get; set; }

        public virtual CardEntity Card { get; set; }
        public virtual PersonEntity Person { get; set; }
        public virtual DoorEntity Door { get; set; }
    }
}
