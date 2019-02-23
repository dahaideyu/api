using System.Web.Http;
using Koowoo.Pojo;
using Koowoo.Services;
using Koowoo.Web.Common;
using System.Text;
using Koowoo.Services.System;
using Koowoo.Pojo.System;

namespace Koowoo.Web.Controllers.Admin
{
    /// <summary>
    /// 
    /// </summary>
    [RoutePrefix("api/EvevatorConfig")]
    public class EvevatorConfigController : BaseApiController
    {
        /// <summary>
        /// 
        /// </summary>
        public IEvevatorConfigService evevatorConfigService { get; set; }

        /// <summary>
        /// 添加门
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, Route("add"), RequestAuthorize("admin:door:add")]
        public IHttpActionResult Create([FromBody] EvevatorConfigDto model)
        {
            evevatorConfigService.Create(model);
            return Ok(new
            {
                code = 0,
                msg = "success"
            });
        }

        /// <summary>
        /// 根据ID获取区域信息
        /// </summary>
        /// <param name="areaId"></param>
        /// <returns></returns>
        [HttpGet, Route("info"), RequestAuthorize("admin:area:info")]
        public IHttpActionResult GetByCommunityId(string communityId)
        {
            var dto = evevatorConfigService.GetByCommunityId(communityId);
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
