using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koowoo.Pojo
{
    public class DoorDto
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
        /// 门类型？？
        /// </summary>
        public int DoorType { get; set; }
        public string DoorTypeName { get; set; }


        /// <summary>
        /// 设备关联ID
        /// </summary>
        public string DeviceUUID { get; set; }
        public string DeviceName { get; set; }
        public string DeviceIP { get; set; }
        public string DevicePort { get; set; }
        public string DeviceSN { get; set; }
        public int? DeviceType { get; set; }


        /// <summary>
        /// 门序号
        /// </summary>
        public string DoorNo { get; set; }

        /// <summary>
        /// ？？
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 社区关联ID
        /// </summary>
        public string CommunityUUID { get; set; }

        public string CommunityName { get; set; }
        /// <summary>
        /// 区域关联ID
        /// </summary>
        public string AreaUUID { get; set; }
        public string AreaName { get; set; }
        public int? Floor { get; set; }

    }
}
