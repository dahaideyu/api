using System;

namespace Koowoo.Pojo
{
    public class RoomUserCardDeviceListDto
    {
        public string CardNo { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public string DoorType { get; set; }
        public string DoorName { get; set; }
        public string DoorUUID { get; set; }
        public string DeviceType { get; set; }
        public string DeviceName { get; set; }
        public string SNNumber { get; set; }
    }
}
