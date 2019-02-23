using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koowoo.Pojo
{
    public class RentalContractListDto
    {
        public string ContractUUID { get; set; }
        public string RoomUUID { get; set; }
        public string RoomFullPath { get; set; }
        public string PersonUUID { get; set; }
        public string PersonName { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }
}
