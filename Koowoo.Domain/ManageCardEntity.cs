using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koowoo.Domain
{
    public class ManageCardEntity:BaseEntity
    {
        public string Guid { get; set; }
        public string PersonUUID { get; set; }
        public string CardUUID { get; set; }
    }
}
