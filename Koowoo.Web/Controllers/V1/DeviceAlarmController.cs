using System.Web.Http;
using Koowoo.Pojo;
using Koowoo.Services;
using Koowoo.Pojo.Request;
using Koowoo.Web.Common;

namespace Koowoo.Web.Controllers.Admin
{
    /// <summary>
    /// 
    /// </summary>
    [RoutePrefix("api/deviceAlarm")]
    public class DeviceAlarmController : BaseApiController
    {
        /// <summary>
        /// 
        /// </summary>
        public IDeviceAlarmService deviceAlarmService { get; set; }

        /// <summary>
        /// 设备报警列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet, Route("list"), RequestAuthorize("admin:alarm:list")]
        public IHttpActionResult GetList([FromUri]QueryDateReq req)
        {
            if (null == req)
            {
                req = new QueryDateReq();
            }
            var table = deviceAlarmService.GetList(req);
            return Ok(new
            {
                code = 0,
                msg = "",
                data = table
            });
        }

        /// <summary>
        /// 添加设备报警信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost, Route("add")]
        public IHttpActionResult Create([FromBody] DeviceAlarmDto model)
        {
            deviceAlarmService.Create(model);
            return Ok(new
            {
                code = 0,
                msg = "success"
            });
        }


        /// <summary>
        /// 修改设备报警信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, Route("update")]
        public IHttpActionResult Update([FromBody] DeviceAlarmDto model)
        {
            deviceAlarmService.Update(model);
            return Ok(new
            {
                code = 0,
                msg = "success"
            });
        }


        /// <summary>
        /// 获取门信息
        /// </summary>
        /// <param name="alarmId"></param>
        /// <returns></returns>
        [HttpGet, Route("info"), RequestAuthorize("admin:alarm:info")]
        public IHttpActionResult Get(string alarmId)
        {
            var dto = deviceAlarmService.GetById(alarmId);
            if (dto != null)
            {
                return Ok(new
                {
                    code = 0,
                    msg = "success",
                    data = dto
                });
            }
            else
            {
                return Ok(new
                {
                    code = 1,
                    msg = "数据不存在"
                });
            }
        }


        /// <summary>
        /// 删除设备报警信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpDelete, Route("delete"), RequestAuthorize("admin:alarm:delete")]
        public IHttpActionResult Delete([FromBody]DeleteDto dto)
        {
            deviceAlarmService.Delete(dto.ids);
            return Ok(new
            {
                code = 0,
                msg = "success"
            });
        }
    }
}
