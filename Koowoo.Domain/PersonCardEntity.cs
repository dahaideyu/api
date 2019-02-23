using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koowoo.Domain
{
    public class PersonCardEntity:BaseEntity
    {
        public string PersonCardUUID { get; set; }
        /// <summary>
        /// person关联ID
        /// </summary>
        public string PersonUUID { get; set; }

        /// <summary>
        /// card关联ID
        /// </summary>
        public string CardUUID { get; set; }

        /// <summary>
        /// ？
        /// </summary>
        public string IDCard { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 社区关联ID
        /// </summary>
        public string CommunityUUID { get; set; }

        public virtual PersonEntity Person { get; set; }
        public virtual CardEntity Card { get; set; }

    }
}
