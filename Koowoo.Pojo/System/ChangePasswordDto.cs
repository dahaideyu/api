using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koowoo.Pojo.System
{
    public class ChangePasswordDto
    {
        public string opassword { get; set; }
        public string npassword { get; set; }
    }

    public class ResetPasswordDto
    {
        public int userId { get; set; }
        public string password { get; set; }
    }
}
