using Koowoo.Domain.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koowoo.Domain
{
    public class RoomEntity:BaseEntity
    {
        /// <summary>
        /// 房间号主键
        /// </summary>
        public string RoomUUID { get; set; }

        public string RoomName { get; set; }
        public string RoomFullName { get; set; }
        public string OtherCode { get; set; }

        /// <summary>
        /// 居住类型（数据字典）
        /// </summary>
        public int? ResidenceType { get; set; }

        /// <summary>
        /// 房屋类型（数据字典）
        /// </summary>
        public int? RoomType { get; set; }

        /// <summary>
        /// 房屋所有权类型（数据字典）
        /// </summary>
        public int? OwnershipType { get; set; }

        /// <summary>
        /// 房屋地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 房产证号
        /// </summary>
        public string CertificationNo { get; set; }

        /// <summary>
        /// 所属派出所
        /// </summary>
        public string PoliceStation { get; set; }

        /// <summary>
        /// 民警姓名
        /// </summary>
        public string Policeman { get; set; }

        /// <summary>
        /// 出租用途
        /// </summary>
        public string RentalUseage { get; set; }

        /// <summary>
        /// 登记备案
        /// </summary>
        public string Registration { get; set; }

        /// <summary>
        /// 备案开始时间
        /// </summary>
        public DateTime? RegFrom { get; set; }

        /// <summary>
        /// 备案结束时间
        /// </summary>
        public DateTime? RegTo { get; set; }

        /// <summary>
        /// 性质（数据字典）
        /// </summary>
        public int? Nature { get; set; }

        /// <summary>
        /// 管理等级（数据字典）
        /// </summary>
        public int? ManageLevel { get; set; }

        /// <summary>
        /// 租赁类型（数据字典）
        /// </summary>
        public int? RentalType { get; set; }

        /// <summary>
        /// 居所户型（数据字典）
        /// </summary>
        public int? RoomStyle { get; set; }

        /// <summary>
        /// 租赁信息
        /// </summary>
        public string RentalInfo { get; set; }

        /// <summary>
        /// 租赁价格
        /// </summary>
        public decimal RentalPrice { get; set; }

        /// <summary>
        /// 区域关联ID
        /// </summary>
        public string AreaUUID { get; set; }

        /// <summary>
        /// 房间楼层
        /// </summary>
        public int Floor { get; set; }

        /// <summary>
        /// 关联社区ID
        /// </summary>
        public string CommunityUUID { get; set; }

        public string HomeNo { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        public bool Deleted { get; set; }
        public bool SyncStatus { get; set; }
        public int SyncVersion { get; set; }

        public virtual DictEntity ResidenceTypeDict { set; get; }
        public virtual DictEntity RoomTypeDict { get; set; }
        public virtual DictEntity OwnershipTypeDict { get; set; }
        public virtual DictEntity NatureDict { get; set; }
        public virtual DictEntity ManageLevelDict { get; set; }
        public virtual DictEntity RentalTypeDict { get; set; }
        public virtual DictEntity RoomStyleDict { get; set; }


        public virtual AreaEntity Area { get; set; }
    }
}
