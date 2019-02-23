using Koowoo.Pojo;
using Koowoo.Services;
using System.Web.Mvc;

namespace Koowoo.Web.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        public IRoomService roomService { get; set; }
     

        // GET: Home
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return Content("Home/Index");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Docs()
        {
            return Redirect("/Swagger/ui/index");
        }
    }
}