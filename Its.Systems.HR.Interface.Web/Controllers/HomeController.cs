using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Its.Systems.HR.Domain.Interfaces;
using Its.Systems.HR.Infrastructure;
using Microsoft.Practices.ServiceLocation;

namespace Its.Systems.HR.Interface.Web.Controllers
{
    public class HomeController : Controller
    {
       private IPersonManager personManager;// = ServiceLocator.Current.GetInstance<IPersonManager>();

        public HomeController(IPersonManager pManager)
        {
            personManager = pManager;
        }

        public ActionResult Index()
        {
            var result = personManager.GetAllHrPersons().ToList();

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