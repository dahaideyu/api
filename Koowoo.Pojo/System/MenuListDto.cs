using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koowoo.Pojo.System
{
    public class MenuListDto
    {
        public MenuListDto()
        {
            this.Children = new List<MenuListDto>();
        }

        public int MenuID { get; set; }
        public string MenuName { get; set; }
        public string MenuUrl { get; set; }
        public string MenuIcon { get; set; }

        public List<MenuListDto> Children { get; set; }
    }
}
