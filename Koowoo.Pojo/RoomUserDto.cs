using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koowoo.Pojo
{
    public class RoomUserDto
    {
        public string RoomUserUUID { get; set; }

        /// <summary>
        /// 承租房间ID
        /// </summary>
        public string RoomUUID { get; set; }

        /// <summary>
        /// 人员ID
        /// </summary>
        public string PersonUUID { get; set; }

        /// <summary>
        /// 入住日期
        /// </summary>
        public DateTime? LiveDate { get; set; }

        /// <summary>
        /// 离开日期
        /// </summary>
        public DateTime? LeaveDate { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreateDate { get; set; }

        public bool IsAdd { get; set; }

        /// <summary>
        /// 与承租人关系，字典值
        /// </summary>
        public int? FamilyRelation { get; set; }
        public string FamilyRelationName { get; set; }
        public int Status { get; set; }

        /// <summary>
        /// 关联社区ID
        /// </summary>
        public string CommunityUUID { get; set; }
    }
}
