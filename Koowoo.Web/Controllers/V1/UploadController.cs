using Koowoo.Pojo;
using Koowoo.Web.Controllers.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Koowoo.Web.Controllers.V1
{
    /// <summary>
    /// 
    /// </summary>
    [RoutePrefix("api/upload")]
    public class UploadController : BaseApiController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("base")]
        public ResponseModel Upload()
        {
            string id = HttpContext.Current.Request["id"];
            string name = HttpContext.Current.Request["name"];
            HttpFileCollection files = HttpContext.Current.Request.Files;
            foreach (string key in files.AllKeys)
            {
                HttpPostedFile file = files[key];//file.ContentLength文件长度  
                if (string.IsNullOrEmpty(file.FileName) == false)
                    file.SaveAs(HttpContext.Current.Server.MapPath("~/upload/") + file.FileName);
            }


            return new ResponseModel();

        }
    }
}
