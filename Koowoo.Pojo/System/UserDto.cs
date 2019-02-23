using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koowoo.Pojo.System
{
    public class UserDto
    {
        public UserDto()
        {
            //this.Roles = new List<RoleDto>();
        }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }     
        public string Mobile { get; set; }
        public int Status { get; set; }
        public DateTime CreateTime { get; set; }
        

        //public List<RoleDto> Roles { get; set; }

        public string RoleIds { get; set; }
        public string RoleNames { get; set; }
        public string CommunityUUID { get; set; }
        public string CommunityName { get; set; }
        public string Remark { get; set; }
        public string Email { get; set; }
    }
}
