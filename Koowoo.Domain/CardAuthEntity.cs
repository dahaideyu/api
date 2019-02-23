using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koowoo.Domain
{
    /// <summary>
    /// 出入卡权限信息
    /// </summary>
    public class CardAuthEntity:BaseEntity
    {

        public CardAuthEntity()
        {
            Deleted = false;
            SyncStatus = false;
            SyncVersion = 0;
        }


        public string CardAuthUUID { get; set; }
        /// <summary>
        /// 卡ID
        /// </summary>
        public string CardUUID { get; set; }

       // public string DeviceUUID { get; set; }

        /// <summary>
        /// 门ID
        /// </summary>
        public string DoorUUID { get; set; }
        public string AuthType { get; set; }

        /// <summary>
        /// 有效期开始时间
        /// </summary>
        public DateTime ValidFrom { get; set; }

        /// <summary>
        /// 有效期结束时间
        /// </summary>
        public DateTime ValidTo { get; set; }

        /// <summary>
        /// 添加人
        /// </summary>
        public int CreateBy { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

        public virtual CardEntity Card { get; set; }
        public virtual DoorEntity Door { get; set; }
        public bool Deleted { get; set; }
        public bool SyncStatus { get; set; }
        public int SyncVersion { get; set; }
    }
}
