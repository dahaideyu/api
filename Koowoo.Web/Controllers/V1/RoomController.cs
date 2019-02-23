using Koowoo.Services;
using Koowoo.Pojo;
using System.Web.Http;
using Koowoo.Web.Common;

namespace Koowoo.Web.Controllers.Admin
{
    /// <summary>
    /// 
    /// </summary>
    [RoutePrefix("api/room")]
    public class RoomController : BaseApiController
    {
        /// <summary>
        /// 
        /// </summary>
        public IRoomService roomService { get; set; }
     
        /// <summary>
        /// 房间列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="key"></param>
        /// <param name="parentId"></param>
        /// <param name="communityId"></param>
        /// <returns></returns>
        [HttpGet, Route("list"), RequestAuthorize("admin:room:list")]
        public IHttpActionResult GetList(int page = 1, int pageSize = 20, string key = "", string parentId = "",string communityId="")
        {
            var table = roomService.GetList(page, pageSize, key, parentId, communityId);
            return Ok(new
            {
                code = 0,
                msg = "success",
                data = table
            });
        }

        /// <summary>
        /// 社区房间选择，办卡 合同 等地方使用
        /// </summary>
        /// <param name="communityId"></param>
        /// <returns></returns>
        [HttpGet, Route("select")]
        public IHttpActionResult GetList(string communityId)
        {
            var table = roomService.GetRoomList(communityId);
            return Ok(new
            {
                code = 0,
                msg = "success",
                data = table
            });
        }

        /// <summary>
        /// 根据区域ID获取房间信息
        /// </summary>
        /// <param name="areaId"></param>
        /// <returns></returns>       i5cfdvtgffffffffffqdscazu7
        [HttpGet, Route("infoByAreaUUID"), RequestAuthorize("admin:room:info")]
        public IHttpActionResult GetByAreaUUID(string areaId)
        {
            var dto = roomService.GetByAreaId(areaId);
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
        /// 根据roomID获取房间信息
        /// </summary>
        /// <param name="roomId"></param>
        /// <returns></returns>
        [HttpGet, Route("infoByRoomUserId"), RequestAuthorize("admin:room:info")]
        public IHttpActionResult GetByRoomUserId(string roomUserId)
        {
            var dto = roomService.GetByRoomUserId(roomUserId);
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
        /// 根据roomID获取房间信息
        /// </summary>
        /// <param name="roomId"></param>
        /// <returns></returns>
        [HttpGet, Route("info"), RequestAuthorize("admin:room:info")]
        public IHttpActionResult Get(string roomId)
        {
            var dto = roomService.GetById(roomId);
            if (dto != null)
            {
                return Ok(new {
                    code = 0,
                    msg = "",
                    data = dto
                });
            }
            else
            {
                return Ok(new {
                    code = 1,
                    msg = "数据不存在"
                });
            }
        }

        /// <summary>
        /// 添加房间
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, Route("add"), RequestAuthorize("admin:room:add")]
        public ResponseModel Create([FromBody] RoomDto model)
        {
            var rooms = roomService.GetRoomList(model.CommunityUUID, model.AreaUUID);
            if (rooms != null && rooms.Count > 0)
            {
                return new ResponseModel() { code = 1, msg = "已添加房间信息，不能重复添加" };
            }
            roomService.Insert(model);
            return new ResponseModel();
        }

        /// <summary>
        /// 添加/修改房间
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut, Route("update"), RequestAuthorize("admin:room:update")]
        public ResponseModel Update([FromBody] RoomDto model)
        {
            roomService.Update(model);
            return new ResponseModel();
        }

        /// <summary>
        /// 删除房间
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpDelete, Route("delete"), RequestAuthorize("admin:room:delete")]
        public ResponseModel Delete([FromBody]DeleteDto dto)
        {
            roomService.Delete(dto.ids);
            return new ResponseModel();
        }
    }
}
