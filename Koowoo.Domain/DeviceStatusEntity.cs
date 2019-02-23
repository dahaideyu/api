using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koowoo.Domain
{
    /// <summary>
    /// 设备状态
    /// </summary>
    public class DeviceStatusEntity:BaseEntity
    {
        /// <summary>
        /// 设备ID，主键 外键
        /// </summary>
        public string DeviceUUID { get; set; }

        /// <summary>
        /// 0 脱机，1联机
        /// </summary>
        public int DeviceStatus { get; set; }

        /// <summary>
        /// 硬件版本
        /// </summary>
        public string HardwardVersion { get; set; }

        /// <summary>
        /// 软件版本
        /// </summary>
        public string SoftwareVersion { get; set; }

        /// <summary>
        /// IMSI
        /// </summary>
        public string IMSI { get; set; }

        /// <summary>
        /// MISSDN
        /// </summary>
        public string MISSDN { get; set; }

        /// <summary>
        /// 电池百分比
        /// </summary>
        public float Battery { get; set; }

        /// <summary>
        /// 温度
        /// </summary>
        public float Temperature { get; set; }

        /// <summary>
        /// 信号
        /// </summary>
        public float Signal { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 卡容量
        /// </summary>
        public int CardCapacity { get; set; }

        /// <summary>
        /// 白名单卡数量
        /// </summary>
        public int CardWhiteListCount { get; set; }

        /// <summary>
        /// 指纹容量
        /// </summary>
        public int FingerCapacity { get; set; }

        /// <summary>
        /// 指纹数量
        /// </summary>
        public int FingerCount { get; set; }

        /// <summary>
        /// 门开关状态 0: 门闭合 1: 门打开
        /// </summary>
        public int IsOPened { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 工作状态
        /// </summary>
        public string WorkMode { get; set; }

        /// <summary>
        /// 电池状态
        /// </summary>
        public string PowerMode { get; set; }
        public bool Deleted { get; set; }
        public bool SyncStatus { get; set; }
        public int SyncVersion { get; set; }

        public virtual DeviceEntity Device { get; set; }
    }
}
