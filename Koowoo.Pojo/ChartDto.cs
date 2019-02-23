using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koowoo.Pojo
{
    public class ChartDto
    {
        public List<string> columns { get; set; }
        public List<Dictionary<string, object>> rows{ get; set; }
    }
}
