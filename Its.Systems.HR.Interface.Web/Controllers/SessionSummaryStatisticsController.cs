using Its.Systems.HR.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Its.Systems.HR.Domain.Model;

namespace Its.Systems.HR.Interface.Web.Controllers
{


    public class SessionSummaryStatisticsController : Controller
    {

        private IActivityManager _activityManager;
        private IPersonManager _personManager;

        public SessionSummaryStatisticsController(IActivityManager manager, IPersonManager personManager)
        {
            _activityManager = manager;
            _personManager = personManager;
        }

        public ActionResult Details(int? year)
        {
            Session session;

            

            return View();

        }

    }

}