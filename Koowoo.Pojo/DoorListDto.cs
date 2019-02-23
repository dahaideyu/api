using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koowoo.Pojo
{
    public class DoorListDto
    {
        public string DoorUUID { get; set; }
        /// <summary>
        /// 设备关联ID
        /// </summary>
        public string DeviceUUID { get; set; }
        public string DeviceName { get; set; }
        public string DeviceIP { get; set; }
        public string DevicePort { get; set; }
        public string DeviceSN { get; set; }
        public string AreaUUID { get; set; }
        public string DoorTypeName { get; set; }

        public int CardConvertType { get; set; }
        public int DeviceType { get; set; }
        /// <summary>
        /// 楼层
        /// </summary>
        public int Floor { get; set; }
        public string DoorName { get; set; }
        /// <summary>
        /// 门序号
        /// </summary>
        public string DoorNo { get; set; }
        public int Status { get; set; }
    }
}
