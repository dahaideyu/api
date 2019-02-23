using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koowoo.Pojo
{
    public class AreaSelectListDto
    {
       
        /// <summary>
        /// 共有字段
        /// </summary>
        public string AreaUUID { get; set; }

        /// <summary>
        ///完整区域行政编码（定长为 31位）
        /// </summary>
        public string Code { get; set; }
       
        /// <summary>
        /// 中文名称
        /// </summary>
        public string ChineseName { get; set; }

        /// <summary>
        /// 英文名称
        /// </summary>
      //  public string EnglishName { get; set; }

      
        public string FullAreaName { get; set; }
    }
}
