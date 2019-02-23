using System.Web.Http;
using Koowoo.Pojo;
using Koowoo.Services;
using Koowoo.Pojo.Request;
using Koowoo.Web.Common;
using Koowoo.Core.Extentions;
using Koowoo.Services.Auth;

namespace Koowoo.Web.Controllers.Admin
{
    /// <summary>
    /// 
    /// </summary>
    [RoutePrefix("api/device")]
    public class DeviceController : BaseApiController
    {
        /// <summary>
        /// 
        /// </summary>
        public IDeviceService deviceService { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public IDeviceStatusService deviceStatusService { get; set; }

        /// <summary>
        /// 设备列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet, Route("list"), RequestAuthorize("admin:device:list")]
        public IHttpActionResult GetList([FromUri]QueryListReq req)
        {
            if (null == req)
            {
                req = new QueryDateReq();
            }
            var table = deviceService.GetList(req);
            return Ok(new
            {
                code = 0,
                msg = "succese",
                data = table
            });
        }

        /// <summary>
        /// 根据社区ID获取设备列表（添加门的时候选择设备使用）
        /// </summary>
        /// <param name="communityId"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet, Route("select")]
        public IHttpActionResult GetSelectList(string communityId)
        {
            if (communityId.IsBlank())
            {
                return Ok(new
                {
                    code = 0,
                    msg = "communityId不能为空"                   
                });
            }

            var req = new QueryDateReq();
            req.page = 1;
            req.pageSize = int.MaxValue;
            req.communityId = communityId;

            var table = deviceService.GetList(req);

            return Ok(new
            {
                code = 0,
                msg = "",
                data = table.list
            });
        }

        /// <summary>
        /// 获取设备信息
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        [HttpGet, Route("info"), RequestAuthorize("admin:device:info")]
        public IHttpActionResult Get(string deviceId)
        {
            var dto = deviceService.GetById(deviceId);
            if (dto != null)
            {
                return Ok(new
                {
                    code = 0,
                    msg = "",
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
        /// 添加设备
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, Route("add"), RequestAuthorize("admin:device:add")]
        public IHttpActionResult Create([FromBody] DeviceDto model)
        {
            int UserId = CurrentUserUtils.CurrentPrincipal.UserId;
            model.UserID = UserId;
            deviceService.Create(model);
            return Ok(new
            {
                code = 0,
                msg = "success"
            });
        }

        /// <summary>
        /// 修改设备
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, Route("update"), RequestAuthorize("admin:device:update")]
        public IHttpActionResult Update([FromBody] DeviceDto model)
        {
            deviceService.Update(model);
            return Ok(new
            {
                code = 0,
                msg = "success"
            });
        }

        /// <summary>
        /// 修改设备Status字段
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, Route("setStatus"), RequestAuthorize("admin:device:update")]
        public IHttpActionResult setStatus([FromBody] DeviceDto model)
        {
            var dto = deviceService.GetById(model.DeviceUUID);
            dto.Status = model.Status;
            deviceService.Update(dto);
            return Ok(new
            {
                code = 0,
                msg = "success"
            });
        }

        /// <summary>
        /// 获取设备状态信息
        /// </summary>
        /// <param name="deviceId">设备ID</param>
        /// <returns></returns>
        [HttpGet, Route("status"), RequestAuthorize("admin:device:info")]
        public IHttpActionResult GetStatus(string deviceId)
        {
            var dto = deviceStatusService.GetById(deviceId);
            return Ok(new
            {
                code = 0,
                msg = "",
                data = dto
            });
        }

        /// <summary>
        /// 修改设备状态信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, Route("updateStatus"), RequestAuthorize("admin:device:update")]
        public IHttpActionResult UpdateStauts([FromBody] DeviceStatusDto model)
        {
            deviceStatusService.Save(model);
            return Ok(new
            {
                code = 0,
                msg = "success"
            });
        }

        /// <summary>
        /// 删除设备
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpDelete, Route("delete"), RequestAuthorize("admin:device:delete")]
        public IHttpActionResult Delete([FromBody]DeleteDto dto)
        {
            deviceService.Delete(dto.ids);
            return Ok(new
            {
                code = 0,
                msg = "success"
            });
        }
        /// <summary>
        /// 删除设备
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpGet, Route("deletebyid"), RequestAuthorize("admin:device:delete")]
        public IHttpActionResult DeleteById(string ids)
        {
            deviceService.Delete(ids);
            return Ok(new
            {
                code = 0,
                msg = "success"
            });
        }
    }
}
