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
    [RoutePrefix("api/dict")]
    public class DictController : BaseApiController
    {

        #region 私有字段
        /// <summary>
        /// 
        /// </summary>
        public IDictService dictService { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IDictTypeService dictTypeService { get; set; }

        #endregion


        /// <summary>
        /// 根据字典类型获取字典值单个（维护）
        /// </summary>
        /// <param name="dictType">单个字典类型</param>
        /// <returns></returns>
        [HttpGet, Route("list"), RequestAuthorize("sys:dict:list")]
        public IHttpActionResult GetList(string dictType)
        {
            var table = dictService.GetList(dictType,new QueryListReq { page=1,pageSize=int.MaxValue});
            return Ok(new
            {
                code = 0,
                msg = "success",
                data = table.list
            });
        }

        /// <summary>
        /// 根据字典类型获取字典值（多个字典类型，含字典类型）
        /// </summary>
        /// <param name="code">多个字典类型</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet, Route("listByCode")]
        public IHttpActionResult GetListByCode(string code)
        {
            var list = dictService.GetListByCode(code);
            return Ok(new
            {
                code = 0,
                msg = "success",
                data = list
            });
        }

        /// <summary>
        /// 根据字典 类型及字典值获取字典类型
        /// </summary>
        /// <param name="dictType">字典类型</param>
        /// <param name="dictCode">字典值</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet, Route("infoByCode")]
        public IHttpActionResult GetInfoByCode(string dictType, string dictCode)
        {
            var dto = dictService.GetInfoByCode(dictType, dictCode);
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
        /// 根据ID获取字典信息
        /// </summary>
        /// <param name="dictId"></param>
        /// <returns></returns>
        [HttpGet, Route("info/{dictId:int}"), RequestAuthorize("sys:dict:info")]
        public IHttpActionResult Get(int dictId)
        {
            var dto = dictService.GetById(dictId);
            if (dto != null)
            {
                return Ok(new {
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
        /// 添加字典
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, Route("add"), RequestAuthorize("sys:dict:add")]
        public ResponseModel Create([FromBody] DictDto model)
        {
            dictService.Create(model);
            return new ResponseModel();
        }

        /// <summary>
        /// 修改字典
        /// </summary>
        /// <param name="model"></param>
        /// <param name="dictId"></param>
        /// <returns></returns>
        [HttpPut, Route("update/{dictId:int}"), RequestAuthorize("sys:dict:update")]
        public ResponseModel UpdateBatch([FromBody] DictDto model, int dictId)
        {
            dictService.Update(model);
            return new ResponseModel();
        }

       
        /// <summary>
        /// 删除按照id
        /// </summary>
        /// <param name="dictId"></param>
        /// <returns></returns>
        [HttpDelete, Route("delete/{dictId}"), RequestAuthorize("sys:dict:delete")]
        public ResponseModel Delete(string dictId)
        {
            dictService.Delete(dictId);
            return new ResponseModel();
        }
    }
}
