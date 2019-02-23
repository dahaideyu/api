using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koowoo.Pojo.Request
{
    public class QueryDateReq: QueryListReq
    {
        public DateTime? beginTime { get; set; }
        public DateTime? endTime { get; set; }
        public int? status { get; set; }
    }
}
