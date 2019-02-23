using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Koowoo.Web.Areas.Authen.Controllers
{
    public class MenuController : Controller
    {
        // GET: Authen/Menu
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Create()
        {
            return View();
        }


        public ActionResult Edit()
        {
            return View();
        }
    }
}