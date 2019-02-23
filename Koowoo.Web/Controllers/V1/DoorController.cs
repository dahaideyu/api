using Koowoo.Pojo;
using Koowoo.Pojo.System;
using Koowoo.Pojo.Request;
using Koowoo.Services;
using System.Web.Http;
using Koowoo.Web.Common;
using System.Text;

namespace Koowoo.Web.Controllers.Admin
{
    /// <summary>
    /// 
    /// </summary>
    [RoutePrefix("api/door")]
    public class DoorController : BaseApiController
    {
        /// <summary>
        /// 
        /// </summary>
        public IDoorService doorService { get; set; }

        /// <summary>
        /// 门列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet, Route("list"), RequestAuthorize("admin:door:list")]
        public IHttpActionResult GetList([FromUri]QueryListReq req)
        {
            if (null == req)
            {
                req = new QueryListReq();
            }
            var table = doorService.GetList(req);
            return Ok(new
            {
                code = 0,
                msg = "success",
                data = table
            });
        }

        /// <summary>
        /// 根据社区ID获取门列表用于选择
        /// </summary>
        /// <param name="communityId"></param>
        /// <returns></returns>
        [HttpGet, Route("select")]
        public IHttpActionResult GetSelectList(string communityId)
        {           
            var doorList = doorService.GetList(communityId);
            return Ok(new
            {
                code = 0,
                msg = "success",
                data = doorList
            });
        }

        /// <summary>
        /// 获取门信息,修改用
        /// </summary>
        /// <param name="doorId"></param>
        /// <returns></returns>
        [HttpGet, Route("info"),RequestAuthorize("admin:door:info")]
        public IHttpActionResult Get(string doorId)
        {
            var dto = doorService.GetById(doorId);
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
        /// 获取门和设备信息，用于远程开门
        /// </summary>
        /// <param name="doorId"></param>
        /// <returns></returns>
        [HttpGet, Route("device"), RequestAuthorize("admin:door:info")]
        public IHttpActionResult GetDoor(string doorId)
        {
            var dto = doorService.GetDoorDevice(doorId);
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
        /// 添加门
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, Route("add"), RequestAuthorize("admin:door:add")]
        public IHttpActionResult Create([FromBody] DoorDto model)
        {
            doorService.Create(model);
            return Ok(new
            {
                code = 0,
                msg = "success"
            });
        }

        /// <summary>
        /// 修改门
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, Route("update"), RequestAuthorize("admin:door:update")]
        public IHttpActionResult Update([FromBody] DoorDto model)
        {
            doorService.Update(model);
            return Ok(new
            {
                code = 0,
                msg = "success"
            });
        }

        /// <summary>
        /// 删除门
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpDelete, Route("delete"), RequestAuthorize("admin:door:delete")]
        public IHttpActionResult Delete([FromBody]DeleteDto dto)
        {
            doorService.Delete(dto.ids);
            return Ok(new
            {
                code = 0,
                msg = "success"
            });
        }

        /// <summary>
        /// 删除门
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet, Route("deleteById"), RequestAuthorize("admin:door:delete")]
        public IHttpActionResult DeleteById(string id)
        {
            doorService.Delete(id);
            return Ok(new
            {
                code = 0,
                msg = "success"
            });
        }
    }
}
