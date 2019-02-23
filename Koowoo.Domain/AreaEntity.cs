using Koowoo.Domain.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koowoo.Domain
{
    /// <summary>
    /// 区域表
    /// </summary>
    public class AreaEntity:BaseEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string AreaUUID { get; set; }

        /// <summary>
        /// 区域层级编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 完整区域行政编码（定长为 31位）
        /// </summary>
        public string AreaCode { get; set; }

        /// <summary>
        /// 状态 0是冻结，1是激活
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
        /// 类型，数据字典
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
        /// 是否有下级分类，当添加房间是为0,在无下级
        /// </summary>
        public string IsParent { get; set; }

        /// <summary>
        /// 父级编码
        /// </summary>
        public string ParentCode { get; set; }
       
        /// <summary>
        /// true 已删除 false未删除
        /// </summary>
        public bool Deleted { get; set; }

        public bool SyncStatus { get; set; }
        public int SyncVersion { get; set; }

        public virtual AreaEntity ParentArea { get; set; }

        public virtual ICollection<AreaEntity> Children { get; set; }
    }
}
