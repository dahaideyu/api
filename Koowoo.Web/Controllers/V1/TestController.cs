using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Koowoo.Pojo;
using System.Text;

namespace Koowoo.Web.Controllers.V1
{
    /// <summary>
    /// 测试
    /// </summary>
    [RoutePrefix("api")]
    public class TestController : ApiController
    {
        /// <summary>
        /// test
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("test")]
        public IHttpActionResult Get()
        {            
            return Ok(new
            {
                DateTime = DateTime.Now,
                Plugins = "TestGet"
            });
        }

        /// <summary>
        /// any
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("test/any")]
        public HttpResponseMessage GetAny()
        {         
            var response = Request.CreateResponse(HttpStatusCode.NotFound);
            response.Content = new StringContent("yes, yes", Encoding.UTF8);
            return response;
        }

        /// <summary>
        /// string
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("test/string")]
        public IHttpActionResult GetString()
        {
            return Ok("Hello");
        }

        /// <summary>
        /// ok
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("test/ok")]
        public ResponseModel GetOk()
        {
            return new ResponseModel();
        }
    }
}
