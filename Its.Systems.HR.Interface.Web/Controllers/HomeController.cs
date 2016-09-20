using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Its.Systems.HR.Infrastructure;

namespace Its.Systems.HR.Interface.Web.Controllers
{
    public class HomeController : Controller
    {
        private HRContext db = new HRContext();

        public ActionResult Index()
        {
            var result = db.HrPersons.ToList();

            return View(result);
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