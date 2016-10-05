using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Its.Systems.HR.Domain.Interfaces;
using Its.Systems.HR.Domain.Model;

namespace Its.Systems.HR.Interface.Web.Controllers
{
    public class HomeController : Controller
    {
        private IPersonManager personManager;// = ServiceLocator.Current.GetInstance<IPersonManager>();
        private readonly IActivityManager _activityManager;

        public HomeController(IPersonManager pManager, IActivityManager activityManager)
        {
            personManager = pManager;
            _activityManager = activityManager;
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