using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koowoo.Domain
{
    /// <summary>
    /// 入住人员卡详情
    /// </summary>
   public class RoomUserCardEntity : BaseEntity
    {
       
        public string RoomUserCardUUID { get; set; }


        /// <summary>
        /// 入住人员ID
        /// </summary>
        public string RoomUserUUID { get; set; }

        /// <summary>
        /// 卡ID
        /// </summary>
        public string CardUUID { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

        public bool Deleted { get; set; }
        public bool SyncStatus { get; set; }
        public int SyncVersion { get; set; }

    

        public virtual CardEntity Card { get; set; }
        public virtual RoomUserEntity RoomUser { get; set; }
    }
}
