using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Koowoo.Services.LkbResponse
{
    [XmlRoot("Tpp2Fpp")]
    public class ResultResponse
    {
        [XmlElement("RspHeader")]
        public RspHeader Header { get; set; }
    }

    [XmlType("RspHeader")]
    public class RspHeader
    {
        [XmlElement("RspSeqNo")]
        public string RspSeqNo { get; set; }

        [XmlElement("RspCode")]
        public string RspCode { get; set; }

        [XmlElement("RspMsg")]
        public string RspMsg { get; set; }
    }

    //RspCode 说明
    // 0 ： 成功
    // 1 ： 用户不存在，或密码错误
    // 2 ： 传入数据条数超过限制
    // 3 ： 必填字段缺失
    // 4 ： 数据已经存在
    // 5 ： 入参格式错误
    //-1 ： 系统内部错误
}
