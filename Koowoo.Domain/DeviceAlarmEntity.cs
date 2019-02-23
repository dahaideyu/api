using Koowoo.Domain.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koowoo.Domain
{

    /// <summary>
    /// 设备告警
    /// </summary>
    public class DeviceAlarmEntity:BaseEntity
    {
        /// <summary>
        /// 告警uuid
        /// </summary>
        public string AlarmUUID { get; set; }

        /// <summary>
        /// 告警产生时间
        /// </summary>
        public DateTime OccurTime { get; set; }


        /// <summary>
        /// 入库时间
        /// </summary>
        public DateTime CreateTime { get; set; }


        /// <summary>
        /// 告警类型，字典值      强拆	1    机械钥匙开门	11    强行闯入	2    非法刷卡		14
        /// /// </summary>
        public int AlarmType { get; set; }

        /// <summary>
        /// 告警状态（状态1.已处理 0.未处理）
        /// </summary>
        public int Status { get; set; }


        /// <summary>
        /// 告警处理时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 告警设备ID
        /// </summary>
        public string DeviceUUID { get; set; }

        /// <summary>
        /// 处理意见、备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 告警非法刷卡卡号
        /// </summary>
        public string CardNo { get; set; }

        /// <summary>
        /// 社区关联ID
        /// </summary>
        public string CommunityUUID { get; set; }
        public bool Deleted { get; set; }

        public bool SyncStatus { get; set; }
        public int SyncVersion { get; set; }

        public virtual DeviceEntity Device { get; set; }

        public virtual DictEntity AlarmTypeDict { get; set; }

    }
}
