using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koowoo.Services
{
    public class RoomAuthDto
    {
        public string RoomOtherCode { get; set; }
        public int RoomFloor { get; set; }
        public string AreaOtherCode { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
    }
}
