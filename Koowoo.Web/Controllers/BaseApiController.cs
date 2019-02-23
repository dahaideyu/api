using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Koowoo.Web.Common;

namespace Koowoo.Web.Controllers
{
    /// <summary>
    /// 权限过滤，ApiController继承
    /// </summary>

    [RequestAuthorize]
    public class BaseApiController : ApiController
    {

    }
}
