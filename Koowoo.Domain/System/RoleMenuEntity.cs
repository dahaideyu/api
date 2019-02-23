using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koowoo.Entity.System
{
    public class RoleMenuEntity: BaseEntity
    {
        public int ID { get; set; }
        public int MenuID { get; set; }
        public int RoleID { get; set; }
    }
}
