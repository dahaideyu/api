using System.Web.Http;
using Koowoo.Pojo;
using Koowoo.Services;
using System;

namespace Koowoo.Web.Controllers.Admin
{
    /// <summary>
    /// 
    /// </summary>
    [RoutePrefix("api/statistics")]
    public class ChartController : BaseApiController
    {
        /// <summary>
        /// 
        /// </summary>
        public IStatisticsService stateService { get; set; }


        /// <summary>
        /// "小区名称", "房间数", "已租房间数", "居住人数", "出租率"
        /// </summary>        
        /// <returns></returns>
        [HttpGet, Route("data")]
        public IHttpActionResult ChartData()
        {
            var chartData = stateService.GetCommunnityEntryState(DateTime.Now.AddDays(-30),DateTime.Now);

            return Ok(new
            {
                code = 0,
                msg = "success",
                data = chartData
            });
        }

        /// <summary>
        /// "小区名称", "房间数", "已租房间数", "居住人数", "出租率"
        /// </summary>        
        /// <returns></returns>
        [HttpGet, Route("data2")]
        public IHttpActionResult ChartData2()
        {
            var chartData = stateService.GetCommunnityState();       
            

            return Ok(new
            {
                code = 0,
                msg = "success",
                data = chartData
            });
        }


        
    }
}
