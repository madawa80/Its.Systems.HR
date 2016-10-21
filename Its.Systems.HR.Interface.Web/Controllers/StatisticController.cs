using Its.Systems.HR.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Its.Systems.HR.Domain.Model;
using Its.Systems.HR.Interface.Web.ViewModels;
using Its.Systems.HR.Interface.Web.ViewModels.Statistic;

namespace Its.Systems.HR.Interface.Web.Controllers
{


    public class StatisticController : Controller
    {
        private readonly ISessionManager _sessionManager;
        private readonly IPersonManager _personManager;
        private readonly IUtilityManager _utilityManager;
        private List<int> years;
        private int PaticipantCount;
        private int selectedyear;
        private int selectedTag;
        private List<Session> sessionsForTag;

        public StatisticController(ISessionManager sessionManager, IPersonManager personManager, IUtilityManager utilityManager)
        {
            _sessionManager = sessionManager;
            _personManager = personManager;
            _utilityManager = utilityManager;
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
                years = Enumerable.Range(2011, DateTime.Now.AddYears(1).Year-2010).ToList();
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


        public ViewResult FilterSessionsForTag(string TagId)
          
        {
            int Tag;
            

            if (string.IsNullOrEmpty(TagId))
            {
                return View(new SessionTagsViewModel());

            }
            var allTags = _utilityManager.GetAllTags().OrderBy(n => n.Name).ToList();

            if (int.TryParse(Request.Form["yearslist"], out Tag) == true)
            {
                selectedTag = Int32.Parse(Request.Form["yearslist"]);
                //years = Enumerable.Range(2011, DateTime.Now.AddYears(1).Year - 2010).ToList();
                sessionsForTag = _sessionManager.GetAllSessionsForTag(selectedTag).Include(n => n.Activity).ToList().OrderBy(n => n.Id).ToList();
               
            }


            var viewModel = new SessionTagsViewModel()
            {
                Tags = new SelectList(allTags, "Id", "Name"),
                Sessions = sessionsForTag,
              
            };

            return View(viewModel);
        }

    }

}



