using Koowoo.Domain.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koowoo.Domain
{
    public class CardNoConvertEntity: BaseEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string CardNOConverUUID { get; set; }

        /// <summary>
        /// 卡关联ID
        /// </summary>
        public string CardUUID { get; set; }

        /// <summary>
        /// 转码类型，字典值
        /// </summary>
        public int CardConvertType { get; set; }

        /// <summary>
        /// 转码后的号码
        /// </summary>
        public string CardConvertNo { get; set; }

        public virtual CardEntity Card { get; set; }

        public virtual DictEntity CardConvertTypeDict { get; set; }
    }
}
