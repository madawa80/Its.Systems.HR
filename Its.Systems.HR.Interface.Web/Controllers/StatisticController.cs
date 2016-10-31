using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using System.Windows.Documents;
using Its.Systems.HR.Domain.Interfaces;
using Its.Systems.HR.Domain.Model;
using Its.Systems.HR.Interface.Web.ViewModels;
using Its.Systems.HR.Interface.Web.ViewModels.Statistic;

namespace Its.Systems.HR.Interface.Web.Controllers
{
    public class StatisticController : Controller
    {
        private readonly IPersonManager _personManager;
        private readonly ISessionManager _sessionManager;
        private readonly IUtilityManager _utilityManager;
        private int PaticipantCount;
        private int selectedTag;
        private int selectedyear;
        private string tagDisplay;
        private List<int> years;
        private IQueryable<Session> sessionsForTag;

        public StatisticController(ISessionManager sessionManager, IPersonManager personManager,
            IUtilityManager utilityManager)
        {
            _sessionManager = sessionManager;
            _personManager = personManager;
            _utilityManager = utilityManager;
        }

        public ActionResult YearlyStatistics()
        {
            var viewModel = new SessionSummaryStatisticsViewModel();

            return View(viewModel);
        }

        [HttpPost]
        public ViewResult YearlyStatistics(string yearsList)
        {
            int yearInInt;
            var sessionStatisticsRowsList = new List<SessionStatisticsRow>();
            var PaticipantsPerYear = 0;
            var sessionscount = 0;

            if (string.IsNullOrEmpty(yearsList))
                return View(new SessionSummaryStatisticsViewModel());


            if (int.TryParse(yearsList, out yearInInt))
            {
                selectedyear = int.Parse(yearsList);
                //years = Enumerable.Range(2011, DateTime.Now.AddYears(1).Year - 2010).ToList();
                var sessionsForYear =
                    _sessionManager.GetAllSessionsForYear(selectedyear)
                    .Include(n => n.Activity)
                        .ToList()
                        .OrderBy(n => n.Id);

                sessionscount = sessionsForYear.Count();


                foreach (var session in sessionsForYear)
                {
                    PaticipantCount = _personManager.GetAllParticipantsForSession(session.Id).ToList().Count;
                    sessionStatisticsRowsList.Add(new SessionStatisticsRow
                    {
                        NumberOfParticipants = PaticipantCount,
                        Session = session
                    });

                    PaticipantsPerYear = PaticipantsPerYear + PaticipantCount;
                }
            }


            var viewModel = new SessionSummaryStatisticsViewModel
            {
                //Years = years,
                SessionStatisticsRows = sessionStatisticsRowsList,
                TotalPaticipants = PaticipantsPerYear,
                TotalSessions = sessionscount
            };

            return View(viewModel);
        }

        //[HttpGet]
        //public ActionResult FilterSessionsForTag()
        //{
        //    var viewModel1 = new SessionTagsViewModel()
        //    {

        //    };

        //    return View(viewModel1);

        //}

        [HttpGet]
        public ViewResult FilterSessionsForTag(string taglist, string id)
        {
            int Tag;
            int Id;
            tagDisplay = "-- Välj Etiketter--";

            
            if (int.TryParse(taglist, out Tag))
            {
                selectedTag = Tag;
         

                sessionsForTag =
                  _sessionManager.GetAllSessionsForTag(selectedTag)
                      .Include(n => n.Activity)
                      .OrderBy(n => n.Id);


                tagDisplay = "-- Välj Etiketter--";
                id = null;
            }

            if (int.TryParse(id, out Id))
            {
                selectedTag = Id;
                sessionsForTag =
                  _sessionManager.GetAllSessionsForTag(selectedTag)
                      .Include(n => n.Activity)
                      .OrderBy(n => n.Id);


                tagDisplay = _utilityManager.GetTag(selectedTag).Name;

                // IEnumerable<string> tagName =
                //_utilityManager.GetAllTags()
                //    .Where(n => n.Id == Id)
                //    .Select(a => a.Name);
            }


            var allTags = _utilityManager.GetAllTags().OrderBy(n => n.Name).ToList();
            var viewModel = new SessionTagsViewModel
            {
                tagName = tagDisplay,
                Tags = new SelectList(allTags, "Id", "Name"),
                Sessions = sessionsForTag
               
            };

            return View(viewModel);
        }
    }
}


