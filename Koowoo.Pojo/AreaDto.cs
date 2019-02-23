using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koowoo.Pojo
{
    public class AreaDto
    {
        public AreaDto()
        {
            this.Children = new List<AreaDto>();
        }
        /// <summary>
        /// 共有字段
        /// </summary>
        public string AreaUUID { get; set; }

        /// <summary>
        ///完整区域行政编码（定长为 31位）
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        ///  区域层级编码
        /// </summary>
        public string AreaCode { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int? OrderID { get; set; }

        /// <summary>
        /// 中文名称
        /// </summary>
        public string ChineseName { get; set; }

        /// <summary>
        /// 英文名称
        /// </summary>
        public string EnglishName { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public int AreaType { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 区域权限因子(二级) （定长为 4位数字）
        /// </summary>
        public string KeyCode { get; set; }

        /// <summary>
        /// 区域权限因子(一级) （定长为 4位数字）
        /// </summary>
        public string KeyCode1 { get; set; }

        /// <summary>
        /// 是否
        /// </summary>
        public string IsParent { get; set; }

        /// <summary>
        /// 父级编码
        /// </summary>
        public string ParentCode { get; set; }
        public string ParentName { get; set; }

        public List<AreaDto> Children { get; set; }

        public string AreaTypeName { get; set; }
        public string FullAreaName { get; set; }
    }
}
