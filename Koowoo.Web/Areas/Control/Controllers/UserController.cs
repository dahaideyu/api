using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Koowoo.Web.Areas.Authen.Controllers
{
    public class UserController : Controller
    {
        // GET: Authen/User
        public ActionResult Index()
        {
            return View();
        }
    }
}