using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuartzNetMonitor.Controllers
{
    public class MonitorController : Controller
    {
       // private static RemoteSchedulerProvider remoteSchedulerProvider = new CrystalQuartz.Core.SchedulerProviders.RemoteSchedulerProvider();
       // remoteSchedulerProvider.SchedulerHost = System.Configuration.ConfigurationManager.AppSettings["SchedulerHost"];
        // GET: Monitor
        public ActionResult Index()
        {
            return View();
        }
    }
}