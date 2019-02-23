using Koowoo.Domain.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koowoo.Domain
{
   public class FingerPrintEntity:BaseEntity
    {
        public string FingerPrintUUID { get; set; }
        /// <summary>
        /// Person关联ID
        /// </summary>
        public string PersonUUID { get; set; }

        /// <summary>
        /// 指纹类型
        /// </summary>
        public int FingerPrintType { get; set; }

        /// <summary>
        /// 指纹图片
        /// </summary>
        public string FingerPrintPic { get; set; }

        /// <summary>
        /// 关联社区ID
        /// </summary>
        public string CommunityUUID { get; set; }

        public virtual PersonEntity Person { get; set; }
        public virtual DictEntity FingerPrintTypeDict { get; set; }
    }
}
