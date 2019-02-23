using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koowoo.Domain.System
{
    public class DictEntity: BaseEntity
    {
        public int DictID { get; set; }
        public string DictType { get; set; }
        public string DictName { get; set; }
        public string DictCode { get; set; }
        public int? OrderID { get; set; }
    }
}
