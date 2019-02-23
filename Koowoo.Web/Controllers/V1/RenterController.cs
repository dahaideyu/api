using Koowoo.Services;
using Koowoo.Pojo;
using System.Web.Http;
using Koowoo.Pojo.Request;
using Koowoo.Web.Common;

namespace Koowoo.Web.Controllers.Admin
{
    /// <summary>
    /// 
    /// </summary>
    [RoutePrefix("api/renter")]
    public class RenterController : BaseApiController
    {
        /// <summary>
        /// 
        /// </summary>
        public IRenterService renterService { get; set; }
     
        /// <summary>
        /// 获取租户信息
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        [HttpGet, Route("info")] //, RequestAuthorize("sys:renter:list")
        public IHttpActionResult Get(string personId)
        {
            var dto = renterService.GetById(personId);
            return Ok(new
            {
                code = 0,
                msg = "success",
                data = dto
            });
        }

        /// <summary>
        /// 保存租户信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, Route("save")] //, RequestAuthorize("sys:renter:save")
        public ResponseModel AddOrUpdate([FromBody] RenterDto model)
        {
            renterService.Save(model);
            return new ResponseModel();
        }      
    }
}
