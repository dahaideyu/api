using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koowoo.Domain.System
{
    public class ConfigEntity:BaseEntity
    {
        public int ConfigID { get; set; }
        public string ConfigKey { get; set; }
        public string ConfigName { get; set; }
        public string ConfigValue { get; set; }
        public string CommunityUUID { get; set; }
    }
}
