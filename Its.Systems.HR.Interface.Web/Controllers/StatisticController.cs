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


    public class StatisticController : Controller
    {
        public List<int> years;
        private IActivityManager _activityManager;
        private IPersonManager _personManager;
        private int PaticipantCount;
        public int selectedyear;

        public StatisticController(IActivityManager manager, IPersonManager personManager)
        {
            _activityManager = manager;
            _personManager = personManager;
        }
        
        public ActionResult YearlyStatistics()
        {

            var viewModel = new SessionSummaryStatisticsViewModel()
            {
                //SessionStatisticsRows = new List<SessionStatisticsRow>()
                //{
                //    new SessionStatisticsRow()
                //}
            };

            return View(viewModel);

        }



        [HttpPost]
        public ViewResult YearlyStatistics(string yearsList)
        {
            int yearInInt;
            var sessionStatisticsRowsList = new List<SessionStatisticsRow>();


            if (string.IsNullOrEmpty(yearsList))
            {
                return View(new SessionSummaryStatisticsViewModel());

            }



            if (int.TryParse(Request.Form["yearslist"], out yearInInt) == true)
            {
                selectedyear = Int32.Parse(Request.Form["yearslist"]);
                years = Enumerable.Range(2010, 100).ToList();
                var sessionsForYear = _activityManager.GetAllSessionsForYear(selectedyear).ToList().OrderBy(n => n.Id);
               

                foreach (var session in sessionsForYear)
                {
                    PaticipantCount = _activityManager.GetAllParticipantsForSession(session.Id).ToList().Count;
                    sessionStatisticsRowsList.Add(new SessionStatisticsRow()
                    {
                        NumberOfParticipants = PaticipantCount,
                        Session = session
                    });
                }
            }


            var viewModel = new SessionSummaryStatisticsViewModel()
            {
                
                Years = years,
                SessionStatisticsRows = sessionStatisticsRowsList

            };

            return View(viewModel);
        }
    }

}





