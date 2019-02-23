using Koowoo.Domain.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koowoo.Domain
{
    /// <summary>
    /// 设备信息
    /// </summary>
    public class DeviceEntity:BaseEntity
    {

        /// <summary>
        /// 主键
        /// </summary>
        public string DeviceUUID { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string DeviceName { get; set; }


        /// <summary>
        /// 序列号
        /// </summary>
        public string SNNumber { get; set; }
 

        /// <summary>
        /// 设备提供商
        /// </summary>
        public string Producer { get; set; }

        /// <summary>
        /// IP地址
        /// </summary>
        public string IPAddress { get; set; }

        /// <summary>
        /// MAC地址
        /// </summary>
        public string Mac { get; set; }

        /// <summary>
        /// IP地址端口号
        /// </summary>
        public string Port { get; set; }
      
        /// <summary>
        /// 设备类型，数据字典
        /// </summary>
        public int? DeviceType { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 内部码
        /// </summary>
        public string InnerKey { get; set; }

        /// <summary>
        /// 创建人ID
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// 门锁类型，字典
        /// </summary>
        public int? LockType { get; set; }

        /// <summary>
        /// 传输方式，字典
        /// </summary>
        public int? TransferType { get; set; }

        /// <summary>
        /// 设备所接受的转码规则
        /// </summary>
        public int CardConvertType { get; set; }

        public int? Floor { get; set; }

        /// <summary>
        /// 状态 1激活 0冻结
        /// </summary>
        public int Status { get; set; }
        public bool Deleted { get; set; }

        public bool SyncStatus { get; set; }
        public int SyncVersion { get; set; }

        /// <summary>
        /// 社区关联ID
        /// </summary>
        public string CommunityUUID { get; set; }


        public virtual DictEntity DeviceTypeDict { get; set; }

        public virtual DictEntity LockTypeDict { get; set; }

        public virtual DictEntity TransferTypeDict { get; set; }

        public virtual DictEntity CardConvertTypeDict { get; set; }

        public virtual DeviceStatusEntity DeviceStatus { get; set; }

        public virtual ICollection<DoorEntity> Doors { get; set; }
    }
}
