using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koowoo.Domain.System
{
    public class MenuEntity: BaseEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int MenuID { get; set; }

        /// <summary>
        /// 菜单名称
        /// </summary>
        public string MenuName { get; set; }

        /// <summary>
        /// 上级菜单id
        /// </summary>
        public int ParentID { get; set; }

        /// <summary>
        /// 菜单Url
        /// </summary>
        public string MenuUrl { get; set; }

        /// <summary>
        /// 操作权限编码
        /// </summary>
        public string Perms { get; set; }

        /// <summary>
        /// 菜单类型 1 目录 2 菜单，  3 按钮
        /// </summary>
        public int MenuType { get; set; }

        /// <summary>
        /// 菜单图标
        /// </summary>
        public string MenuIcon { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int OrderNum { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 是否激活 true有效 false禁用
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// 删除标识 true 删除 false 有效
        /// </summary>
        public bool Deleted { get; set; }


        private ICollection<RoleEntity> _roles;
        public virtual ICollection<RoleEntity> Roles
        {
            get { return _roles ?? (_roles = new List<RoleEntity>()); }
            protected set { _roles = value; }
        }
    }
}
