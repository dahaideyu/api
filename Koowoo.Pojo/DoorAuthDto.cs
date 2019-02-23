using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koowoo.Pojo
{
    public class DoorAuthDto
    {
        public string IPAddress { get; set; }
        public string SNNumber { get; set; }
        public string Port { get; set; }
        public string DoorNo { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
    }
}
