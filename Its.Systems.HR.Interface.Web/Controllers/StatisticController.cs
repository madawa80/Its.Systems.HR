using Its.Systems.HR.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Its.Systems.HR.Domain.Model;
using Its.Systems.HR.Interface.Web.ViewModels;

namespace Its.Systems.HR.Interface.Web.Controllers
{


    public class StatisticController : Controller
    {
        private readonly ISessionManager _sessionManager;
        private readonly IPersonManager _personManager;

        private List<int> years;
        private int PaticipantCount;
        private int selectedyear;

        public StatisticController(ISessionManager sessionManager, IPersonManager personManager)
        {
            _sessionManager = sessionManager;
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
            int PaticipantsPerYear =0;
            int sessionscount = 0;

            if (string.IsNullOrEmpty(yearsList))
            {
                return View(new SessionSummaryStatisticsViewModel());

            }
            

            if (int.TryParse(Request.Form["yearslist"], out yearInInt) == true)
            {
                selectedyear = Int32.Parse(Request.Form["yearslist"]);
                years = Enumerable.Range(2010, 100).ToList();
                var sessionsForYear = _sessionManager.GetAllSessionsForYear(selectedyear).Include(n => n.Activity).ToList().OrderBy(n => n.Id);
                sessionscount = sessionsForYear.Count();
               

                foreach (var session in sessionsForYear)
                {
                    PaticipantCount = _personManager.GetAllParticipantsForSession(session.Id).ToList().Count;
                    sessionStatisticsRowsList.Add(new SessionStatisticsRow()
                    {
                        NumberOfParticipants = PaticipantCount,
                        Session = session
                    });

                     PaticipantsPerYear = PaticipantsPerYear + PaticipantCount;
                }
            }


            var viewModel = new SessionSummaryStatisticsViewModel()
            {
                
                Years = years,
                SessionStatisticsRows = sessionStatisticsRowsList,
                TotalPaticipants = PaticipantsPerYear,
                TotalSessions = sessionscount,

            };

            return View(viewModel);
        }
    }

}





