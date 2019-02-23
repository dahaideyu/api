using Koowoo.Domain.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koowoo.Domain
{
    /// <summary>
    /// 门表
    /// </summary>
    public class DoorEntity : BaseEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string DoorUUID { get; set; }

        /// <summary>
        /// 门的名称
        /// </summary>
        public string DoorName { get; set; }

        /// <summary>
        /// 门类型，字典
        /// </summary>
        public int DoorType { get; set; }

        /// <summary>
        /// 设备关联ID
        /// </summary>
        public string DeviceUUID { get; set; }

        /// <summary>
        /// 门序号
        /// </summary>
        public string DoorNo { get; set; }

        /// <summary>
        /// 区域关联ID
        /// </summary>
        public string AreaUUID { get; set; }

        /// <summary>
        /// 楼层
        /// </summary>
        public int Floor { get; set; }

        /// <summary>
        /// 1激活 0冻结
        /// </summary>
        public int Status { get; set; }

        public bool Deleted { get; set; }

        public bool SyncStatus { get; set; }

        public int SyncVersion { get; set; }

        /// <summary>
        /// 社区关联ID
        /// </summary>
        public string CommunityUUID { get; set; }
        public DateTime CreateTime { get; set; }

        public virtual DeviceEntity Device { get; set; }

        public virtual DictEntity DoorTypeDict { get; set; }

        public virtual AreaEntity Area { get; set; }

    }
}
