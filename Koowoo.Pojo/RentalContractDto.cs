using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koowoo.Pojo
{
    public class RentalContractDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string ContractUUID { get; set; }

        /// <summary>
        /// 承租人ID
        /// </summary>
        public string PersonUUID { get; set; }

        /// <summary>
        /// 承租房间ID
        /// </summary>
        public string RoomUUID { get; set; }

        /// <summary>
        /// 合同开始时间
        /// </summary>
        public DateTime? DateFrom { get; set; }

        /// <summary>
        /// 合同结束时间
        /// </summary>
        public DateTime? DateTo { get; set; }

        /// <summary>
        /// 状态，字典
        /// </summary>
        public int? RentalStauts { get; set; }

        /// <summary>
        /// 社区关联ID
        /// </summary>
        public string CommunityUUID { get; set; }
    }
}
