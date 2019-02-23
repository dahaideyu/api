using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koowoo.Pojo.Request
{
    public class QueryListReq
    {
        public QueryListReq()
        {
            page = 1;
            pageSize = 20;
        }
        public int page { get; set; }
        public int pageSize { get; set; }
        public string keyword { get; set; }
        public string communityId { get; set; }
    }
}
