using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koowoo.Pojo
{
    public class CardAuthListDto
    {
        public string CardNo { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public string DoorType { get; set; }
        public string DoorName { get; set; }
        public string SNNumber { get; set; }
        public string DeviceType { get; set; }

    }
}
