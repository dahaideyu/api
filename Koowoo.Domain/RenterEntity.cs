using Koowoo.Domain.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koowoo.Domain
{
    /// <summary>
    /// 承租人
    /// </summary>
    public class RenterEntity:BaseEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string PersonUUID { get; set; }

        /// <summary>
        /// 政治面貌 数据字典
        /// </summary>
        public int? PoliticalStatus { get; set; }

        /// <summary>
        /// 教育程度，字典
        /// </summary>
        public int? EduLevel { get; set; }

        /// <summary>
        /// 来本地日期
        /// </summary>
        public DateTime? LocalArriveDate { get; set; }

        /// <summary>
        /// 防疫情况
        /// </summary>
        public string EpidemicPrevention { get; set; }

        /// <summary>
        /// 就学情况
        /// </summary>
        public string StudyHistory { get; set; }

        /// <summary>
        /// 是否接种
        /// </summary>
        public int Inoculability { get; set; }

        /// <summary>
        /// 暂住证编号
        /// </summary>
        public string StayCard { get; set; }

        /// <summary>
        /// 工作地址
        /// </summary>
        public string WorkingAddress { get; set; }

        /// <summary>
        /// 婚姻状态（不是应该是字典值码）
        /// </summary>
        public string MarriageStatus { get; set; }

        /// <summary>
        /// 配偶姓名
        /// </summary>
        public string MateName { get; set; }

        /// <summary>
        /// 配偶证件号码
        /// </summary>
        public string MateNo { get; set; }

        /// <summary>
        /// 生育女孩子数
        /// </summary>
        public int? Daughter { get; set; }

        /// <summary>
        /// 生育男孩子数
        /// </summary>
        public int? Sions { get; set; }

        /// <summary>
        /// 所持孕育证号码
        /// </summary>
        public string GestationNo { get; set; }

        /// <summary>
        /// 孕检时间
        /// </summary>
        public DateTime? PregnancyTestDate { get; set; }

        /// <summary>
        /// 落实避孕措施
        /// </summary>
        public string ContraceptionCount { get; set; }

        /// <summary>
        /// 是否怀孕
        /// </summary>
        public int IsPregnant { get; set; }

        /// <summary>
        /// 是否参加社会保险
        /// </summary>
        public int IsSocialInsur { get; set; }

        /// <summary>
        /// 是否签到劳动合同
        /// </summary>
        public int IsLaborContract { get; set; }

        /// <summary>
        /// 参保单位名称
        /// </summary>
        public string InsurCompany { get; set; }
        /// <summary>
        /// 参保单位联系人
        /// </summary>
        public string InsurCompanyContact { get; set; }

        /// <summary>
        /// 参保单位联系电话
        /// </summary>
        public string ICCPhoneNo { get; set; }

        /// <summary>
        /// 教育经历
        /// </summary>
        public string EduHistory { get; set; }

        /// <summary>
        /// 就读院校
        /// </summary>
        public string School { get; set; }

        /// <summary>
        /// 居住类型 （数据字典）
        /// </summary>
        public int? ResidenceType { get; set; }

        /// <summary>
        /// 户籍类型（数据字典）
        /// </summary>
        public int? RegCertType { get; set; }

        /// <summary>
        /// 居住证件
        /// </summary>
        public string RegCert { get; set; }

        /// <summary>
        /// 租赁状态，数据字典
        /// </summary>
        public int? RentalStatus { get; set; }

        /// <summary>
        /// 暂住事由，数据字典
        /// </summary>
        public int? LivingReason { get; set; }

        /// <summary>
        /// 户主流水号
        /// </summary>
        public string RegCertNo { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 关联社区ID
        /// </summary>
        public string CommunityUUID { get; set; }

        public DateTime CreateTime { get; set; }

        public bool Deleted { get; set; }
        public bool SyncStatus { get; set; }
        public int SyncVersion { get; set; }

        public virtual DictEntity PoliticalStatusDict { get; set; }
        public virtual DictEntity EduLevelDict { get; set; }
        public virtual DictEntity ResidenceTypeDict { get; set; }
        public virtual DictEntity RegCertTypeDict { get; set; }
        public virtual DictEntity RentalStatusDict { get; set; }
        public virtual DictEntity LivingReasonDict { get; set; }
    }
}
