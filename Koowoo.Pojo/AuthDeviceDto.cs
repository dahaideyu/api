using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koowoo.Pojo
{
    public class AuthDeviceDto
    {
        public string IP { get; set; }
        public string Port { get; set; }
        public string SNNumber { get; set; }
        public string DoorNo { get; set; }
        public string DeviceNo { get; set; }
        public int DeviceType { get; set; }
        public string HomeNo { get; set; }
        public string DoorName { get; set; }
        public int CardConvertType { get; set; }
        /// <summary>
        /// 楼层
        /// </summary>
        public int Floor { get; set; }
        public string DoorUUID { get; set; }
        public string DoorTypeName { get; set; }
        public int Status { get; set; }
    }
}
