using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koowoo.Data.Domain
{
    /// <summary>
    /// 承租信息表
    /// </summary>
    public class RentalParnterEntity : BaseEntity
    {
        /// <summary>
        /// 承租房间关联ID
        /// </summary>
        public string RoomUUID { get; set; }

        /// <summary>
        /// 承租人关联ID
        /// </summary>
        public string PersonUUID { get; set; }

        /// <summary>
        /// 入住日期
        /// </summary>
        public DateTime LiveDate { get; set; }

        /// <summary>
        /// 离开日期
        /// </summary>
        public DateTime LeaveDate { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// ？？
        /// </summary>
        public int RelType { get; set; }

        /// <summary>
        /// 与承租人关系
        /// </summary>
        public int FamilyRelation { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Parm1 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Parm2 { get; set; }

        /// <summary>
        /// 卡关联ID
        /// </summary>
        public string crk_uuid { get; set; }

        /// <summary>
        /// 卡No
        /// </summary>
        public string crk_crkno { get; set; }

        /// <summary>
        /// 关联社区ID
        /// </summary>
        public string CommunityUUID { get; set; }
    }
}
