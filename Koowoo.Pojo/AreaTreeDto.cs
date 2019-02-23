using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koowoo.Pojo
{
    public class AreaTreeDto
    {
        public AreaTreeDto()
        {
            this.Children = new List<AreaTreeDto>();
        }

        public string AreaUUID { get; set; }

        public string AreaCode { get; set; }

        public string AreaName { get; set; }

        public List<AreaTreeDto> Children { get; set; }
    }
}
