using System;
using System.Collections.Generic;
using System.Linq;
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
    [RoutePrefix("api/config")]
    public class ConfigController : BaseApiController
    {
      
        /// <summary>
        /// 
        /// </summary>
        public IConfigService configService { get; set; }


       
        /// <summary>
        /// 参数列表
        /// </summary>
        /// <param name="page">请求页数</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="keyword">关键字</param>
        /// <returns></returns>
        [HttpGet, Route("list"), RequestAuthorize("sys:config:list")]       
        public IHttpActionResult GetList(int page = 1, int pageSize = 20, string keyword = "")
        {
            var req = new QueryListReq() {
                page = page,
                pageSize = pageSize,
                keyword = keyword
            };
            var table = configService.GetList(req);
            return Ok(new {
                code = 0,
                msg = "",
                data = table
            });
        }
        /// <summary>
        /// 获取参数值
        /// </summary>
        /// <param name="configId"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet, Route("infoBykey")]
        public IHttpActionResult GetByKey(String key)
        {
            var dto = configService.GetByKey(key);
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
        /// 获取参数值
        /// </summary>
        /// <param name="configId"></param>
        /// <returns></returns>
        [HttpGet, Route("info/{configId:int}"), RequestAuthorize("sys:config:info")]
        public IHttpActionResult Get(int configId)
        {
            var dto = configService.GetById(configId);
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
        /// 添加参数
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, Route("add"), RequestAuthorize("sys:config:add")]
        public ResponseModel Create([FromBody] ConfigDto model)
        {
            configService.Create(model);
            return new ResponseModel();
        }

        /// <summary>
        /// 修改参数
        /// </summary>
        /// <param name="model"></param>
        /// <param name="configId"></param>
        /// <returns></returns>
        [HttpPut, Route("update/{configId:int}"), RequestAuthorize("sys:config:update")]
        public ResponseModel Update([FromBody] ConfigDto model, int configId)
        {
            configService.Update(model);
            return new ResponseModel();
        }

        /// <summary>
        /// 删除参数
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpDelete, Route("delete"), RequestAuthorize("sys:config:delete")]
        public ResponseModel Delete([FromBody]DeleteDto dto)
        {
            configService.Delete(dto.ids);
            return new ResponseModel();
        }
    }
}
