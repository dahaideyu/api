using Koowoo.Pojo;
using Koowoo.Pojo.System;
using Koowoo.Services.System;
using Koowoo.Web.Common;
using System.Web.Http;


namespace Koowoo.Web.Controllers.Admin
{
    /// <summary>
    /// 
    /// </summary>
    [RoutePrefix("api/menu")]
    public class MenuController : BaseApiController
    {
        #region 私有字段
        /// <summary>
        /// 
        /// </summary>
        public IMenuService menuService { get; set; }

        #endregion

        /// <summary>
        /// 菜单权限列表
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("list"), RequestAuthorize("sys:menu:list")]
        public IHttpActionResult GetList()
        {
            var menuList = menuService.GetAllMenuList(false);
            return Ok(new 
            {
                code = 0,
                msg = "success",
                data = menuList
            });
        }

        /// <summary>
        /// 菜单权限列表（可用List接口）
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("select")]
        public IHttpActionResult GetSelectList()
        {
            var menuList = menuService.GetSelectList();
            return Ok(new 
            {
                code = 0,
                msg = "success",
                data = menuList
            });
        }

        /// <summary>
        /// 根据ID获取菜单信息
        /// </summary>
        /// <param name="menuId"></param>
        /// <returns></returns>
        [HttpGet, Route("info/{menuId:int}"), RequestAuthorize("sys:menu:info")]
        public IHttpActionResult Get(int menuId)
        {
            var dto = menuService.GetById(menuId);
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
                return Ok(new{
                    code = 1,
                    msg = "数据不存在"
                });
            }
        }

        /// <summary>
        /// 添加菜单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, Route("add"), RequestAuthorize("sys:menu:add")]
        public ResponseModel Create([FromBody] MenuDto model)
        {
            menuService.Create(model);
            return new ResponseModel();
        }

        /// <summary>
        /// 修改菜单
        /// </summary>
        /// <param name="model"></param>
        /// <param name="menuId"></param>
        /// <returns></returns>
        [HttpPut, Route("update/{menuId:int}"), RequestAuthorize("sys:menu:update")]
        public ResponseModel Update([FromBody] MenuDto model, int menuId)
        {
            menuService.Update(model);
            return new ResponseModel();
        }

        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="menuId"></param>
        /// <returns></returns>
        [HttpDelete, Route("delete/{menuId:int}"), RequestAuthorize("sys:menu:delete")]
        public ResponseModel Delete(int menuId)
        {
           var result = menuService.Delete(menuId);
            return result;
        }
    }
}
