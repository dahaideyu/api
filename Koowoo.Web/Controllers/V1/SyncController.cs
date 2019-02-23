using Koowoo.Pojo;
using Koowoo.Pojo.Request;
using Koowoo.Services;
using Koowoo.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Koowoo.Web.Controllers.V1
{
    /// <summary>
    /// 
    /// </summary>
    [RoutePrefix("api/sync")]
    public class SyncController : BaseApiController
    {

        /// <summary>
        /// 
        /// </summary>
        public ISyncLogServie _syncService { get; set; }


        /// <summary>
        /// 同步日志记录分页
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet, Route("list")] //, RequestAuthorize("admin:entry:list")
        public IHttpActionResult GetLogList([FromUri]QueryDateReq req)
        {
            if (null == req)
            {
                req = new QueryDateReq();
            }
            var table = _syncService.GetList(req);
            return Ok(new
            {
                code = 0,
                msg = "success",
                data = table
            });
        }

        /// <summary>
        /// 获取门信息
        /// </summary>
        /// <param name="logId"></param>
        /// <returns></returns>
        [HttpGet, Route("info")] // , RequestAuthorize("admin:door:info")
        public IHttpActionResult Get(string logId)
        {
            var dto = _syncService.GetById(logId);
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

    }
}
