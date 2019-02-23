using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koowoo.Entity.System
{
    public class UserRoleEntity: BaseEntity
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int RoleID { get; set; }

        /// <summary>
        /// 所属用户
        /// </summary>
        public virtual UserEntity User { get; set; }
    }
}
