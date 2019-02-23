using System.Net;
using System.Net.Http;
using System.Web.Http;
using Koowoo.Core;
using Koowoo.Pojo;
using Koowoo.Services.System;
using System.Text;
using Koowoo.Pojo.System;
using Koowoo.Pojo.Request;
using Newtonsoft.Json;
using Koowoo.Web.Common;

namespace Koowoo.Web.Controllers.Admin
{
    /// <summary>
    /// 
    /// </summary>
    [RoutePrefix("api/role")]
    public class RoleController : BaseApiController
    {

        #region 私有字段
        /// <summary>
        /// 
        /// </summary>
        public IRoleService roleService { get; set; }

        #endregion


        /// <summary>
        /// 角色列表分页
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [HttpGet, Route("list"),RequestAuthorize("sys:role:list")]
        public IHttpActionResult GetList(int page = 1, int pageSize = 20, string keyword = "")
        {
            var req = new QueryListReq()
            {
                page = page,
                pageSize = pageSize,
                keyword = keyword
            };
            var table = roleService.GetList(req);
            return Ok(new
            {
                code = 0,
                msg = "success",
                data = table
            });
        }

        /// <summary>
        /// 角色列表（选择）
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("select")]
        public IHttpActionResult GetSelectList()
        {
            QueryListReq req = new QueryListReq() {
                page=1,pageSize=int.MaxValue
            };
            var table = roleService.GetList(req);
            return Ok(new
            {
                code = 0,
                msg = "success",
                data = table.list
            });
        }

        /// <summary>
        /// 获取角色信息
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpGet, Route("info/{roleId:int}"), RequestAuthorize("sys:role:info")]
        public IHttpActionResult Get(int roleId)
        {
            var dto = roleService.GetById(roleId);
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
                return Ok(new {
                    code = 1,
                    msg = "数据不存在"
                });
            }
        }

        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, Route("add"), RequestAuthorize("sys:role:add")]
        public ResponseModel Create([FromBody] RoleDto model)
        {
            roleService.Create(model);
            return new ResponseModel();
        }

        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="model"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpPut, Route("update/{roleId:int}"), RequestAuthorize("sys:role:update")]
        public ResponseModel Update([FromBody] RoleDto model, int roleId)
        {
            roleService.Update(model);
            return new ResponseModel();
        }

        /// <summary>
        /// 角色授权
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, Route("authen"), RequestAuthorize("sys:role:update")]
        public ResponseModel UpdateAuthen([FromBody] RoleDto model)
        {
            roleService.UpdateAuthen(model);
            return new ResponseModel();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpDelete, Route("delete"), RequestAuthorize("sys:role:delete")]
        public ResponseModel Delete([FromBody]DeleteDto dto)
        {
            roleService.Delete(dto.ids);
            return new ResponseModel();
        }
    }
}
