using System;
using System.Collections.Generic;
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
        private IActivityManager _activityManager;
        private IPersonManager _personManager;

        public ActivitySummaryController(IActivityManager manager, IPersonManager personManager)
        {
            _activityManager = manager;
            _personManager = personManager;

        }


        //// GET: Participant/Details/5
        //public ActionResult Details(int? id)
        //{

        //    Activity activity;
        //    if (id != null)
        //    {
        //        activity = _activityManager.GetAllActivities().SingleOrDefault(n => n.Id == id);
        //    }

        //    else
        //    {
        //        activity = _activityManager.GetAllActivities().OrderBy(n => n.Name).FirstOrDefault();
        //    }

        //    if (activity == null)
        //    {
        //        return HttpNotFound();
        //    }


        //    var viewModel = new ActivitySummaryViewModel()
        //    {

        //        Activities = new SelectList(
        //                                    _activityManager.GetAllActivities().OrderBy(n => n.Name),
        //                                    "Id",
        //                                    "Name",
        //                                    _activityManager.GetAllActivities().OrderBy(n => n.Name).First().Id),


        //    };

        //    return View(viewModel);
        //}

        //public ActionResult GetSessions(int activityId)
        //{

        //    var allSessionsForActivityResult = new List<Session>();
        //    var allSessionsForActivity = _activityManager.GetAllSessionsForActivity(activityId).OrderBy(n => n.Name).ToList();

        //    SelectList obgsessions;
        //    if (allSessionsForActivity.Count != 0)
        //    {

        //        List<Session> sessions = new List<Session>();
        //        sessions = _activityManager.GetAllSessionsForActivity(activityId).ToList();
        //        obgsessions = new SelectList(sessions, "Id", "Name", 0);

        //    }
        //    else
        //    {
        //        obgsessions = new SelectList(new List<SelectListItem>(), "Id", "Name", 0);
        //    }


        //    return Json(obgsessions);
        //}

        public ActionResult GetSession(int id)
        {
            var theSession = _activityManager.GetSessionByIdWithIncludes(id);

            if (theSession == null)
                return View("Error");

            var allParticipant = _activityManager.GetAllParticipantsForSession(id).ToList();
            var result = new ParticipantViewModel()
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

            };

            ViewBag.SessionParticipantId = new SelectList(
              _personManager.GetAllParticipants().OrderBy(n => n.FirstName),
              "Id",
              "FullName",
              _personManager.GetAllParticipants().OrderBy(n => n.FirstName).First().Id);

            return View("SessionParticipant", result);
        }

        public ViewResult FilterSessions(string searchString, string yearSlider, string hrPerson, string nameOfLocation)
        {
            IQueryable<Session> allSessions;
            if (string.IsNullOrEmpty(searchString))
                allSessions = _activityManager.GetAllSessionsWithIncludes();
            else
                allSessions = _activityManager.GetAllSessionsWithIncludes().Where(n => n.Name.Contains(searchString));
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
            if (_activityManager.SaveCommentsForSession(sessionId, comments))
                return Json(result, JsonRequestBehavior.AllowGet);

            // TODO: ErrorMessage
            result = new { Success = false };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveSessionEvaluation(int sessionId, string evaluation)
        {
            var result = new { Success = true };

            if (_activityManager.SaveEvaluationForSession(sessionId, evaluation))
                return Json(result, JsonRequestBehavior.AllowGet);

            result = new { Success = false };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult GetSessionsForActivity(int id)
        //{

        //    var allSessionsForActivityResult = new List<Session>();
        //    var allSessionsForActivity = _activityManager.GetAllSessionsForActivity(id).OrderBy(n => n.Name).ToList();
        //    var activity = _activityManager.GetActivityById(id);
        //    var sessionRowsList = new List<SessionRow>();

        //    if (allSessionsForActivity.Count == 0)
        //    {
        //        return PartialView("_NothingPartial");
        //    }

        //    foreach (var session in allSessionsForActivity)
        //    {
        //        sessionRowsList.Add(new SessionRow()
        //        {
        //            Id = session.Id,
        //            Name = session.Name,
        //            StartDate = session.StartDate,
        //            EndDate = session.EndDate,
        //        });

        //    }

        //    var result = new SessionViewModel()
        //    {
        //        SessionRows = sessionRowsList,
        //        ActivityName = activity.Name,
        //    };

        //    return View("Sessionsforactivity", result);
        //}

    }
}
