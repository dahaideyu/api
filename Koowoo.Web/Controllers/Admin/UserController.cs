using Koowoo.Pojo;
using Koowoo.Pojo.Request;
using Koowoo.Pojo.System;
using Koowoo.Services.System;
using Koowoo.Web.Common;
using System.Web.Http;

namespace Koowoo.Web.Controllers.Admin
{
    /// <summary>
    /// 管理员
    /// </summary>
    [RoutePrefix("api/user")]
    public class UserController : BaseApiController
    {
        /// <summary>
        /// 
        /// </summary>
        public IUserService userService { get; set; }

        /// <summary>
        /// 用户列表 分页
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [HttpGet, Route("list"), RequestAuthorize("sys:user:list")]
        public IHttpActionResult GetList(int page = 1, int pageSize = 20, string keyword = "")
        {
            
            var req = new QueryListReq()
            {
                page = page,
                pageSize = pageSize,
                keyword = keyword
            };

            var table = userService.GetList(req);
            return Ok(new {
                code = 0,
                msg = "success",
                data = table
            });
        }

        /// <summary>
        /// 获取用户信息（userId）
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        //[HttpGet, Route("info"), RequestAuthorize("sys:user:info")]
        [HttpGet, Route("info/{userId:int}"), RequestAuthorize("sys:role:info")]
        public IHttpActionResult Get(int userId)
        {
            var userDto = userService.GetById(userId);
            if (userDto != null)
            {
                return Ok(new {
                    code = 0,
                    msg = "success",
                    data = userDto
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
        /// 添加用户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, Route("add"), RequestAuthorize("sys:user:add")]
        public ResponseModel Create([FromBody] UserDto model)
        {

            var result = userService.Create(model);
            return result;
        }

        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPut, Route("update/{userId:int}"), RequestAuthorize("sys:user:update")]
        public ResponseModel Update([FromBody] UserDto model, int userId)
        {
            userService.Update(model);
            return new ResponseModel();
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpDelete, Route("delete"), RequestAuthorize("sys:user:delete")]
        public ResponseModel Delete([FromBody]DeleteDto dto)
        {
            userService.Delete(dto.ids);
            return new ResponseModel();
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, Route("modifypwd"), RequestAuthorize("sys:user:reset")]
        public ResponseModel ChangePwd([FromBody] ResetPasswordDto model)
        {
            var result = userService.ResetPwd(model.userId, model.password);
            return result;
        }
    }
}
