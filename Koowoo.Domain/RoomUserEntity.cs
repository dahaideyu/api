using Koowoo.Domain.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koowoo.Domain
{
    /// <summary>
    /// 承租信息表
    /// </summary>
   public class RoomUserEntity:BaseEntity
    {
        public string RoomUserUUID { get; set; }
        
        /// <summary>
        /// 承租房间ID
        /// </summary>
        public string RoomUUID { get; set; }

        /// <summary>
        /// 人员ID
        /// </summary>
        public string PersonUUID { get; set; }
        
        /// <summary>
        /// 入住日期
        /// </summary>
        public DateTime? LiveDate { get; set; }

        /// <summary>
        /// 离开日期
        /// </summary>
        public DateTime? LeaveDate { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 与承租人关系，字典值
        /// </summary>
        public int? FamilyRelation { get; set; }
        public int Status { get; set; }
        public bool Deleted { get; set; }
        public bool SyncStatus { get; set; }
        public int SyncVersion { get; set; }

        /// <summary>
        /// 关联社区ID
        /// </summary>
        public string CommunityUUID { get; set; }

        public virtual DictEntity FamilyRelationDict { get; set; }
        public virtual RoomEntity Room { get; set; }
        public virtual PersonEntity Person { get; set; }
    }
}
