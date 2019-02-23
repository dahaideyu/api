using System.Web.Http;
using Koowoo.Pojo;
using Koowoo.Services;
using Koowoo.Web.Common;
using System.Text;

namespace Koowoo.Web.Controllers.Admin
{
    /// <summary>
    /// 
    /// </summary>
    [RoutePrefix("api/area")]
    public class AreaController : BaseApiController
    {
        /// <summary>
        /// 
        /// </summary>
        public IAreaService areaService { get; set; }

        /// <summary>
        /// 区域列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyword"></param>
        /// <param name="parentId">上级AraaUUID</param>
        /// <returns></returns>
        [HttpGet, Route("list"), RequestAuthorize("admin:area:list")]
        public IHttpActionResult GetList(int page = 1, int pageSize = 20, string keyword = "", string parentId = "")
        {
            var table = areaService.GetList(page, pageSize, keyword, parentId);
            return Ok(new
            {
                code = 0,
                msg = "success",
                data = table
            });
        }

       
        /// <summary>
        /// 根据ID获取区域信息
        /// </summary>
        /// <param name="areaId"></param>
        /// <returns></returns>
        [HttpGet, Route("info"), RequestAuthorize("admin:area:info")]
        public IHttpActionResult Get(string areaId)
        {
            var dto = areaService.GetById(areaId);
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
        /// 区域树型结构（区域管理左边）
        /// </summary>        
        /// <returns></returns>
        [HttpGet, Route("tree")]
        public IHttpActionResult GetTree()
        {
            var tree = areaService.GetTree();

            return Ok(new
            {
                code = 0,
                msg = "success",
                data = tree
            });
        }


        /// <summary>
        ///  添加区域
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, Route("add"), RequestAuthorize("admin:area:add")]
        public ResponseModel Create([FromBody] AreaDto model)
        {
            areaService.Create(model);
            return new ResponseModel();
        }

        /// <summary>
        /// 修改区域
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut, Route("update"), RequestAuthorize("admin:area:update")]
        public ResponseModel Update([FromBody] AreaDto model)
        {
            areaService.Update(model);
            return new ResponseModel();
        }

        /// <summary>
        /// 删除区域（web）
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpDelete, Route("delete"), RequestAuthorize("admin:area:delete")]
        public ResponseModel Delete([FromBody]DeleteDto dto)
        {
            areaService.Delete(dto.ids);
            return new ResponseModel();
        }


        /// <summary>
        /// 删除区域（winform）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpGet, Route("deleteById"), RequestAuthorize("admin:area:delete")]
        public ResponseModel DeleteById(string ids)
        {
            areaService.Delete(ids);
            return new ResponseModel();
        }
    }
}
