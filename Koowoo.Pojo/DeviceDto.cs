using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koowoo.Pojo
{
    /// <summary>
    /// 设备信息
    /// </summary>
    public class DeviceDto
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
        /// 设备类型，可为空
        /// </summary>
        public int? DeviceType { get; set; }

        public string DeviceTypeName { get; set; }

        public int CardConvertType { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }
        public string InnerKey { get; set; }
        public int UserID { get; set; }

        /// <summary>
        /// 锁类型，可为空
        /// </summary>
        public int? LockType { get; set; }

        /// <summary>
        /// 传输方式，可为空
        /// </summary>
        public int? TransferType { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 社区关联ID
        /// </summary>
        public string CommunityUUID { get; set; }

        public string CommunityName { get; set; }

    }
}
