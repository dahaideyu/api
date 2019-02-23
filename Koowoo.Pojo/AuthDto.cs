using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koowoo.Pojo
{
    public class AuthDto
    {
        public string RoomUUID { get; set; }

        public string RoomUserUUID { get; set; }

        /// <summary>
        /// 1 授权，2 取消授权
        /// </summary>
        public int AuthType { get; set; }

        public string CardUUID { get; set; }


        public int? CreateBy { get; set; }
    }
}
