using System.Net;
using System.Net.Http;
using System.Web.Http;
using Koowoo.Core;
using Koowoo.Pojo;
using Koowoo.Services.System;
using System.Text;
using Koowoo.Pojo.System;
using Koowoo.Pojo.Request;
using Koowoo.Web.Common;

namespace Koowoo.Web.Controllers.Admin
{
    /// <summary>
    /// 
    /// </summary>
    [RoutePrefix("api/dictType")]
    public class DictTypeController : BaseApiController
    {

        #region 私有字段

        /// <summary>
        /// 
        /// </summary>
        public IDictTypeService dictTypeService { get; set; }

        #endregion


        /// <summary>
        /// 字典类型列表
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("list"), RequestAuthorize("sys:dicttype:list")]
        public IHttpActionResult GetList()
        {
            var table = dictTypeService.GetList();
            return Ok(new
            {
                code = 0,
                msg = "success",
                data = table
            });
        }

        /// <summary>
        /// 获取字典类型
        /// </summary>
        /// <param name="code">多个字典类型</param>
        /// <returns></returns>
        [HttpGet, Route("ByCode"), RequestAuthorize("sys:dicttype:info")]
        public IHttpActionResult GetByCode(string code)
        {
            var entity = dictTypeService.GetByCode(code);
            return Ok(new
            {
                code = 0,
                msg = "success",
                data = entity
            });
        }



       

        /// <summary>
        /// 添加字典类型
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, Route("add"), RequestAuthorize("sys:dicttype:add")]
        public ResponseModel Create([FromBody] DictTypeDto model)
        {
            dictTypeService.Create(model);
            return new ResponseModel();
        }

        /// <summary>
        /// 修改字典类型
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, Route("update"), RequestAuthorize("sys:dicttype:update")]
        public ResponseModel Update([FromBody] DictTypeDto model)
        {
            dictTypeService.Update(model);
            return new ResponseModel();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpDelete, Route("delete"), RequestAuthorize("sys:dicttype:delete")]
        public ResponseModel Delete([FromBody]DictTypeDto dto)
        {
            dictTypeService.Delete(dto);
            return new ResponseModel();
        }
        /// <summary>
        /// 删除按照id
        /// </summary>
        /// <param name="dicttypeId"></param>
        /// <returns></returns>
        [HttpDelete, Route("delete/{dicttypeId}"), RequestAuthorize("sys:dicttype:delete")]
        public ResponseModel Delete(string dicttypeId)
        {
            dictTypeService.Delete(dicttypeId);
            return new ResponseModel();
        }
    }
}
