using Koowoo.Domain.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koowoo.Domain
{
    /// <summary>
    /// 卡信息
    /// </summary>
    public class CardEntity:BaseEntity
    {
        /// <summary>
        /// 主键 ID
        /// </summary>
        public string CardUUID { get; set; }

        /// <summary>
        /// 卡的号码（16进制卡内码）
        /// </summary>
        public string CardNo { get; set; }


        /// <summary>
        /// 有效期开始时间
        /// </summary>
        public DateTime? ValidFrom { get; set; }

        /// <summary>
        /// 有效期结束时间
        /// </summary>
        public DateTime? ValidTo { get; set; }
       
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        public bool Deleted { get; set; }
        public bool SyncStatus { get; set; }
        public int SyncVersion { get; set; }

        /// <summary>
        /// 卡类型，数据字典
        /// </summary>
        public int CardType { get; set; }

        /// <summary>
        ///数据字典， 流口办的卡类型：1．临时卡 2.身份证 3.居住证
        /// </summary>
        public int CardTypeOfLK { get; set; }

        /// <summary>
        /// 区域权限因子(二级) （定长为 4位）流口办保留字段暂不考虑
        /// </summary>
        public string AreaKey { get; set; }

        /// <summary>
        /// 区域权限因子(一级) （定长为 4位） 流口办保留字段暂不考虑
        /// </summary>
        public string AreaKey1 { get; set; }

        /// <summary>
        /// 卡号后4位 流口办保留字段暂不考虑
        /// </summary>
        public string CardLast4NO { get; set; }

        /// <summary>
        /// 社区关联ID
        /// </summary>
        public string CommunityUUID { get; set; }

        public virtual DictEntity CardTypeDict { get; set; }
        public virtual DictEntity CardTypeOfLKDict { get; set; }

        //public virtual ICollection<CardAuthEntity> CardAuthList { get; set; }
        //public virtual ICollection<CardNoConvertEntity> CardNoConvertList { get; set; }
    }
}
