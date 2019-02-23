using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koowoo.Pojo.System
{
    public class RoleDto
    {
        //public RoleDto()
        //{
        //    this.Menus = new List<MenuDto>();
        //}
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public DateTime CreateTime { get; set; }

        public string MenuIds { get; set; }

      //  public List<MenuDto> Menus { get; set; }
    }
}
