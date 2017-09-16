using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ManageQuartzNet.Controllers
{
    public class HomeController : Controller
    {
        [OutputCache(Duration = 20)]
        public ActionResult Index()
        {
            DateTime nowTime = DateTime.Now;
            ViewBag.Now = nowTime.ToString();
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}