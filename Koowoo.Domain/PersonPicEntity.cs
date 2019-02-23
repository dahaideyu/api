using Koowoo.Domain.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koowoo.Domain
{
   public class PersonPicEntity : BaseEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string PersonPicUUID { get; set; }

        /// <summary>
        /// Person关联ID
        /// </summary>
        public string PersonUUID { get; set; }

        /// <summary>
        /// 图片类型，数据字典
        /// </summary>
        public int PersonPicType { get; set; }

        /// <summary>
        /// 图片地址
        /// </summary>
        public string PersonPic { get; set; }

        /// <summary>
        /// 关联社区ID
        /// </summary>
        public string CommunityUUID { get; set; }

        public virtual PersonEntity Person { get; set; }
        public virtual DictEntity PersonPicTypeDict { get; set; }
    }
}
