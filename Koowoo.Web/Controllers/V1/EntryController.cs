using Koowoo.Core;
using Koowoo.Pojo;
using Koowoo.Pojo.Request;
using Koowoo.Services;
using Koowoo.Web.Common;
using Newtonsoft.Json;
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
    [RoutePrefix("api/entry")]
    public class EntryController : BaseApiController
    {

        /// <summary>
        /// 
        /// </summary>
        public IEntryHistoryService entryService { get; set; }


        /// <summary>
        /// 出入记录分页
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet, Route("listByPerson"), RequestAuthorize("admin:entry:list")]
        public IHttpActionResult GetListByPerson(EntryListReq req)
        {
            if (null == req)
            {
                req = new EntryListReq();
            }
            var table = entryService.GetListByPerson(req);
            return Ok(new
            {
                code = 0,
                msg = "success",
                data = table
            });
        }


        /// <summary>
        /// 出入记录分页
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet, Route("EntryList"), RequestAuthorize("admin:entry:list")]
        public IHttpActionResult GetEntryList([FromUri]EntryListReq req)
        {
            if (null == req)
            {
                req = new EntryListReq();
            }
            var table = entryService.GetList(req);
            return Ok(new
            {
                code = 0,
                msg = "success",
                data = table
            });
        }


        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="req"></param>
        ///// <returns></returns>
        //[HttpGet, Route("list"), RequestAuthorize("admin:entry:list")]
        //public IHttpActionResult GetList(EntryListReq req)
        //{
        //    if (null == req)
        //    {
        //        req = new EntryListReq();
        //    }
        //    var table = entryService.GetListByPerson(req);
        //    return Ok(new
        //    {
        //        code = 0,
        //        msg = "success",
        //        data = table
        //    });
        //}

        /// <summary>
        ///  添加区域
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost, Route("add"), RequestAuthorize("admin:entry:add")]
        public ResponseModel Create([FromBody] EntryHistoryDto model)
        {
            try
            {
                Log.Debug("insert controll DTO start:" + JsonConvert.SerializeObject(model));
                entryService.Create(model);
                return new ResponseModel();
            }
            catch (Exception ex)
            {
                Log.Debug(this.GetType().ToString(), ex.Message);
                return new ResponseModel() { code = 99999, msg = ex.Message };
            }

        }
    }
}
