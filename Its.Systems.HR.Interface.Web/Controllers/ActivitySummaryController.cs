﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Its.Systems.HR.Interface.Web.ViewModels;
using Its.Systems.HR.Domain.Interfaces;
using Its.Systems.HR.Domain.Model;

namespace Its.Systems.HR.Interface.Web.Controllers
{
    public class ActivitySummaryController : Controller
    {
        private readonly IActivityManager _activityManager;
        private readonly ISessionManager _sessionManager;
        private readonly IPersonManager _personManager;
        private readonly IUtilityManager _utilityManager;

        public ActivitySummaryController(IActivityManager manager, ISessionManager sessionManager, IPersonManager personManager, IUtilityManager utilityManager)
        {
            _activityManager = manager;
            _sessionManager = sessionManager;
            _personManager = personManager;
            _utilityManager = utilityManager;
        }

        public ActionResult SessionForActivity(int id)
        {
            var theSession = _sessionManager.GetSessionByIdWithIncludes(id);

            if (theSession == null)
                return View("Error");

            var allParticipant = _personManager.GetAllParticipantsForSession(id).ToList();

            // Get Tags for session
            var sessionTagIdsForSession = theSession.SessionTags.Select(n => n.TagId);
            var allTagsForSession =
                _utilityManager.GetAllTags().Where(n => sessionTagIdsForSession.Contains(n.Id)).ToList();

            var result = new SessionForActivityViewModel()
            {
                Comments = theSession.Comments,
                Evaluation = theSession.Evaluation,
                StartDate = theSession.StartDate,
                EndDate = theSession.EndDate,
                HrPerson = theSession.HrPerson,
                Location = theSession.Location,
                TotalPaticipants = allParticipant.Count,
                Participants = allParticipant,
                SessionName = theSession.Name,
                SessionId = id,
                Tags = allTagsForSession
            };

            ViewBag.SessionParticipantId = new SelectList(
              _personManager.GetAllParticipants().OrderBy(n => n.FirstName),
              "Id",
              "FullName",
              _personManager.GetAllParticipants().OrderBy(n => n.FirstName).First().Id);

            return View(result);
        }

        public ViewResult FilterSessions(string searchString, string yearSlider, string hrPerson, string nameOfLocation)
        {
            IQueryable<Session> allSessions;
            if (string.IsNullOrEmpty(searchString))
                allSessions = _sessionManager.GetAllSessionsWithIncludes();
            else
                allSessions = _sessionManager.GetAllSessionsWithIncludes().Where(n => n.Name.Contains(searchString));
            // TODO: Take 10?

            int yearStart = 0;
            int yearEnd = 10000;
            if (!string.IsNullOrEmpty(yearSlider))
            {
                var years = yearSlider.Split(',');
                yearStart = int.Parse(years[0]);
                yearEnd = int.Parse(years[1]);

                allSessions = allSessions.Where(n => n.StartDate.Year >= yearStart && n.StartDate.Year <= yearEnd);
            }

            if (!string.IsNullOrEmpty(hrPerson))
            {
                var hrPersonAsInt = int.Parse(hrPerson);
                allSessions = allSessions.Where(n => n.HrPersonId == hrPersonAsInt); //TODO error handling
            }

            if (!string.IsNullOrEmpty(nameOfLocation))
            {
                allSessions = allSessions.Where(n => n.Location.Name == nameOfLocation);
            }


            var allHrPersons = _personManager.GetAllHrPersons().OrderBy(n => n.FirstName).ToList();
            var result = new FilterSessionsViewModel()
            {
                Sessions = allSessions.ToList(),
                HrPersons = new SelectList(allHrPersons, "Id", "FullName"),
                NameOfLocation = nameOfLocation,
                YearStart = yearStart,
                YearEnd = yearEnd,
                MinYear = 2011,
                MaxYear = DateTime.Now.AddYears(1).Year,    // Maximum year set to this year + 1
            };

            return View(result);
        }

        [HttpPost]
        public ActionResult SaveSessionComments(int sessionId, string comments)
        {
            var result = new { Success = true };
            if (_sessionManager.SaveCommentsForSession(sessionId, comments))
                return Json(result);

            // TODO: ErrorMessage
            result = new { Success = false };
            return Json(result);
        }

        [HttpPost]
        public ActionResult SaveSessionEvaluation(int sessionId, string evaluation)
        {
            var result = new { Success = true };

            if (_sessionManager.SaveEvaluationForSession(sessionId, evaluation))
                return Json(result);

            result = new { Success = false };
            return Json(result);
        }

       public ActionResult AllSessionsForActivity(int id)
        {
            var allSessionsForActivity = _sessionManager
                            .GetAllSessionsForActivity(id)
                            .Include(n => n.HrPerson)
                            .Include(n => n.Location)
                            .ToList();
            var activityName = _activityManager.GetActivityById(id).Name;

            var viewModel = new AllSessionsForActivityViewModel()
            {
                ActivityId = id,
                ActivityName = activityName,
                Sessions = allSessionsForActivity
            };
            return View(viewModel);
        }
    }
}
