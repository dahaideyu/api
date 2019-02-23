using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koowoo.Domain.System
{
    public class RoleEntity: BaseEntity
    {
        /// <summary>
        /// 主键，角色ID
        /// </summary>
        public int RoleID { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 删除标识
        /// </summary>
        public bool Deleted { get; set; }

        /// <summary>
        /// 状态标识 true 激活 false 禁用
        /// </summary>
        public int Status { get; set; }

        private ICollection<MenuEntity> _menus;
        public virtual ICollection<MenuEntity> Menus
        {
            get { return _menus ?? (_menus = new List<MenuEntity>()); }
            protected set { _menus = value; }
        }
    }
}
