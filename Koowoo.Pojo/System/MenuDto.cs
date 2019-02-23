using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koowoo.Pojo.System
{
   

    public class MenuDto 
    {
        public MenuDto()
        {
            this.Children = new List<MenuDto>();
        }

        public int MenuID { get; set; }
        public string MenuName { get; set; }
        public int ParentID { get; set; }
        public string MenuUrl { get; set; }
        public string Perms { get; set; }
        public int MenuType { get; set; }
        public string MenuIcon { get; set; }
        public List<MenuDto> Children { get; set; }
        public string ParentName { get; set; }
        public int OrderNum { get; set; }
        public bool Active { get; set; }
    }
}
