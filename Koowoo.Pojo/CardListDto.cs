using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koowoo.Pojo
{
    public class CardListDto
    {
        public string CardUUID { get; set; }
        public string CardNo { get; set; }
        public int CardType { get; set; }
        public string CardTypeName { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
