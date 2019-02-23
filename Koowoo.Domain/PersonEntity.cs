using Koowoo.Domain.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koowoo.Domain
{
    public class PersonEntity:BaseEntity
    {
        /// <summary>
        /// Person主键
        /// </summary>
        public string PersonUUID { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string PersonName { get; set; }

        /// <summary>
        /// 英文名
        /// </summary>
        public string EnglishName { get; set; }

        /// <summary>
        /// 身份证号
        /// </summary>
        public string IDCardNo { get; set; }

        /// <summary>
        /// 性别（字典）
        /// </summary>
        public int Sex { get; set; }

        /// <summary>
        /// 民族（字典）
        /// </summary>
        public int Nation { get; set; }

        /// <summary>
        /// 出生年月 
        /// </summary>
        public DateTime Birthday { get; set; }

        /// <summary>
        /// 身份证签发机关
        /// </summary>
        public string IssueOrg { get; set; }

        /// <summary>
        /// 身份证有效期开始时间
        /// </summary>
        public DateTime? ValidFrom { get; set; }

        /// <summary>
        /// 身份证有效期结束时间
        /// </summary>
        public DateTime? ValidTo { get; set; }

        /// <summary>
        /// 户籍地址
        /// </summary>
        public string RegAddress { get; set; }

        /// <summary>
        /// 原户籍地址
        /// </summary>
        public string OriginRegAddress { get; set; }

        /// <summary>
        /// 现住地
        /// </summary>
        public string LivingAddress { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string PhoneNo { get; set; }
        //public byte[] CertificateIIMG { get; set; }

        /// <summary>
        /// 备用电话
        /// </summary>
        public string BackUpPhoneNo { get; set; }
        /// <summary>
        /// 人脸照片
        /// </summary>
        public string FacePic { get; set; }

        /// <summary>
        /// 身份证照片
        /// </summary>
        public string CertificateIPic { get; set; } 
        /// <summary>
        /// 身份证头像
        /// </summary>
        public string IDCardPic { get; set; }

        /// <summary>
        /// 是否重点人员 0|否, 1|是
        /// </summary>
        public int IsFocal { get; set; }

        /// <summary>
        /// 证件类型,数据字典
        /// </summary>
        public int? IDDocumentType { get; set; }

        /// <summary>
        /// 是否本地人口
        /// </summary>
        public int IsLocal { get; set; }

        /// <summary>
        /// 身份证是否被验证
        /// </summary>
        public int IsIDVerified { get; set; }

        /// <summary>
        /// 人员类型 字典
        /// </summary>
        public int? PersonType { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }
       
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 社区关联id
        /// </summary>
        public string CommunityUUID { get; set; }

        public bool Deleted { get; set; }
        public bool SyncStatus { get; set; }
        public int SyncVersion { get; set; }

        public bool IsBasicInfo { get; set; } //基本信息 true false
        public bool IsVerified { get; set; } //已验证 true false
        public int IsLived { get; set; }  //已入住 0 未入住 1 已入住 2 离开
        public bool IsRenter { get; set; } //承租人 true false

        public virtual DictEntity SexDict { get; set; }
        public virtual DictEntity NationDict { get; set; }
        public virtual DictEntity IDDocumentTypeDict { get; set; }
        public virtual DictEntity PersonTypeDict { get; set; }
        //public virtual ICollection<PersonCardEntity> Cards { get; set; }
        public virtual ICollection<EntryHistoryEntity> EntryHistorys { get; set; }
        public virtual ICollection<FingerPrintEntity> FingerPrints { get; set; }
        public virtual ICollection<RentalContractEntity> RentalContracts { get; set; }
        public virtual ICollection<PersonPicEntity> PersonPics { get; set; }

    }
}
