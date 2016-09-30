using Its.Systems.HR.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Its.Systems.HR.Domain.Model;
using Its.Systems.HR.Interface.Web.ViewModels;

namespace Its.Systems.HR.Interface.Web.Controllers
{


    public class SessionSummaryStatisticsController : Controller
    {

        private IActivityManager _activityManager;
        private IPersonManager _personManager;
        List<int>  PaticipantCount = new List<int>();

        public SessionSummaryStatisticsController(IActivityManager manager, IPersonManager personManager)
        {
            _activityManager = manager;
            _personManager = personManager;
        }

        public ActionResult Details(int year)
        {
           var years = Enumerable.Range(1960,140).ToList();
           var sessionsForYear = _activityManager.GetAllSessionsForYear(year).OrderBy(n => n.Id);

            foreach (var session in sessionsForYear)
            {
                PaticipantCount.Add(_activityManager.GetAllParticipantsForSession(session.Id).Count());
            }


            var viewModel = new SessionSummaryStatisticsViewModel()
            {
                Years = years,

                Sessions = _activityManager.GetAllSessionsForYear(year).OrderBy(n => n.Id),

                SessionParticipants = PaticipantCount,
                
            };
            
            return View(viewModel);

        }

      }

}





