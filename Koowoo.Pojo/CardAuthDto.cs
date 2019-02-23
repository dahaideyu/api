using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koowoo.Pojo
{
    public class CardAuthDto
    {
        public string CardAuthUUID { get; set; }
        public string CardUUID { get; set; }
        public string DoorUUID { get; set; }
        public string DeviceUUID { get; set; }
        public string AuthType { get; set; }

        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public int? CreateBy { get; set; }

    }
}
