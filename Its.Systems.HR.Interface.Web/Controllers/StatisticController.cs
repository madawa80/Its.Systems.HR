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
            var participantsPerYearCount = 0;

            var sessionsForYear = _sessionManager.GetAllSessions()
                                    .Include(n => n.Activity)
                                    .OrderBy(n => n.Id)
                                    .ToList();

            var totalSessionCount = sessionsForYear.Count;
            var sessionStatisticsRowsList = new List<SessionStatisticsRow>();

            foreach (var session in sessionsForYear)
            {
                var participantCount = _personManager.GetAllParticipantsForSession(session.Id).ToList().Count;
                sessionStatisticsRowsList.Add(new SessionStatisticsRow
                {
                    NumberOfParticipants = participantCount,
                    Session = session
                });

                participantsPerYearCount += participantCount; 
            }

            var viewModel = new SessionSummaryStatisticsViewModel
            {
                SessionStatisticsRows = sessionStatisticsRowsList,
                TotalPaticipants = participantsPerYearCount,
                TotalSessions = totalSessionCount,
                SelectedYear = 0,
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult YearlyStatistics(string yearsList)
        {
            int yearInInt;
            var sessionStatisticsRowsList = new List<SessionStatisticsRow>();
            var PaticipantsPerYear = 0;
            var sessionscount = 0;

            if (string.IsNullOrEmpty(yearsList))
                return RedirectToAction("YearlyStatistics");


            if (int.TryParse(yearsList, out yearInInt))
            {
                selectedyear = yearInInt;
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
                TotalSessions = sessionscount,
                SelectedYear = selectedyear,
            };

            return View(viewModel);
        }

        [HttpGet]
        public ViewResult FilterSessionsForTag(string taglist, string id)
        {
            int tag;
            int totalParticipantCount = 0;
            int totalSessionCount = 0;
            string tagDisplay = "-- Välj etikett --";

            if (id != null && taglist == null)
                taglist = id;

            if (int.TryParse(taglist, out tag))
            {
                selectedTag = tag;

                sessionsForTag =
                  _sessionManager.GetAllSessionsForTag(selectedTag)
                      .Include(n => n.Activity)
                      .Include(n => n.SessionParticipants)
                      .OrderBy(n => n.Id);

                totalSessionCount = sessionsForTag.Count();
                tagDisplay = _utilityManager.GetTag(selectedTag).Name;

                foreach (var session in sessionsForTag)
                {
                    totalParticipantCount += session.SessionParticipants.Count;
                }
            }

            var allTags = _utilityManager.GetAllTags().OrderBy(n => n.Name).ToList();

            var viewModel = new SessionTagsViewModel
            {
                tagName = tagDisplay,
                Tags = new SelectList(allTags, "Id", "Name"),
                Sessions = sessionsForTag,
                TotalParticipants = totalParticipantCount,
                TotalSessions = totalSessionCount,
            };

            return View(viewModel);
        }
    }
}


