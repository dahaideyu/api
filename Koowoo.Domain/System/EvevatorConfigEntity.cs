using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koowoo.Domain.System
{
    public class EvevatorConfigEntity : BaseEntity
    {
        public int Id { get; set; }
        public bool? MutiEvevator { get; set; }
        public bool? AutoCall { get; set; }
        public int? EvevatorCount { get; set; }
        public string EvevatorSN { get; set; }
        public string CommunityUUID { get; set; }
    }
}
